CREATE OR ALTER PROCEDURE [Configurations].[Object_GetHeaderFileName] @DataSetObjectId [UNIQUEIDENTIFIER] AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 'header_' + [SupplierObjectName] + '_' + CAST([DataSetSpecificationId] AS nchar(36)) + '.csv' AS [FileName]
	FROM [Configurations].[DataSetObjects]
	WHERE [Id] = @DataSetObjectId;
END