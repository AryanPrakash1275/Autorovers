using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Autorovers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_VehicleVariants_And_Remove_Old_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "Variant",
                table: "Vehicle");

            migrationBuilder.AddColumn<int>(
                name: "StartingPrice",
                table: "Vehicle",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VariantAddon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Price = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariantAddon", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleVariant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VehicleId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Code = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    ExShowroomPrice = table.Column<int>(type: "integer", nullable: true),
                    PriceDelta = table.Column<int>(type: "integer", nullable: true),
                    Transmission = table.Column<string>(type: "text", nullable: true),
                    Powertrain = table.Column<int>(type: "integer", nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Slug = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleVariant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleVariant_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleVariantAddon",
                columns: table => new
                {
                    VehicleVariantId = table.Column<int>(type: "integer", nullable: false),
                    VariantAddonId = table.Column<int>(type: "integer", nullable: false),
                    IsIncluded = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    OverridePrice = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleVariantAddon", x => new { x.VehicleVariantId, x.VariantAddonId });
                    table.ForeignKey(
                        name: "FK_VehicleVariantAddon_VariantAddon_VariantAddonId",
                        column: x => x.VariantAddonId,
                        principalTable: "VariantAddon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleVariantAddon_VehicleVariant_VehicleVariantId",
                        column: x => x.VehicleVariantId,
                        principalTable: "VehicleVariant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVariant_VehicleId_Slug",
                table: "VehicleVariant",
                columns: new[] { "VehicleId", "Slug" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVariantAddon_VariantAddonId",
                table: "VehicleVariantAddon",
                column: "VariantAddonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleVariantAddon");

            migrationBuilder.DropTable(
                name: "VariantAddon");

            migrationBuilder.DropTable(
                name: "VehicleVariant");

            migrationBuilder.DropColumn(
                name: "StartingPrice",
                table: "Vehicle");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Vehicle",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Variant",
                table: "Vehicle",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);
        }
    }
}
