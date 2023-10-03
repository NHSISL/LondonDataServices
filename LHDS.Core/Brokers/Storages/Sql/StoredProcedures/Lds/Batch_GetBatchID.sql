CREATE OR ALTER PROCEDURE [LDS].[Batch_GetBatchID] 
	@BusinessKey NVARCHAR(50),
	@SourceSystem VARCHAR(50)
AS 

BEGIN
	IF @BusinessKey IS NULL
		OR @SourceSystem IS NULL
	BEGIN
		DECLARE @ErrorMessage VARCHAR(255);

		SET @ErrorMessage = 'The BusinessKey and @SourceSystem cannot be set to NULL when generating a Id. Check input parameters';

		THROW 50001
			,@ErrorMessage
			,1;
	END;

	SET NOCOUNT ON;

	DECLARE @StartDateTime DATETIME2 = GETDATE();

	IF NOT EXISTS (
			SELECT 1
			FROM [LDS].[Batch]
			WHERE [BusinessKey] = @BusinessKey -- Source system batch identifier e.g. ProcessingId
				AND [SourceSystem] = @SourceSystem -- Source system name e.g. PrimaryCareEMIS
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
			@BusinessKey
			,@SourceSystem
			,'Started'
			,@StartDateTime
			)
	END;

	SELECT TOP 1 [Id]
		,[SourceSystem]
		,[Status]
	FROM [LDS].[Batch]
	WHERE [BusinessKey] = @BusinessKey
		AND [SourceSystem] = @SourceSystem
		AND [Status] = 'Started'
	--AND (@StartDateTime BETWEEN [StartDateTime] AND DATEADD(HOUR,24,[StartDateTime]))
	ORDER BY [StartDateTime];
END