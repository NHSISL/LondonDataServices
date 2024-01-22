// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMetaDataSchemaItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var seedDataSchemaScript = @"
                IF OBJECT_ID('[LDS].[Batch_GetBatchID]', 'P') IS NOT NULL
                    DROP PROCEDURE [LDS].[Batch_GetBatchID];

                IF OBJECT_ID('[LDS].[Batch_UpdateBatchID]', 'P') IS NOT NULL
                    DROP PROCEDURE [LDS].[Batch_UpdateBatchID];

                IF OBJECT_ID('[LDS].[PrimaryCareEMIS_GetProcessingId]', 'P') IS NOT NULL
                    DROP PROCEDURE [LDS].[PrimaryCareEMIS_GetProcessingId];

                IF OBJECT_ID('[LDS].[SplitString]', 'FN') IS NOT NULL
                    DROP FUNCTION [LDS].[SplitString];

                IF OBJECT_ID('[LDS].[Batch]', 'U') IS NOT NULL
                BEGIN
                    ALTER TABLE [LDS].[Batch] SET ( SYSTEM_VERSIONING = OFF  )
                    DROP TABLE [LDS].[Batch];
                END

                IF OBJECT_ID('[LDS].[BatchHistory]', 'U') IS NOT NULL
                    DROP TABLE [LDS].[BatchHistory];

                IF SCHEMA_ID('LDS') IS NOT NULL
                    DROP SCHEMA [LDS];

                IF OBJECT_ID('[Metadata].[Columns_GetNames]', 'P') IS NOT NULL
                    DROP PROCEDURE [Metadata].[Columns_GetNames];

                IF OBJECT_ID('[Metadata].[Columns_GetNamesTypecasted]', 'P') IS NOT NULL
                    DROP PROCEDURE [Metadata].[Columns_GetNamesTypecasted];

                -- Add similar checks for other procedures and functions

                IF OBJECT_ID('[Metadata].[Columns_GetBaseMetadata]', 'FN') IS NOT NULL
                    DROP FUNCTION [Metadata].[Columns_GetBaseMetadata];

                IF OBJECT_ID('[Metadata].[Objects_GetBaseMetadata]', 'FN') IS NOT NULL
                    DROP FUNCTION [Metadata].[Objects_GetBaseMetadata];

                IF SCHEMA_ID('Metadata') IS NOT NULL
                    DROP SCHEMA [Metadata];
            ";

            seedDataSchemaScript = seedDataSchemaScript.Replace("'", "''");
            migrationBuilder.Sql($"EXEC(N'{seedDataSchemaScript}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
