CREATE OR ALTER FUNCTION [Configurations].[Columns_GetBaseMetadata] (@DataSetObjectId [UNIQUEIDENTIFIER]) RETURNS TABLE
AS
RETURN (
	SELECT 
		c.[OrdinalPosition]
		,c.[OurColumnName] AS [column_name]
		,c.[SqlDataType] AS [data_type]
		,c.[Length]
		,c.[Scale] 
		,c.[Precision]
		,c.[IsEntityBusinessKey]
		,c.[IsVersionHashElement]
		,s.[Id] as DataSetSpecificationsId
		,d.[DataSetName]
		,o.[SupplierObjectName]
	FROM [Configurations].[DataSets] d
	JOIN [Configurations].[DataSetSpecifications] s ON s.[DataSetId] = d.[Id]
	JOIN [Configurations].[DataSetObjects] o ON o.[DataSetSpecificationId] = s.[Id]
	JOIN [Configurations].[ObjectColumns] c ON c.[DataSetObjectId]= o.[Id]
	WHERE o.[Id] = @DataSetObjectId
)