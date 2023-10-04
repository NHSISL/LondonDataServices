CREATE OR ALTER PROCEDURE [Metadata].[RawContinuum_CreateMainTable] @SpecificationObjectId [uniqueidentifier] AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Sql nvarchar(max)
		,@TableName nvarchar(255)
		,@ColumnNamesWithDataTypes nvarchar(max);

	SELECT @TableName = '[RawContinuum].[' + [OurObjectName] + '_' + CAST([DataSetSpecificationId] AS nchar(36)) + ']'
	FROM [Configuration].[SpecificationObjects]
	WHERE [Id] = @SpecificationObjectId;

	EXEC [Configuration].[Columns_GetNamesWithVarchar] @SpecificationObjectId, @ColumnNamesWithDataTypes OUT;

	SET @Sql = '
CREATE TABLE ' + @TableName + '
(
	[LDSBusinessKey] [varchar](8000) NOT NULL,
	[LDSRecordId] [uniqueidentifier] NOT NULL,
	[LDSRecordHash] [binary](32) NOT NULL,
' + @ColumnNamesWithDataTypes + '
)
WITH
(
	DISTRIBUTION = HASH ( [LDSBusinessKey] ),
	CLUSTERED COLUMNSTORE INDEX
)';

SELECT @Sql as SqlCommand,@TableName as ObjectName;

END