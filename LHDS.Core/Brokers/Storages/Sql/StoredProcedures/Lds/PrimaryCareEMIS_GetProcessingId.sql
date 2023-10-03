CREATE OR ALTER PROCEDURE [LDS].[PrimaryCareEMIS_GetProcessingId] 
	@SourceFileName NVARCHAR(128)
AS

BEGIN
	SELECT [value] AS ProcessingId
	FROM STRING_SPLIT(@SourceFileName, '_', 1)
	WHERE ordinal = 2
END