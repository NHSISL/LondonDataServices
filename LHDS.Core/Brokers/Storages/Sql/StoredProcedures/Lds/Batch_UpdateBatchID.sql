CREATE OR ALTER PROCEDURE [LDS].[Batch_UpdateBatchID]
	@Id UNIQUEIDENTIFIER
	,@Status NVARCHAR(255)
	,@ErrorMessage NVARCHAR(255)
AS

BEGIN
	SET NOCOUNT ON;

	UPDATE [LDS].[Batch]
	SET [Status] = @Status
		,[ErrorMessage] = @ErrorMessage
		,[EndDateTime] = GETDATE()
	WHERE [Id] = @Id;
END