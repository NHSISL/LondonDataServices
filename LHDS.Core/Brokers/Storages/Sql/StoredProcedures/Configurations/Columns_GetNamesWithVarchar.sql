CREATE OR ALTER PROCEDURE [Configurations].[Columns_GetNamesWithVarchar] @DataSetObjectId [UNIQUEIDENTIFIER],@ColumnNamesWithVarchar [NVARCHAR](MAX) OUT AS
BEGIN
	SET NOCOUNT ON;
	
	SET @ColumnNamesWithVarchar = (
	SELECT STRING_AGG(CHAR(9) + QUOTENAME([column_name]) + ' [varchar](' + CASE WHEN [Length] <= 255 THEN '255' ELSE '500' END + ') NULL', ',' + CHAR(13) + CHAR(10))
	WITHIN GROUP (ORDER BY [OrdinalPosition])
	FROM [Configurations].[Columns_GetBaseMetadata] (@DataSetObjectId)
	);
END