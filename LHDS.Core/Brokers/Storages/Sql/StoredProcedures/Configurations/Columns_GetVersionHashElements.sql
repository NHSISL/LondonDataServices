CREATE OR ALTER PROCEDURE [Configurations].[Columns_GetVersionHashElements] @DataSetObjectId [UNIQUEIDENTIFIER],@VersionHashElements [NVARCHAR](MAX) OUT AS
BEGIN
	SET NOCOUNT ON;
	
	SET @VersionHashElements = (
	SELECT STRING_AGG(QUOTENAME([column_name]), ', ')
	WITHIN GROUP (ORDER BY [OrdinalPosition])
	FROM  [Configurations].[Columns_GetBaseMetadata] (@DataSetObjectId)
	WHERE [IsVersionHashElement] = 1 -- columns that need to be included in LDSRecordHash
	);
END