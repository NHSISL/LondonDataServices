CREATE PROC [Configurations].[RawStaged_CreateTable] @DataSetObjectId [uniqueidentifier] AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Sql nvarchar(max)
		,@TableName nvarchar(255)
		,@ColumnNamesWithVarchar nvarchar(max);

	SELECT @TableName = '[RawStaged].[' + [OurObjectName] + '_' + CAST([DataSetSpecificationId] AS nchar(36)) + ']'
	FROM [Configurations].[DataSetObjects]
	WHERE [Id] = @DataSetObjectId;
	
	EXEC [Configurations].[Columns_GetNamesWithVarchar] @DataSetObjectId, @ColumnNamesWithVarchar OUT;

	SET @Sql = '
CREATE TABLE ' + @TableName + '
(
' + @ColumnNamesWithVarchar + '
)
WITH
(
	DISTRIBUTION = ROUND_ROBIN,
	HEAP
)';

SELECT @Sql as SqlCommand,@TableName as ObjectName;

END