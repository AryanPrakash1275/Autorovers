using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Autorovers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<int>(type: "integer", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Preferences = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicel",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VehicleType = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Variant = table.Column<string>(type: "text", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Transmission = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Labels = table.Column<List<string>>(type: "text[]", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItems_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleDetails",
                schema: "dbo",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "integer", nullable: false),
                    EngineType = table.Column<string>(type: "text", nullable: true),
                    EngineDisplacement = table.Column<int>(type: "integer", nullable: true),
                    Specification = table.Column<string>(type: "text", nullable: true),
                    InductionType = table.Column<string>(type: "text", nullable: true),
                    Power = table.Column<double>(type: "double precision", nullable: true),
                    PowerRpm = table.Column<int>(type: "integer", nullable: true),
                    Torque = table.Column<double>(type: "double precision", nullable: true),
                    TorqueRpm = table.Column<int>(type: "integer", nullable: true),
                    Emission = table.Column<string>(type: "text", nullable: true),
                    Mileage = table.Column<double>(type: "double precision", nullable: true),
                    AutoStartStop = table.Column<string>(type: "text", nullable: true),
                    Range = table.Column<int>(type: "integer", nullable: true),
                    FuelType = table.Column<string>(type: "text", nullable: true),
                    Transmission = table.Column<string>(type: "text", nullable: true),
                    BatteryCapacity = table.Column<double>(type: "double precision", nullable: true),
                    ChargingTimeFast = table.Column<double>(type: "double precision", nullable: true),
                    ChargingTimeNormal = table.Column<double>(type: "double precision", nullable: true),
                    MotorPower = table.Column<int>(type: "integer", nullable: true),
                    MotorTorque = table.Column<int>(type: "integer", nullable: true),
                    NumberOfGears = table.Column<int>(type: "integer", nullable: true),
                    DriveType = table.Column<string>(type: "text", nullable: true),
                    ZeroToHundred = table.Column<double>(type: "double precision", nullable: true),
                    TopSpeed = table.Column<int>(type: "integer", nullable: true),
                    Length = table.Column<int>(type: "integer", nullable: true),
                    Width = table.Column<int>(type: "integer", nullable: true),
                    Height = table.Column<int>(type: "integer", nullable: true),
                    WheelBase = table.Column<int>(type: "integer", nullable: true),
                    GroundClearance = table.Column<int>(type: "integer", nullable: true),
                    Weight = table.Column<int>(type: "integer", nullable: true),
                    TurningRadius = table.Column<double>(type: "double precision", nullable: true),
                    PersonCapacity = table.Column<int>(type: "integer", nullable: true),
                    Rows = table.Column<int>(type: "integer", nullable: true),
                    Doors = table.Column<int>(type: "integer", nullable: true),
                    BootSpace = table.Column<int>(type: "integer", nullable: true),
                    TankSize = table.Column<double>(type: "double precision", nullable: true),
                    FrontType = table.Column<string>(type: "text", nullable: true),
                    BackType = table.Column<string>(type: "text", nullable: true),
                    FrontBrake = table.Column<string>(type: "text", nullable: true),
                    BackBrake = table.Column<string>(type: "text", nullable: true),
                    FrontSuspension = table.Column<string>(type: "text", nullable: true),
                    RearSuspension = table.Column<string>(type: "text", nullable: true),
                    TyreSizeFront = table.Column<string>(type: "text", nullable: true),
                    TyreSizeBack = table.Column<string>(type: "text", nullable: true),
                    TyreType = table.Column<string>(type: "text", nullable: true),
                    WheelMaterial = table.Column<string>(type: "text", nullable: true),
                    Spare = table.Column<string>(type: "text", nullable: true),
                    PoweredSteering = table.Column<string>(type: "text", nullable: true),
                    SteeringType = table.Column<string>(type: "text", nullable: true),
                    Abs = table.Column<bool>(type: "boolean", nullable: true),
                    TractionControl = table.Column<bool>(type: "boolean", nullable: true),
                    Airbags = table.Column<int>(type: "integer", nullable: true),
                    HillAssist = table.Column<bool>(type: "boolean", nullable: true),
                    RearViewCamera = table.Column<bool>(type: "boolean", nullable: true),
                    ParkingSensors = table.Column<bool>(type: "boolean", nullable: true),
                    CruiseControl = table.Column<bool>(type: "boolean", nullable: true),
                    DisplaySize = table.Column<double>(type: "double precision", nullable: true),
                    Bluetooth = table.Column<bool>(type: "boolean", nullable: true),
                    Navigation = table.Column<bool>(type: "boolean", nullable: true),
                    SmartConnectivity = table.Column<bool>(type: "boolean", nullable: true),
                    FastChargingPort = table.Column<bool>(type: "boolean", nullable: true),
                    ColorsAvailableJson = table.Column<string>(type: "text", nullable: true),
                    WarrantyYears = table.Column<int>(type: "integer", nullable: true),
                    ServiceIntervalKm = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleDetails", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_VehicleDetails_Vehicel_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "dbo",
                        principalTable: "Vehicel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_UserId",
                schema: "dbo",
                table: "TodoItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TodoItems",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VehicleDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Vehicel",
                schema: "dbo");
        }
    }
}
