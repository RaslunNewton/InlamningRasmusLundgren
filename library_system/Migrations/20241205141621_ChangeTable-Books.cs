using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace library_system.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "yearPublished",
                table: "Books",
                newName: "datePublished");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "datePublished",
                table: "Books",
                newName: "yearPublished");
        }
    }
}
