CREATE PROC [Configurations].[Typecasted_CreateView] @DataSetObjectId [uniqueidentifier] AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Sql nvarchar(max)
		,@TableName nvarchar(255)
		,@MetadataTableName nvarchar(255)
		,@ViewName nvarchar(255)
		,@BusinessKey nvarchar(max)
		,@VersionHashElements nvarchar(max)
		,@ColumnNames nvarchar(max);

	SELECT @TableName = '[RawContinuum].[' + [OurObjectName] + '_' + CAST([DataSetSpecificationId] AS nchar(36)) + ']'
		,@MetadataTableName = '[RawContinuum].[' + [OurObjectName] + '_Metadata_' + CAST([DataSetSpecificationId] AS nchar(36)) + ']'
		,@ViewName = '[TypeCasted].[v' + [OurObjectName] + '_' + CAST([DataSetSpecificationId] AS nchar(36)) + ']'
	FROM [Configurations].[DataSetObjects]
	WHERE [Id] = @DataSetObjectId;
	
	EXEC [Configurations].[Columns_GetNamesTypecasted] @DataSetObjectId, @ColumnNames OUT;

	SET @Sql = '
CREATE VIEW ' + @ViewName + '
AS SELECT R1.[LDSBusinessKey],
    R1.[LDSRecordId],
	R1.[LDSRecordHash],
	R3.[RawStaged_EventID],' 
	+ @ColumnNames + 
'
FROM ' + @TableName + 'R1
INNER JOIN '+ @MetadataTableName +' R2
	ON R1.[LDSBusinessKey] = R2.[LDSBusinessKey]
		AND R1.[LDSRecordID] = R2.[LDSRecordID]
INNER JOIN [Log].[RawContinuum_Events] R3
	ON R2.[RawContinuum_EventID] = R3.[RawContinuum_EventID]';

SELECT @Sql as SqlCommand, @ViewName as ObjectName;

END