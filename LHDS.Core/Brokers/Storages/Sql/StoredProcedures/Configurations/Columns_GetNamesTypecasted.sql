CREATE OR ALTER PROCEDURE [Configurations].[Columns_GetNamesTypecasted] @DataSetObjectId [UNIQUEIDENTIFIER],@ColumnNamesWithDataTypes [NVARCHAR](MAX) OUT AS
BEGIN
	SET NOCOUNT ON;
	
	SET @ColumnNamesWithDataTypes = (
	SELECT STRING_AGG(CHAR(9) + QUOTENAME([column_name]) + ' = TRY_CAST(' + QUOTENAME([column_name]) + ' AS [' +
		CASE 
		WHEN [data_type] IN ('varchar','nvarchar','char') THEN [data_type] + '](' + CAST([Length] AS nvarchar(10)) + ')' 
		WHEN [data_type] = 'decimal' THEN [data_type] + '](' + CAST([Precision] AS nvarchar(10)) + ',' + CAST([Scale] AS nvarchar(10)) + ')'
		ELSE [data_type] + ']'
		END + ')'
		,',' + CHAR(13) + CHAR(10))
	WITHIN GROUP (ORDER BY [OrdinalPosition])
	FROM [Configurations].[Columns_GetBaseMetadata] (@DataSetObjectId)
	);
END