CREATE OR ALTER PROCEDURE [Metadata].[Specification_GetObjects] @DataSetSpecificationId [UNIQUEIDENTIFIER] AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT [Id] AS [SpecificationObjectId]
	FROM [Configuration].[SpecificationObjects]
	WHERE [DataSetSpecificationId] = @DataSetSpecificationId
	ORDER BY [SupplierObjectName];
END