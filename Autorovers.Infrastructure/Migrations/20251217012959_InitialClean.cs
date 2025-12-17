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
                name: "Vehicle",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Kind = table.Column<int>(type: "integer", nullable: false),
                    Powertrain = table.Column<int>(type: "integer", nullable: false),
                    Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Variant = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Transmission = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Slug = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
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
                    Engine_EngineType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Engine_EngineDisplacement = table.Column<int>(type: "integer", nullable: true),
                    Engine_InductionType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Engine_Emission = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Engine_Power = table.Column<double>(type: "double precision", nullable: true),
                    Engine_PowerRpm = table.Column<int>(type: "integer", nullable: true),
                    Engine_Torque = table.Column<double>(type: "double precision", nullable: true),
                    Engine_TorqueRpm = table.Column<int>(type: "integer", nullable: true),
                    Engine_Mileage = table.Column<double>(type: "double precision", nullable: true),
                    Engine_Range = table.Column<double>(type: "double precision", nullable: true),
                    Engine_FuelType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Ev_BatteryCapacity = table.Column<double>(type: "double precision", nullable: true),
                    Ev_ChargingTimeFast = table.Column<double>(type: "double precision", nullable: true),
                    Ev_ChargingTimeNormal = table.Column<double>(type: "double precision", nullable: true),
                    Ev_MotorPower = table.Column<int>(type: "integer", nullable: true),
                    Ev_MotorTorque = table.Column<int>(type: "integer", nullable: true),
                    Ev_FastChargingPort = table.Column<bool>(type: "boolean", nullable: true),
                    Ev_Range = table.Column<double>(type: "double precision", nullable: true),
                    Dimensions_Length = table.Column<int>(type: "integer", nullable: true),
                    Dimensions_Width = table.Column<int>(type: "integer", nullable: true),
                    Dimensions_Height = table.Column<int>(type: "integer", nullable: true),
                    Dimensions_WheelBase = table.Column<int>(type: "integer", nullable: true),
                    Dimensions_GroundClearance = table.Column<int>(type: "integer", nullable: true),
                    Dimensions_Weight = table.Column<int>(type: "integer", nullable: true),
                    Dimensions_TurningRadius = table.Column<double>(type: "double precision", nullable: true),
                    Dynamics_FrontType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Dynamics_BackType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Dynamics_FrontBrake = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Dynamics_BackBrake = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    Dynamics_FrontSuspension = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Dynamics_RearSuspension = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Dynamics_TyreSizeFront = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Dynamics_TyreSizeBack = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Dynamics_TyreType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Dynamics_WheelMaterial = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Bike_NumberOfGears = table.Column<int>(type: "integer", nullable: true),
                    Bike_TankSize = table.Column<double>(type: "double precision", nullable: true),
                    Bike_Abs = table.Column<bool>(type: "boolean", nullable: true),
                    Bike_TractionControl = table.Column<bool>(type: "boolean", nullable: true),
                    Bike_DisplaySize = table.Column<double>(type: "double precision", nullable: true),
                    Bike_Bluetooth = table.Column<bool>(type: "boolean", nullable: true),
                    Bike_Navigation = table.Column<bool>(type: "boolean", nullable: true),
                    Bike_SmartConnectivity = table.Column<bool>(type: "boolean", nullable: true),
                    Car_DriveType = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Car_ZeroToHundred = table.Column<double>(type: "double precision", nullable: true),
                    Car_TopSpeed = table.Column<int>(type: "integer", nullable: true),
                    Car_PersonCapacity = table.Column<int>(type: "integer", nullable: true),
                    Car_Rows = table.Column<int>(type: "integer", nullable: true),
                    Car_Doors = table.Column<int>(type: "integer", nullable: true),
                    Car_BootSpace = table.Column<int>(type: "integer", nullable: true),
                    Car_PoweredSteering = table.Column<bool>(type: "boolean", nullable: true),
                    Car_SteeringType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Car_HasSpareWheel = table.Column<bool>(type: "boolean", nullable: true),
                    Car_Airbags = table.Column<int>(type: "integer", nullable: true),
                    Car_HillAssist = table.Column<bool>(type: "boolean", nullable: true),
                    Car_RearViewCamera = table.Column<bool>(type: "boolean", nullable: true),
                    Car_ParkingSensors = table.Column<bool>(type: "boolean", nullable: true),
                    Car_CruiseControl = table.Column<bool>(type: "boolean", nullable: true),
                    Car_DisplaySize = table.Column<double>(type: "double precision", nullable: true),
                    Car_Bluetooth = table.Column<bool>(type: "boolean", nullable: true),
                    Car_Navigation = table.Column<bool>(type: "boolean", nullable: true),
                    Car_SmartConnectivity = table.Column<bool>(type: "boolean", nullable: true),
                    ColorsAvailableJson = table.Column<string>(type: "text", nullable: true),
                    WarrantyYears = table.Column<int>(type: "integer", nullable: true),
                    ServiceIntervalKm = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleDetails", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_VehicleDetails_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "dbo",
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_UserId",
                schema: "dbo",
                table: "TodoItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Slug",
                schema: "dbo",
                table: "Vehicle",
                column: "Slug",
                unique: true);
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
                name: "Vehicle",
                schema: "dbo");
        }
    }
}
