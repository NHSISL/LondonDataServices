CREATE OR ALTER PROCEDURE [LDS].[Batch_GetBatchID] 
	@LDSBusinessKey VARCHAR(50),
	@LDSSourceSystem VARCHAR(50)
AS
BEGIN
	IF @LDSBusinessKey IS NULL
		OR @LDSSourceSystem IS NULL
	BEGIN
		DECLARE @ErrorMessage VARCHAR(255);

		SET @ErrorMessage = 'The @LDSBusinessKey and @LDSSourceSystem cannot be set to NULL when generating a LDSBatchID. Check input parameters';

		THROW 50001
			,@ErrorMessage
			,1;
	END;

	SET NOCOUNT ON;

	DECLARE @StartDateTime DATETIME2 = GETDATE();

	IF NOT EXISTS (
			SELECT TOP 1
				[Id] as [LDSBatchID]
				,[BusinessKey] as [LDSBusinessKey]
				,[SourceSystem] as [LDSSourceSystem]
				,[Status]
				,[ErrorMessage]
				,[StartDateTime]
				,[EndDateTime]
			FROM [LDS].[Batch]
			WHERE [BusinessKey] = @LDSBusinessKey -- Source system batch identifier e.g. ProcessingId
				AND [SourceSystem] = @LDSSourceSystem -- Source system name e.g. PrimaryCareEMIS
				AND [Status] = 'Started'
			)
		--AND (@StartDateTime BETWEEN [StartDateTime] AND DATEADD(HOUR,24,[StartDateTime])) -- No previous identical batches in the last 24 hours  
	BEGIN
		INSERT INTO [LDS].[Batch] (
			[BusinessKey]
			,[SourceSystem]
			,[Status]
			,[StartDateTime]
			)
		VALUES (
			@LDSBusinessKey
			,@LDSSourceSystem
			,'Started'
			,@StartDateTime
			)
	END;

	SELECT TOP 1 
		[Id] as [LDSBatchID]
		,[SourceSystem] as [LDSSourceSystem]
		,[Status]
	FROM [LDS].[Batch]
	WHERE [BusinessKey] = @LDSBusinessKey
		AND [SourceSystem] = @LDSSourceSystem
		AND [Status] = 'Started'
	--AND (@StartDateTime BETWEEN [StartDateTime] AND DATEADD(HOUR,24,[StartDateTime]))
	ORDER BY [StartDateTime];
END