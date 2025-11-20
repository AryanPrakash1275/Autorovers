using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autorovers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "dbo",
                table: "Vehicel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "dbo",
                table: "Vehicel");
        }
    }
}
