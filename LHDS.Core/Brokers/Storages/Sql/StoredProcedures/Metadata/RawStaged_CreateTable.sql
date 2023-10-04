CREATE OR ALTER PROCEDURE [Metadata].[RawStaged_CreateTable] @SpecificationObjectId [uniqueidentifier] AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Sql nvarchar(max)
		,@TableName nvarchar(255)
		,@ColumnNamesWithVarchar nvarchar(max);

	SELECT @TableName = '[RawStaged].[' + [OurObjectName] + '_' + CAST([DataSetSpecificationId] AS nchar(36)) + ']'
	FROM [Configuration].[SpecificationObjects]
	WHERE [Id] = @SpecificationObjectId;
	
	EXEC [Configuration].[Columns_GetNamesWithVarchar] @SpecificationObjectId, @ColumnNamesWithVarchar OUT;

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