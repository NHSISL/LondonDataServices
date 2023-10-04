CREATE OR ALTER PROCEDURE [Metadata].[Object_GetHeaderFileName] @SpecificationObjectId [UNIQUEIDENTIFIER] AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 'header_' + [SupplierObjectName] + '_' + CAST([DataSetSpecificationId] AS nchar(36)) + '.csv' AS [FileName]
	FROM [Configuration].[SpecificationObjects]
	WHERE [Id] = @SpecificationObjectId;
END