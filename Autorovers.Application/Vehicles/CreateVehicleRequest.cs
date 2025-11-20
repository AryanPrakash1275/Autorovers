using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Autorovers.Application.Vehicles
{
    public class CreateVehicleRequest
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Variant { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public string Transmission { get; set; }

        // Details block
        public string EngineType { get; set; }
        public string Specification { get; set; }
        public string InductionType { get; set; }
        public int Power { get; set; }
        public int Torque { get; set; }
        public string Emission { get; set; }
        public int Mileage { get; set; }
        //null problem
        [JsonPropertyName("autoStartStop")]
        public string AutoStartStop { get; set; } = "";
        public int Range { get; set; }

        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int GroundClearance { get; set; }
        public int WheelBase { get; set; }

        public int PersonCapacity { get; set; }
        public int Rows { get; set; }
        public int Doors { get; set; }
        public int BootSpace { get; set; }
        public int TankSize { get; set; }

        public string FrontTyre { get; set; }
        public string BackType { get; set; }
        public string FrontBrake { get; set; }
        public string BackBrake { get; set; }
        public string PoweredSteering { get; set; }
        public string TyreSizeFront { get; set; }
        public string TyreSizeBack { get; set; }
        public string TyreType { get; set; }
        public string WheelMaterial { get; set; }
        public string Spare { get; set; }
    }
}
