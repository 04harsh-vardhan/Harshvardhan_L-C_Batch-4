using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregation.Migrations
{
    /// <inheritdoc />
    public partial class added_report_count_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "Articles",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Article_Url",
                table: "Articles",
                newName: "article_url");

            migrationBuilder.RenameColumn(
                name: "Article_Title",
                table: "Articles",
                newName: "article_title");

            migrationBuilder.RenameColumn(
                name: "Article_Source",
                table: "Articles",
                newName: "article_source");

            migrationBuilder.RenameColumn(
                name: "Article_Description",
                table: "Articles",
                newName: "article_description");

            migrationBuilder.RenameColumn(
                name: "Article_Id",
                table: "Articles",
                newName: "article_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Articles",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "article_url",
                table: "Articles",
                newName: "Article_Url");

            migrationBuilder.RenameColumn(
                name: "article_title",
                table: "Articles",
                newName: "Article_Title");

            migrationBuilder.RenameColumn(
                name: "article_source",
                table: "Articles",
                newName: "Article_Source");

            migrationBuilder.RenameColumn(
                name: "article_description",
                table: "Articles",
                newName: "Article_Description");

            migrationBuilder.RenameColumn(
                name: "article_id",
                table: "Articles",
                newName: "Article_Id");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
