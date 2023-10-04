CREATE OR ALTER PROCEDURE [Metadata].[Columns_GetVersionHashElements] @SpecificationObjectId [UNIQUEIDENTIFIER],@VersionHashElements [NVARCHAR](MAX) OUT AS
BEGIN
	SET NOCOUNT ON;
	
	SET @VersionHashElements = (
	SELECT STRING_AGG(QUOTENAME([column_name]), ', ')
	WITHIN GROUP (ORDER BY [OrdinalPosition])
	FROM  [Configuration].[Columns_GetBaseMetadata] (@SpecificationObjectId)
	WHERE [IsVersionHashElement] = 1 -- columns that need to be included in LDSRecordHash
	);
END