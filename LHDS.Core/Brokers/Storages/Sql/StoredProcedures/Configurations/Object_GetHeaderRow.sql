CREATE OR ALTER PROCEDURE [Configurations].[Object_GetHeaderRow] @DataSetObjectId [UNIQUEIDENTIFIER] AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT STRING_AGG(QUOTENAME([column_name], '"'), ',') WITHIN GROUP (ORDER BY [OrdinalPosition]) AS [HeaderRow]
	FROM [Configurations].[Columns_GetBaseMetadata] (@DataSetObjectId);
END