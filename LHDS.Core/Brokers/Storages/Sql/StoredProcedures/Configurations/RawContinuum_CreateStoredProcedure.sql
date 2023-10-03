CREATE OR ALTER PROCEDURE [Configurations].[RawContinuum_CreateStoredProcedure] @DataSetObjectId [uniqueidentifier] AS

BEGIN

SET NOCOUNT ON;

--DECLARE @DataSetObjectId UNIQUEIDENTIFIER = '28A1F2DF-0DEF-46DA-B319-A08D29B9C7B6';
DECLARE @SQL_RawContinuum_Proc_Template NVARCHAR(MAX);
DECLARE @DataSetSpecificationId UNIQUEIDENTIFIER;
DECLARE @SupplierObjectName NVARCHAR(MAX);
DECLARE @RawContinuumProcedureName NVARCHAR(MAX);
DECLARE @RawContinuumMainTableName NVARCHAR(MAX);
DECLARE @RawContinuumTempTableName NVARCHAR(MAX);
DECLARE @RawContinuumMetadataTableName NVARCHAR(MAX);
DECLARE @RawStagedHashView NVARCHAR(MAX);

-- Get Object Names
SELECT @SupplierObjectName =  [SupplierObjectName], @DataSetSpecificationId = [DataSetSpecificationId]
FROM [Configurations].[Objects_GetBaseMetadata](@DataSetObjectId)

-- to do check @SupplierObjectName and @DataSetSpecificationId is set

SET @RawContinuumProcedureName =  CONCAT('[RawContinuum].[',@SupplierObjectName,'_Process_',@DataSetSpecificationId,']');
SET @RawContinuumMainTableName = CONCAT('[RawContinuum].[',@SupplierObjectName,'_',@DataSetSpecificationId,']');
SET @RawContinuumTempTableName = CONCAT('[RawContinuum].[',@SupplierObjectName,'_',@DataSetSpecificationId,'_Temp]');
SET @RawContinuumMetadataTableName = CONCAT('[RawContinuum].[',@SupplierObjectName,'_Metadata_',@DataSetSpecificationId,']');
SET @RawStagedHashView = CONCAT('[RawStaged].[',@SupplierObjectName,'_Hash_',@DataSetSpecificationId,']');

--SELECT @RawContinuumProcedureName,@RawContinuumMainTableName,@RawContinuumTempTableName,@RawContinuumMetadataTableName,@RawStagedHashView;

-- Get Column Names
DECLARE @ColumnNames VARCHAR(MAX)
EXEC [Configurations].[Columns_GetNames] @DataSetObjectId, @ColumnNames OUT

--SELECT @ColumnNames;

SET @SQL_RawContinuum_Proc_Template =

