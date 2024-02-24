using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CozyCub.Migrations
{
    /// <inheritdoc />
    public partial class cozycub_migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "user",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "admin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "admin",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "user");
        }
    }
}
