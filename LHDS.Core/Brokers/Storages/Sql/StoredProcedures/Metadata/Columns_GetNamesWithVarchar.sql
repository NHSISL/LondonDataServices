CREATE OR ALTER PROCEDURE [Metadata].[Columns_GetNamesWithVarchar] @SpecificationObjectId [UNIQUEIDENTIFIER],@ColumnNamesWithVarchar [NVARCHAR](MAX) OUT AS
BEGIN
	SET NOCOUNT ON;
	
	SET @ColumnNamesWithVarchar = (
	SELECT STRING_AGG(CHAR(9) + QUOTENAME([column_name]) + ' [varchar](' + CASE WHEN [Length] <= 255 THEN '255' ELSE '500' END + ') NULL', ',' + CHAR(13) + CHAR(10))
	WITHIN GROUP (ORDER BY [OrdinalPosition])
	FROM [Configuration].[Columns_GetBaseMetadata] (@SpecificationObjectId)
	);
END