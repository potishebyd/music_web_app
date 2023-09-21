using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bd_course.Migrations
{
    /// <inheritdoc />
    public partial class Added_RecordingStudioIdToSong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecordingStudioId",
                table: "Songs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordingStudioId",
                table: "Songs");
        }
    }
}
