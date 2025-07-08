using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregation.Migrations
{
    /// <inheritdoc />
    public partial class Moderated_Keywords_Entity_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pending_Notifications_Categories_Category_Id",
                table: "Pending_Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Pending_Notifications_Category_Id",
                table: "Pending_Notifications");

            migrationBuilder.DropColumn(
                name: "Category_Id",
                table: "Pending_Notifications");

            migrationBuilder.CreateTable(
                name: "Moderated_Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Keyword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moderated_Keywords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moderated_Keywords");

            migrationBuilder.AddColumn<int>(
                name: "Category_Id",
                table: "Pending_Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pending_Notifications_Category_Id",
                table: "Pending_Notifications",
                column: "Category_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pending_Notifications_Categories_Category_Id",
                table: "Pending_Notifications",
                column: "Category_Id",
                principalTable: "Categories",
                principalColumn: "Category_Id");
        }
    }
}
