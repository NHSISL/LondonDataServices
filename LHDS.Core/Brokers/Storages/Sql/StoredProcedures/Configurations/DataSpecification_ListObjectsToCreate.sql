CREATE OR ALTER PROCEDURE [Configurations].[DataSpecification_ListObjectsToCreate] @DataSetSpecificationId [uniqueidentifier] AS
BEGIN
	SET NOCOUNT ON;

	SELECT ROW_NUMBER() OVER(ORDER BY o.[OurObjectName]) AS [obj_seq], o.[Id] AS [obj_id], '[' + REPLACE(o.[OurObjectName],'.','].[') + '_' + CAST(@DataSetSpecificationId AS nchar(36)) + ']' AS [obj_name]
	FROM [Configurations].[DataSetSpecifications] s
	JOIN [Configurations].[DataSetObjects] o ON o.[DataSetSpecificationId] = s.[Id]
	WHERE s.[Id] = @DataSetSpecificationId;

END