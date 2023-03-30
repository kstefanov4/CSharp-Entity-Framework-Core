using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
	[XmlType("Despatcher")]
    public class ImportDespatchersDto
    {
		[XmlElement("Name")]
		[Required]
		[MinLength(2)]
		[MaxLength(40)]
		public string Name { get; set; } = null!;

		[Required]
        [XmlElement("Position")]
        public string Position { get; set; } = null!;

        [XmlArray("Trucks")]
        public List<ImportTruckDto> Trucks { get; set; } = new List<ImportTruckDto>();


        /*
         <Despatcher>
		<Name>Genadi Petrov</Name>
		<Position>Specialist</Position>
		<Trucks>
			<Truck>
				<RegistrationNumber>CB0796TP</RegistrationNumber>
				<VinNumber>YS2R4X211D5318181</VinNumber>
				<TankCapacity>1000</TankCapacity>
				<CargoCapacity>23999</CargoCapacity>
				<CategoryType>0</CategoryType>
				<MakeType>3</MakeType>
			</Truck>
			<Truck>
				<RegistrationNumber>CB0818TP</RegistrationNumber>
				<VinNumber>YS2R4X211D5318128</VinNumber>
				<TankCapacity>1400</TankCapacity>
				<CargoCapacity>29004</CargoCapacity>
				<CategoryType>3</CategoryType>
				<MakeType>0</MakeType>
			</Truck>
		</Trucks>
	</Despatcher>
         */
    }
}
