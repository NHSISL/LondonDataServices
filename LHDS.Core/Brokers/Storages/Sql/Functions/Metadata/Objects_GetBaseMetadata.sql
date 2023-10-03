CREATE FUNCTION [Metadata].[Objects_GetBaseMetadata] (@SpecificationObjectId [UNIQUEIDENTIFIER]) RETURNS TABLE
AS
RETURN (
	SELECT o.[Id] as [SpecificationObjectId], o.[DataSetSpecificationId], o.[SupplierObjectName], o.[OurObjectName] as [ConformedObjectName]
	FROM [Configuration].[DataSets] d
	JOIN [Configuration].[DataSetSpecifications] s ON s.[DataSetId] = d.[Id]
	JOIN [Configuration].[SpecificationObjects] o ON o.[DataSetSpecificationId] = s.[Id]
	WHERE o.[Id] = @SpecificationObjectId
)