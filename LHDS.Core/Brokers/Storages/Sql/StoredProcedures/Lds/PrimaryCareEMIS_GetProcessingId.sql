CREATE OR ALTER PROCEDURE [LDS].[PrimaryCareEMIS_GetProcessingId] 
    @SourceFileName NVARCHAR(128)
AS
BEGIN
    SELECT SplitValue AS ProcessingId
    FROM [LDS].[SplitString](@SourceFileName, '_')
    WHERE SplitOrdinal = 2
END