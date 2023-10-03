CREATE OR ALTER FUNCTION [Configurations].[Objects_GetBaseMetadata] (@DataSetObjectId [UNIQUEIDENTIFIER]) RETURNS TABLE
AS
RETURN (
	SELECT o.[Id] as [DataSetObjectId], o.[DataSetSpecificationId], o.[SupplierObjectName], o.[OurObjectName] as [ConformedObjectName]
	FROM [Configurations].[DataSets] d
	JOIN [Configurations].[DataSetSpecifications] s ON s.[DataSetId] = d.[Id]
	JOIN [Configurations].[DataSetObjects] o ON o.[DataSetSpecificationId] = s.[Id]
	WHERE o.[Id] = @DataSetObjectId
)