CREATE PROC [Configurations].[RawContinuum_CreateMetadataTable] @DataSetObjectId [uniqueidentifier] AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Sql nvarchar(max)
		,@TableName nvarchar(255);

	SELECT @TableName = '[RawContinuum].[' + [OurObjectName] + '_Metadata_' + CAST([DataSetSpecificationId] AS nchar(36)) + ']'
	FROM [Configurations].[DataSetObjects]
	WHERE [Id] = @DataSetObjectId;
	
	SET @Sql = '
CREATE TABLE ' + @TableName + '
(
	[LDSBusinessKey] [varchar](8000) NOT NULL,
	[LDSRecordId] [uniqueidentifier] NOT NULL,
	[LDSRecordHash] [binary](32) NOT NULL,
	[ExtractStartTime] [datetime] NOT NULL,
	[ProcessDateTime] [datetime] NOT NULL,
	[RawContinuum_EventID] [uniqueidentifier] NOT NULL
)
WITH
(
	DISTRIBUTION = HASH ( [LDSBusinessKey] ),
	CLUSTERED COLUMNSTORE INDEX
)';

SELECT @Sql as SqlCommand,@TableName as ObjectName;

END