using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.DataProcessor.ExportDto
{
    [JsonObject]
    public class ExportTruckDto
    {
        [JsonProperty("TruckRegistrationNumber")]
        public string TruckRegistrationNumber { get; set; } = null!;

        [JsonProperty("VinNumber")]
        public string VinNumber { get; set; } = null!;
        
        [JsonProperty("TankCapacity")]
        public int TankCapacity { get; set; }

        [JsonProperty("CargoCapacity")]
        public int CargoCapacity { get; set; }

        [JsonProperty("CategoryType")]
        public string CategoryType { get; set; } = null!;

        [JsonProperty("MakeType")]
        public string MakeType { get; set; } = null!;
    }
}
