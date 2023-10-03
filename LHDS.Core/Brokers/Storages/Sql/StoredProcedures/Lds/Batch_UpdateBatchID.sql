CREATE OR ALTER PROCEDURE [LDS].[Batch_UpdateBatchID] 
	@LDSBatchID UNIQUEIDENTIFIER
	,@Status VARCHAR(255)
	,@ErrorMessage VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [LDS].[Batch]
	SET [Status] = @Status
		,[ErrorMessage] = @ErrorMessage
		,[EndDateTime] = GETDATE()
	WHERE [Id] = @LDSBatchID;
END