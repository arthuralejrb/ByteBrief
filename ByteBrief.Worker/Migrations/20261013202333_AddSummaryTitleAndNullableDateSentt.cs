using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteBrief.Worker.Migrations
{
    /// <inheritdoc />
    public partial class AddSummaryTitleAndNullableDateSentt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataSent",
                table: "NewsArticles",
                newName: "DateSent");

            migrationBuilder.RenameColumn(
                name: "DataFetched",
                table: "NewsArticles",
                newName: "DateFetched");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateSent",
                table: "NewsArticles",
                newName: "DataSent");

            migrationBuilder.RenameColumn(
                name: "DateFetched",
                table: "NewsArticles",
                newName: "DataFetched");
        }
    }
}
