CREATE OR ALTER PROCEDURE [Metadata].[Specification_GetDBName] @DataSetSpecificationId [UNIQUEIDENTIFIER] AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT d.[DataSetName] AS [DBName]
	FROM [Configuration].[DataSetSpecifications] s
	JOIN [Configuration].[DataSets] d ON s.[DataSetId] = d.[Id]
	WHERE s.[Id] = @DataSetSpecificationId;
END