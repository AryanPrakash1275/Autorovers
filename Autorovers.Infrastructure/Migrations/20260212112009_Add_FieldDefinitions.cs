using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autorovers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_FieldDefinitions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Label = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    InputType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Unit = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    EnumValues = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AppliesTo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Powertrain = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Scope = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IsKeyDiff = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VariantFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VariantId = table.Column<int>(type: "integer", nullable: false),
                    FieldKey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariantFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariantFeatures_VehicleVariant_VariantId",
                        column: x => x.VariantId,
                        principalTable: "VehicleVariant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldDefinitions_Key",
                table: "FieldDefinitions",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FieldDefinitions_Scope_InputType_AppliesTo_Powertrain_IsAct~",
                table: "FieldDefinitions",
                columns: new[] { "Scope", "InputType", "AppliesTo", "Powertrain", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_VariantFeatures_VariantId_FieldKey",
                table: "VariantFeatures",
                columns: new[] { "VariantId", "FieldKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldDefinitions");

            migrationBuilder.DropTable(
                name: "VariantFeatures");
        }
    }
}
