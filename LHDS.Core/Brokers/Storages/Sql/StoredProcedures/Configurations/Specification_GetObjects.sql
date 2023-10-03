CREATE PROC [Configurations].[Specification_GetObjects] @DataSetSpecificationId [UNIQUEIDENTIFIER] AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT [Id] AS [DataSetObjectId]
	FROM [Configurations].[DataSetObjects]
	WHERE [DataSetSpecificationId] = @DataSetSpecificationId
	ORDER BY [SupplierObjectName];
END