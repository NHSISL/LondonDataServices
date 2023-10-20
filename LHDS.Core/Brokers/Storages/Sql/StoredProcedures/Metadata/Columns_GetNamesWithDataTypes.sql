CREATE OR ALTER PROCEDURE [Metadata].[Columns_GetNamesWithDataTypes] @SpecificationObjectId [UNIQUEIDENTIFIER],@ColumnNamesWithDataTypes [NVARCHAR](MAX) OUT AS
BEGIN
	SET NOCOUNT ON;
	
	SET @ColumnNamesWithDataTypes = (
	SELECT STRING_AGG(CHAR(9) + QUOTENAME([column_name]) + ' [' +
		CASE 
		WHEN [data_type] IN ('varchar','nvarchar','char') THEN [data_type] + '](' + CAST([Length] AS nvarchar(10)) + ')' 
		WHEN [data_type] = 'decimal' THEN [data_type] + '](' + CAST([Precision] AS nvarchar(10)) + ',' + CAST([Scale] AS nvarchar(10)) + ')'
		ELSE [data_type] + ']'
		END + ' NULL'
		,',' + CHAR(13) + CHAR(10))
	WITHIN GROUP (ORDER BY [OrdinalPosition])
	FROM [Configuration].[Columns_GetBaseMetadata] (@SpecificationObjectId)
	);
END