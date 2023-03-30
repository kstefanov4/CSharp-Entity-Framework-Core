using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatcher")]
    public class ExportDespatcherDto
    {
        [XmlAttribute("TrucksCount")]
        public int TruckCount { get; set; }
        [XmlElement("DespatcherName")]
        public string DespatcherName { get; set; } = null!;
        [XmlArray("Trucks")]
        public ExportTruckForDesDto[] Trucks { get; set; }
    }
}
