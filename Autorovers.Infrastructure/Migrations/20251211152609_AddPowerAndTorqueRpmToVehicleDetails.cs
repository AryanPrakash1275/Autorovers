using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autorovers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPowerAndTorqueRpmToVehicleDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Width",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "WheelMaterial",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "WheelBase",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "TyreType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "TyreSizeFront",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "TyreSizeBack",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Transmission",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<double>(
                name: "Torque",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "TankSize",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "Specification",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Spare",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Rows",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Range",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "PoweredSteering",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<double>(
                name: "Power",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "PersonCapacity",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Mileage",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "Length",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "InductionType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "GroundClearance",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "FrontType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FrontBrake",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EngineType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Emission",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Doors",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "BootSpace",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "BackType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "BackBrake",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AutoStartStop",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "Abs",
                schema: "dbo",
                table: "VehicleDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Airbags",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BatteryCapacity",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Bluetooth",
                schema: "dbo",
                table: "VehicleDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ChargingTimeFast",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ChargingTimeNormal",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorsAvailableJson",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CruiseControl",
                schema: "dbo",
                table: "VehicleDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DisplaySize",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriveType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EngineDisplacement",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FastChargingPort",
                schema: "dbo",
                table: "VehicleDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontSuspension",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuelType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HillAssist",
                schema: "dbo",
                table: "VehicleDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MotorPower",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MotorTorque",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Navigation",
                schema: "dbo",
                table: "VehicleDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfGears",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ParkingSensors",
                schema: "dbo",
                table: "VehicleDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PowerRpm",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RearSuspension",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RearViewCamera",
                schema: "dbo",
                table: "VehicleDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceIntervalKm",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SmartConnectivity",
                schema: "dbo",
                table: "VehicleDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SteeringType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TopSpeed",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TorqueRpm",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TractionControl",
                schema: "dbo",
                table: "VehicleDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TurningRadius",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarrantyYears",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZeroToHundred",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Variant",
                schema: "dbo",
                table: "Vehicel",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                schema: "dbo",
                table: "Vehicel",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abs",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "Airbags",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "BatteryCapacity",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "Bluetooth",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "ChargingTimeFast",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "ChargingTimeNormal",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "ColorsAvailableJson",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "CruiseControl",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "DisplaySize",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "DriveType",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "EngineDisplacement",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "FastChargingPort",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "FrontSuspension",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "FuelType",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "HillAssist",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "MotorPower",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "MotorTorque",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "Navigation",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "NumberOfGears",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "ParkingSensors",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "PowerRpm",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "RearSuspension",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "RearViewCamera",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "ServiceIntervalKm",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "SmartConnectivity",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "SteeringType",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "TopSpeed",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "TorqueRpm",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "TractionControl",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "TurningRadius",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "WarrantyYears",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "ZeroToHundred",
                schema: "dbo",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                schema: "dbo",
                table: "Vehicel");

            migrationBuilder.AlterColumn<int>(
                name: "Width",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WheelMaterial",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WheelBase",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TyreType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TyreSizeFront",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TyreSizeBack",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Transmission",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Torque",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TankSize",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specification",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Spare",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Rows",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Range",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PoweredSteering",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Power",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonCapacity",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Mileage",
                schema: "dbo",
                table: "VehicleDetails",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Length",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InductionType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GroundClearance",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FrontType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FrontBrake",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EngineType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Emission",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Doors",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BootSpace",
                schema: "dbo",
                table: "VehicleDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BackType",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BackBrake",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AutoStartStop",
                schema: "dbo",
                table: "VehicleDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Variant",
                schema: "dbo",
                table: "Vehicel",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
