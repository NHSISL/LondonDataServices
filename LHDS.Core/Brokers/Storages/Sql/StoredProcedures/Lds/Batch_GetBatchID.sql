CREATE OR ALTER PROCEDURE [LDS].[Batch_GetBatchID] 
	@LDSBusinessKey NVARCHAR(50)
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
			SELECT 1
			FROM [LDS].[Batch]
			WHERE [LDSBusinessKey] = @LDSBusinessKey -- Source system batch identifier e.g. ProcessingId
				AND [LDSSourceSystem] = @LDSSourceSystem -- Source system name e.g. PrimaryCareEMIS
				AND [Status] = 'Started'
			)
		--AND (@StartDateTime BETWEEN [StartDateTime] AND DATEADD(HOUR,24,[StartDateTime])) -- No previous identical batches in the last 24 hours  
	BEGIN
		INSERT INTO [LDS].[Batch] (
			[LDSBusinessKey]
			,[LDSSourceSystem]
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

	SELECT TOP 1 [LDSBatchID]
		,[LDSSourceSystem]
		,[Status]
	FROM [LDS].[Batch]
	WHERE [LDSBusinessKey] = @LDSBusinessKey
		AND [LDSSourceSystem] = @LDSSourceSystem
		AND [Status] = 'Started'
	--AND (@StartDateTime BETWEEN [StartDateTime] AND DATEADD(HOUR,24,[StartDateTime]))
	ORDER BY [StartDateTime];
END