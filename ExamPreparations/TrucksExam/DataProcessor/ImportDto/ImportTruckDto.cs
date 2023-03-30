using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class ImportTruckDto
    {
        [XmlElement("RegistrationNumber")]
        [Required]
        [RegularExpression(@"^[A-Z]{2}[0-9]{4}[A-Z]{2}$")]
        public string RegistrationNumber { get; set; } = null!;

        [Required]
        [XmlElement("VinNumber")]
        [RegularExpression(@"^[A-Za-z0-9]{17}$")]
        public string VinNumber { get; set; } = null!;

        [Required]
        [XmlElement("TankCapacity")]
        [Range(950,1420)]
        public int TankCapacity { get; set; }

        [Required]
        [XmlElement("CargoCapacity")]
        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }

        [Required]
        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [Required]
        [XmlElement("MakeType")]
        public int MakeType { get; set; }


        /*
         <Truck>
				<RegistrationNumber>CB0796TP</RegistrationNumber>
				<VinNumber>YS2R4X211D5318181</VinNumber>
				<TankCapacity>1000</TankCapacity>
				<CargoCapacity>23999</CargoCapacity>
				<CategoryType>0</CategoryType>
				<MakeType>3</MakeType>
			</Truck>
         */
    }
}
