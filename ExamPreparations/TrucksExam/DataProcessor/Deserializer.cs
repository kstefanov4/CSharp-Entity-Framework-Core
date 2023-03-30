namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Text.Json;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Despatchers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportDespatchersDto[]), xmlRoot);

            using StringReader reader = new StringReader(xmlString);

            ImportDespatchersDto[] importDespatchersDtos = (ImportDespatchersDto[])xmlSerializer.Deserialize(reader);

            List<Despatcher> validDespatchers = new List<Despatcher>();
            
            StringBuilder sb = new StringBuilder();

            foreach (var despatchersDto in importDespatchersDtos)
            {
                if (!IsValid(despatchersDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(despatchersDto.Position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher despatcher = new Despatcher();
                despatcher.Name = despatchersDto.Name;
                despatcher.Position = despatchersDto.Position;

                foreach (var truckDto in despatchersDto.Trucks)
                {
                    if (!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = new Truck();
                    truck.CargoCapacity = truckDto.CargoCapacity;
                    truck.TankCapacity = truckDto.TankCapacity;
                    truck.VinNumber = truckDto.VinNumber;
                    truck.RegistrationNumber = truckDto.RegistrationNumber;
                    truck.CategoryType = (CategoryType)truckDto.CategoryType;
                    truck.MakeType = (MakeType)truckDto.MakeType;

                    context.Trucks.Add(truck);

                    despatcher.Trucks.Add(truck);
                }

                validDespatchers.Add(despatcher);
                sb.AppendLine($"Successfully imported despatcher - {despatcher.Name} with {despatcher.Trucks.Count} trucks.");
            }

            context.Despatchers.AddRange(validDespatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            ImportClientDto[] dtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);

            List<Client> validClients = new List<Client>();
            List<int> validTrucks = context.Trucks.Select(x => x.Id).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client();
                client.Name = dto.Name;
                client.Type = dto.Type;
                client.Nationality = dto.Nationality;

                foreach (var truckNumber in dto.Trucks.Distinct())
                {
                    if (!IsValid(truckNumber))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!validTrucks.Any(c => c == truckNumber))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck clientTruck = new ClientTruck();
                    clientTruck.TruckId = truckNumber;
                    client.ClientsTrucks.Add(clientTruck);
                }

                validClients.Add(client);
                sb.AppendLine($"Successfully imported client - {client.Name} with {client.ClientsTrucks.Count} trucks.");
            }
            context.Clients.AddRange(validClients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}