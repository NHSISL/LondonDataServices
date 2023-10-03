CREATE OR ALTER FUNCTION [Metadata].[Columns_GetBaseMetadata] (@SpecificationObjectId [UNIQUEIDENTIFIER]) RETURNS TABLE
AS
RETURN (
	SELECT 
		c.[OrdinalPosition]
		,c.[OurColumnName] AS [column_name]
		,c.[SqlDataType] AS [data_type]
		,c.[Length]
		,c.[Scale] 
		,c.[Precision]
		,c.[IsBusinessKey]
		,c.[IsVersionHashElement]
		,s.[Id] as DataSetSpecificationsId
		,d.[DataSetName]
		,o.[SupplierObjectName]
	FROM [Configuration].[DataSets] d
	JOIN [Configuration].[DataSetSpecifications] s ON s.[DataSetId] = d.[Id]
	JOIN [Configuration].[SpecificationObjects] o ON o.[DataSetSpecificationId] = s.[Id]
	JOIN [Configuration].[ObjectColumns] c ON c.[SpecificationObjectId] = o.[Id]
	WHERE o.[Id] = @SpecificationObjectId
)