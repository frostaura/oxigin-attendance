using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oxigin.Attendance.Datastore.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNameFromSessionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserSessions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
