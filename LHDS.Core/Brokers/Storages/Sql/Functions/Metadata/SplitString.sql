CREATE OR ALTER FUNCTION [LDS].[SplitString]
(
    @InputString NVARCHAR(MAX),
    @Delimiter NVARCHAR(1)
)
RETURNS TABLE
AS
RETURN (
    WITH SplitStrings AS
    (
        SELECT 
            value AS SplitValue,
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS SplitOrdinal
        FROM STRING_SPLIT(@InputString, @Delimiter)
    )
    SELECT SplitValue, SplitOrdinal
    FROM SplitStrings
);