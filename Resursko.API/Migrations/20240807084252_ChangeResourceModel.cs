using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resursko.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeResourceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Resources");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Resources",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
