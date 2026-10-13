using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteBrief.Worker.Migrations
{
    /// <inheritdoc />
    public partial class AddSummaryTitleAndNullableDateSent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataSent",
                table: "NewsArticles",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "NewsArticles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "NewsArticles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "NewsArticles");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataSent",
                table: "NewsArticles",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);
        }
    }
}
