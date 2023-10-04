CREATE OR ALTER PROCEDURE [Metadata].[Columns_GetNames] @SpecificationObjectId [UNIQUEIDENTIFIER],@ColumnNames [NVARCHAR](MAX) OUT AS
BEGIN
	SET NOCOUNT ON;
	
	SET @ColumnNames = (
	SELECT STRING_AGG(QUOTENAME([column_name]) + CHAR(13) + CHAR(10), CHAR(9) + ',')
	WITHIN GROUP (ORDER BY [OrdinalPosition])
	FROM [Configuration].[Columns_GetBaseMetadata] (@SpecificationObjectId)
	);
END