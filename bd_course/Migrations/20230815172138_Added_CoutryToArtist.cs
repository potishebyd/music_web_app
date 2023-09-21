using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bd_course.Migrations
{
    /// <inheritdoc />
    public partial class Added_CoutryToArtist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Artists",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Artists");
        }
    }
}
