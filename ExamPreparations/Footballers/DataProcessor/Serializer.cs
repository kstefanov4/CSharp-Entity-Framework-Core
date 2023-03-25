namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            ExportCoachDto[] coachesWithTheirFootballers = context.Coaches
                .AsNoTracking()
                .Where(c => c.Footballers.Count > 0)
                .OrderByDescending(c => c.Footballers.Count)
                .ThenBy(c=> c.Name)
                .Select(c => new ExportCoachDto
                {
                    Name = c.Name,
                    Count = c.Footballers.Count,
                    Footballers = c.Footballers
                        .OrderBy(c => c.Name)
                        .Select(f => new ExportFootballerDto
                        {
                            Name = f.Name,
                            Position = f.PositionType.ToString()
                        }).ToArray()
                }).ToArray();

            StringBuilder sb = new StringBuilder();
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Coaches");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCoachDto[]), xmlRoot);

            StringWriter stringWriter = new StringWriter(sb);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            xmlSerializer.Serialize(stringWriter, coachesWithTheirFootballers, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teamsWithMostFootballers = context.Teams
                .AsNoTracking()
                .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .OrderByDescending(f => f.TeamsFootballers.Count)
                .ThenBy(t => t.Name)
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                        .Where(f => f.Footballer.ContractStartDate >= date)
                        .OrderByDescending(f => f.Footballer.ContractEndDate)
                        .ThenBy(f => f.Footballer.Name)
                        .Select(f => new
                        {
                            FootballerName = f.Footballer.Name,
                            ContractStartDate = f.Footballer.ContractStartDate.ToString("dd/MM/yyyy"),
                            ContractEndDate = f.Footballer.ContractEndDate.ToString("dd/MM/yyyy"),
                            BestSkillType = f.Footballer.BestSkillType.ToString(),
                            PositionType = f.Footballer.PositionType.ToString()
                        }).ToList()
                })
                .Take(5)
                .ToList();

            return JsonConvert.SerializeObject(teamsWithMostFootballers,Formatting.Indented);
        }
    }
}
