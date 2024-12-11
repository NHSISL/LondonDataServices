// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class ObjectColumnsModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForeignKeyColumnName",
                schema: "Configuration",
                table: "ObjectColumns",
                type: "nvarchar(max)",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<string>(
                name: "ForeignKeyTableName",
                schema: "Configuration",
                table: "ObjectColumns",
                type: "nvarchar(max)",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<bool>(
                name: "IsForeignKey",
                schema: "Configuration",
                table: "ObjectColumns",
                type: "bit",
                nullable: false,
                defaultValue: false)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeignKeyColumnName",
                schema: "Configuration",
                table: "ObjectColumns")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "ForeignKeyTableName",
                schema: "Configuration",
                table: "ObjectColumns")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "IsForeignKey",
                schema: "Configuration",
                table: "ObjectColumns")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
