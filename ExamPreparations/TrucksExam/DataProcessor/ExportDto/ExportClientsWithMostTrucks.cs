using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ExportDto
{
    [JsonObject]
    public class ExportClientsWithMostTrucks
    {
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [JsonProperty("Trucks")]
        public ExportTruckDto[] Trucks { get; set; }

    }
}
