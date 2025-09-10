using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorovers.Domain.Entities
{
    public class VehicleDetails
    {
        public int VehicleId { get; set; }

        //Engine Specs
        public string EngineType { get; set; }
        public string Specification { get; set; }
        public string InductionType {  get; set; } 
        public int Power { get; set; }
        public int Torque { get; set; }
        public string Emission { get; set; }
        public int Mileage { get; set; }
        public string AutoStartStop { get; set; }
        public int Range { get; set; }
        public string Transmission { get; set; }

        //Vehicle Size and weight
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Weight {  get; set; }
        public int GroundClearance { get; set; }
        public int WheelBase { get; set; }

        //Capacity
        public int PersonCapacity { get; set; }
        public int Rows {  get; set; }     
        public int Doors { get; set; }
        public int BootSpace { get; set; } //boot space
        public int TankSize { get; set; } //Tank Size

        //Vehicle Dynamics
        public string FrontTyre { get; set; }
        public string BackType { get; set; }
        public string FrontBrake { get; set; } //drum or disc
        public string BackBrake { get; set; }
        public string PoweredSteering { get; set; }
        public string TyreSizeFront { get; set; }
        public string TyreSizeBack { get; set; }
        public string TyreType { get; set; } //Radial
        public string WheelMaterial { get; set; }  //Alloy or steel rims
        public string Spare {  get; set; } // Spare type Alloy or Steel rims
    }
}
