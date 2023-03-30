namespace Trucks.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            ExportDespatcherDto[] despatchersWithTheirTrucks = context.Despatchers
                .Include(x => x.Trucks)
                .AsNoTracking()
                .ToArray()
                .Where(d => d.Trucks.Count > 0)
                .Select(d => new ExportDespatcherDto
                {
                    DespatcherName = d.Name,
                    TruckCount = d.Trucks.Count,
                    Trucks = d.Trucks
                        .Select(t => new ExportTruckForDesDto
                        {
                            RegistrationNumber = t.RegistrationNumber,
                            Make = t.MakeType.ToString()
                        })
                        .OrderBy(t => t.RegistrationNumber)
                        .ToArray()
                })
                .OrderByDescending(d => d.Trucks.Length)
                .ThenBy(d => d.DespatcherName)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Despatchers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportDespatcherDto[]), xmlRoot);

            StringWriter stringWriter = new StringWriter(sb);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            xmlSerializer.Serialize(stringWriter, despatchersWithTheirTrucks, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clientWithTrucks = context.Clients
                .Include(c => c.ClientsTrucks)
                .ThenInclude(ct => ct.Truck)
                .AsNoTracking()
                .ToArray()
                .Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))
                .Select(c => new ExportClientsWithMostTrucks
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks
                            .Where(ct => ct.Truck.TankCapacity >= capacity)
                            .Select(ct => new ExportTruckDto
                            {
                                TruckRegistrationNumber = ct.Truck.RegistrationNumber.ToString(),
                                VinNumber = ct.Truck.VinNumber.ToString(),
                                TankCapacity = ct.Truck.TankCapacity,
                                CargoCapacity = ct.Truck.CargoCapacity,
                                CategoryType = ct.Truck.CategoryType.ToString(),
                                MakeType = ct.Truck.MakeType.ToString()
                            })
                            .OrderBy(t => t.MakeType)
                            .ThenByDescending(t => t.CargoCapacity)
                            .ToArray()
                })
                .OrderByDescending(c => c.Trucks.Length)
                .ThenBy(c => c.Name)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(clientWithTrucks, Formatting.Indented);
        }
    }
}
