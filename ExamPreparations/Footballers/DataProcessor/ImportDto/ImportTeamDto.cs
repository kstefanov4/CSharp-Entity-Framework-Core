using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportTeamDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z0-9\s\.\-]+$")]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        [JsonProperty("Nationality")]
        public string Nationality { get; set; } = null!;
        [Required]
        [JsonProperty("Trophies")]
        public int Trophies { get; set; }

        public int[] Footballers { get; set; }
    }

    /*"Name": "Brentford F.C.",
    "Nationality": "The United Kingdom",
    "Trophies": "5",
    "Footballers": [
      28,
      28,
      39,
      57
    ]*/
}
