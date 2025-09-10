using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorovers.Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Variant { get; set; }
    public int Price { get; set; }
    public string Category { get; set; }
    public string Transmission { get; set; }

    public VehicleDetails Details { get; set; }
}
