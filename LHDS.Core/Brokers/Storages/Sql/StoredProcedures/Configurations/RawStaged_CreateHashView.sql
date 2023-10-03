CREATE OR ALTER PROCEDURE [Configurations].[RawStaged_CreateHashView] @DataSetObjectId [uniqueidentifier] AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Sql nvarchar(max)
		,@TableName nvarchar(255)
		,@ViewName nvarchar(255)
		,@BusinessKey nvarchar(max)
		,@VersionHashElements nvarchar(max)
		,@ColumnNames nvarchar(max);

	SELECT @TableName = '[RawStaged].[' + [OurObjectName] + '_' + CAST([DataSetSpecificationId] AS nchar(36)) + ']'
		,@ViewName = '[RawStaged].[' + [OurObjectName] + '_Hash_' + CAST([DataSetSpecificationId] AS nchar(36)) + ']'
	FROM [Configurations].[DataSetObjects]
	WHERE [Id] = @DataSetObjectId;
	
	EXEC [Configurations].[Columns_GetBusinessKey] @DataSetObjectId, @BusinessKey OUT;
	EXEC [Configurations].[Columns_GetVersionHashElements] @DataSetObjectId, @VersionHashElements OUT;
	EXEC [Configurations].[Columns_GetNames] @DataSetObjectId, @ColumnNames OUT;

	SET @Sql = '
CREATE VIEW ' + @ViewName + '
AS SELECT DISTINCT [LDSBusinessKey] = ' + @BusinessKey + '
,[LDSRecordHash] = CAST(HASHBYTES(''SHA2_256'', CONCAT_WS(''#'', ' + @VersionHashElements + ')) AS BINARY(32))
,' + @ColumnNames + 'FROM ' + @TableName + ';';

SELECT @Sql as SqlCommand, @ViewName as ObjectName;

END