'
CREATE PROC '+ @RawContinuumProcedureName +' @RawStaged_EventId [uniqueidentifier] AS
BEGIN
	SET NOCOUNT ON;

	-- Log process started
	DECLARE @RawContinuum_EventId UNIQUEIDENTIFIER;

	EXEC [Log].[RawContinuum_InsertEvent] @RawContinuum_EventId OUTPUT
		,@RawStaged_EventId = @RawStaged_EventId
		,@SourceSchema = ''RawStaged''
		,@SourceTable = '''+ @SupplierObjectName +'''
		,@DestinationSchema = ''RawContinuum''
		,@DestinationTable = '''+ @SupplierObjectName +''';

   BEGIN TRY
		DECLARE @ExtractStartTime AS DATETIME2;
		DECLARE @ProcessingId AS INT;

		-- workaround
		SET @ExtractStartTime = (SELECT TOP 1 SourceFileExtractStartTime FROM [Log].[RawStaged_Events] WHERE [RawStaged_EventId] = @RawStaged_EventId);
		SET @ProcessingId = (SELECT TOP 1 SourceFileProcessingId FROM [Log].[RawStaged_Events] WHERE [RawStaged_EventId] = @RawStaged_EventId);


		-- Drop _Temp table if exists
		IF OBJECT_ID(N''' + @RawContinuumTempTableName +''') IS NOT NULL
		BEGIN
			DROP TABLE ' + @RawContinuumTempTableName + '
		END;

		-- Create Hash Distributed _Temp Table from _Hash view
		CREATE TABLE '+ @RawContinuumTempTableName +'
			WITH (
					DISTRIBUTION = HASH ([LDSBusinessKey])
					,CLUSTERED COLUMNSTORE INDEX
					) AS

		SELECT
			-- Data Processing Columns
			[LDSBusinessKey]
			,[LDSRecordId] = NEWID()
			,[LDSRecordHash]
			,[ExtractStartTime] = @ExtractStartTime
			,[ProcessDateTime] = GETDATE()
			,[RawContinuum_EventId] = @RawContinuum_EventId

			-- Source Data Columns
			,'+ @ColumnNames +'

		FROM '+ @RawStagedHashView +' H1
		--WHERE [ProcessingId] = @ProcessingId
		OPTION (LABEL = ''RAWCONTINUUM TEMP - Create '+ @RawContinuumTempTableName  +' '');-- safety feature for landing multiple files concurrently and duff file names

		-- Declare and set logging variables
		DECLARE @SourceTableRowCount BIGINT
			,@DestinationTableInsertCount BIGINT
			,@DestinationTableUpdateCount BIGINT;

		-- Faster row count of last inserted table
		SET @SourceTableRowCount = (SELECT SUM(ISNULL([S].[row_count], 0))
		FROM [sys].[dm_pdw_request_steps] AS [S]
		WHERE [S].[operation_type] = ''OnOperation''
			AND [S].[row_count] <> - 1
			AND [S].[request_id] IN (
				SELECT TOP (1) [R].[request_id]
				FROM [sys].[dm_pdw_exec_requests] AS [R]
				WHERE [R].[session_id] = SESSION_ID()
					AND [R].[resource_class] IS NOT NULL
				ORDER BY [R].[end_time] DESC
				))

		SET @DestinationTableInsertCount = (
				SELECT COUNT(*)
				FROM '+ @RawContinuumTempTableName +' T1
				WHERE NOT EXISTS -- New Records
					(
						SELECT 1
						FROM '+ @RawContinuumMainTableName +' R1
						WHERE T1.[LDSBusinessKey] = R1.[LDSBusinessKey]
						)
				);
		SET @DestinationTableUpdateCount = (
				SELECT COUNT(*)
				FROM '+ @RawContinuumTempTableName +' T1
				WHERE EXISTS -- Updated Records
					(
						SELECT 1
						FROM '+ @RawContinuumMainTableName +' R1
						WHERE T1.[LDSBusinessKey] = R1.[LDSBusinessKey]
							AND T1.[LDSRecordHash] <> R1.[LDSRecordHash]
						)
				);

		BEGIN TRANSACTION

		-- Insert into metadata with LDSRecordId
		INSERT INTO '+ @RawContinuumMetadataTableName +' (
			[LDSBusinessKey]
			,[LDSRecordId]
			,[LDSRecordHash]
			,[ExtractStartTime]
			,[ProcessDateTime]
			,[RawContinuum_EventId]
			)
		SELECT [LDSBusinessKey]
			,[LDSRecordId]
			,[LDSRecordHash]
			,[ExtractStartTime]
			,[ProcessDateTime]
			,[RawContinuum_EventId]
		FROM '+ @RawContinuumTempTableName +' T1
		WHERE NOT EXISTS -- Prevent same file being loaded into _Meta n times. Not sure if this is required. 
			(
				SELECT 1
				FROM '+ @RawContinuumMetadataTableName +' M1
				WHERE T1.[RawContinuum_EventId] = M1.[RawContinuum_EventId]
					AND T1.[LDSBusinessKey] = M1.[LDSBusinessKey]
					AND T1.[LDSRecordHash] = M1.[LDSRecordHash]
				)
		OPTION (LABEL = ''RAWCONTINUUM METADATA - Insert '+ @RawContinuumMetadataTableName +''');

		---- Insert main data 
		INSERT INTO '+ @RawContinuumMainTableName +' (
			[LDSBusinessKey]
			,[LDSRecordId]
			,[LDSRecordHash]

			-- Source Data Columns
			,'+ @ColumnNames +'

			)
		SELECT [LDSBusinessKey]
			,[LDSRecordId]
			,[LDSRecordHash]

			-- Source Data Columns
			,'+ @ColumnNames +'

		FROM '+ @RawContinuumTempTableName +' T1
		WHERE NOT EXISTS -- New & Updated Records
			(
				SELECT 1
				FROM '+ @RawContinuumMainTableName  +' R1
				WHERE T1.[LDSBusinessKey] = R1.[LDSBusinessKey]
					AND T1.[LDSRecordHash] = R1.[LDSRecordHash]
				)
		OPTION (LABEL = ''RAWCONTINUUM MAIN - Insert '+ @RawContinuumMainTableName +''');

		COMMIT TRANSACTION

		-- declare and set final logging variable and log counts
		DECLARE @DestinationTableRowCount BIGINT

		SET @DestinationTableRowCount = (
				SELECT COUNT(*)
				FROM '+ @RawContinuumMainTableName +'
				);

		EXEC [Log].[RawContinuum_UpdateEvent] @RawContinuum_EventId = @RawContinuum_EventId
			,@SourceTableRowCount = @SourceTableRowCount
			,@DestinationTableInsertCount = @DestinationTableInsertCount
			,@DestinationTableUpdateCount = @DestinationTableUpdateCount
			,@DestinationTableRowCount = @DestinationTableRowCount
			,@Status = ''Succeeded''
			,@ErrorMessage = NULL;

	END TRY

	BEGIN CATCH
		IF @@trancount > 0
			ROLLBACK TRANSACTION

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SET @ErrorMessage = (SELECT ERROR_MESSAGE());
		SET @ErrorSeverity = (SELECT  ERROR_SEVERITY());
		SET @ErrorState = (SELECT ERROR_STATE());

		-- Log error
		EXEC [Log].[RawContinuum_UpdateEvent] @RawContinuum_EventId = @RawContinuum_EventId
			,@SourceTableRowCount = NULL
			,@DestinationTableInsertCount = NULL
			,@DestinationTableUpdateCount = NULL
			,@DestinationTableRowCount = NULL
			,@Status = ''Failed''
			,@ErrorMessage = @ErrorMessage;

		-- Throw error back to ADF
		THROW;

	END CATCH

	-- Finally drop _Temp table if exists
	IF OBJECT_ID(N'''+@RawContinuumTempTableName +''') IS NOT NULL
	BEGIN
		DROP TABLE '+ @RawContinuumTempTableName +'
	END;
END;
'
--Debug
--SELECT @SQL_RawContinuum_Proc_Template, SQLLength = LEN(@SQL_RawContinuum_Proc_Template);

--IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@RawContinuumProcedureName) AND type in (N'P'))
	--EXEC(@SQL_EMIS_RawContinuum_Proc_Template);
	--EXEC [Configurations].[ExecSql] @DataSetObjectId, 'RawContinuum_CreateStoredProcedure', @SQL_RawContinuum_Proc_Template;

SELECT @SQL_RawContinuum_Proc_Template as SqlCommand, @RawContinuumProcedureName as ObjectName;

END