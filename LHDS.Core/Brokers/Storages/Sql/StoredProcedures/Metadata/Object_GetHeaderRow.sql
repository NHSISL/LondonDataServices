CREATE OR ALTER PROCEDURE [Metadata].[Object_GetHeaderRow] @SpecificationObjectId [UNIQUEIDENTIFIER] AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT STRING_AGG(QUOTENAME([column_name], '"'), ',') WITHIN GROUP (ORDER BY [OrdinalPosition]) AS [HeaderRow]
	FROM [Configuration].[Columns_GetBaseMetadata] (@SpecificationObjectId);
END