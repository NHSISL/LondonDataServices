// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "DataSets",
                columns: new[] { "Id", "ActiveFrom", "ActiveTo", "CollectedBy", "CreatedBy", "CreatedDate", "DataSetAliases", "DataSetAuthor", "DataSetName", "DataSourceType", "IsActive", "SpecifiedBy", "SupplierId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"), null, null, "EMIS", "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", "EMISDEV", "PrimaryCareEMISDEV", "", false, "EMIS", new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"), "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            var seedDataSchemaScript = @"
                IF NOT EXISTS (SELECT schema_name FROM information_schema.schemata WHERE schema_name = 'SeedData')
                BEGIN
                    EXEC('CREATE SCHEMA SeedData');
                END;";

            seedDataSchemaScript = seedDataSchemaScript.Replace("'", "''");
            migrationBuilder.Sql($"EXEC(N'{seedDataSchemaScript}')");

            var assembly = Assembly.GetExecutingAssembly();
            var sqlFiles = assembly.GetManifestResourceNames().Where(file => file.EndsWith(".sql"));

            foreach (var sqlFile in sqlFiles)
            {
                using (Stream stream = assembly.GetManifestResourceStream(sqlFile))
                using (StreamReader reader = new StreamReader(stream))
                {
                    var sqlScript = reader.ReadToEnd();
                    sqlScript = sqlScript.Replace("'", "''");
                    migrationBuilder.Sql($"EXEC(N'{sqlScript}')");
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Configuration",
                table: "DataSets",
                keyColumn: "Id",
                keyValue: new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"));
        }
    }
}
