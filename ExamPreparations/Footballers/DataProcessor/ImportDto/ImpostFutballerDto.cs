using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImpostFutballerDto
    {
        [XmlElement("Name")]
        [MaxLength(40)]
        [MinLength(2)]
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [XmlElement("ContractStartDate")]
        public string ContractStartDate { get; set; } = null!;
        [Required]
        [XmlElement("ContractEndDate")]
        public string ContractEndDate { get; set; } = null!;
        [Required]
        [XmlElement("BestSkillType")]
        public int BestSkillType { get; set; }
        [Required]
        [XmlElement("PositionType")]
        public int PositionType { get; set; }

    }
/*
    <Footballer>
                <Name>Benjamin Bourigeaud</Name>
                <ContractStartDate>22/03/2020</ContractStartDate>
                <ContractEndDate>24/02/2025</ContractEndDate>
                <BestSkillType>2</BestSkillType>
                <PositionType>2</PositionType>
              </Footballer>*/
}
