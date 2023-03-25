using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class ExportCoachDto
    {
        [XmlElement("CoachName")]
        public string Name { get; set; }

        [XmlAttribute("FootballersCount")]
        public int Count { get; set; }
        [XmlArray("Footballers")]
        public ExportFootballerDto[] Footballers { get; set; }
    }
}
