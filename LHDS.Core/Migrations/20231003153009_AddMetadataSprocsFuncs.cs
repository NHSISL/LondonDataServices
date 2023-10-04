// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddMetadataSprocsFuncs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var schemaScript = @"
                IF NOT EXISTS (SELECT schema_name FROM information_schema.schemata WHERE schema_name = 'Metadata')
                BEGIN
                    EXEC('CREATE SCHEMA Metadata');
                END;";

            schemaScript = schemaScript.Replace("'", "''");
            migrationBuilder.Sql($"EXEC(N'{schemaScript}')");

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
    }
}
