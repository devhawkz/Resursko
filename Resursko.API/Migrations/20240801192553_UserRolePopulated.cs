using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resursko.API.Migrations
{
    /// <inheritdoc />
    public partial class UserRolePopulated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "eff0bd82-d02b-4191-83b9-63c8302340fa", "7c6e4bc0-2a73-4764-8148-6f846c7db73e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "eff0bd82-d02b-4191-83b9-63c8302340fa", "7c6e4bc0-2a73-4764-8148-6f846c7db73e" });
        }
    }
}
