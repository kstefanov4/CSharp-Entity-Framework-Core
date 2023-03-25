namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Coaches");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCoachDto[]), xmlRoot);

            using StringReader reader = new StringReader(xmlString);

            ImportCoachDto[] categoryProductDtos = (ImportCoachDto[])xmlSerializer.Deserialize(reader);

            ICollection<Coach> coaches= new List<Coach>();

            StringBuilder sb = new StringBuilder();
            foreach (var coachDto in categoryProductDtos)
            {
                if (!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach();
                coach.Name = coachDto.Name;
                coach.Nationality = coachDto.Nationality;
                foreach (var fotballerDto in coachDto.Footballers)
                {
                    if (!IsValid(fotballerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Footballer footballer = new Footballer();
                    footballer.Name = fotballerDto.Name;
                    footballer.ContractStartDate = DateTime.ParseExact(fotballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    footballer.ContractEndDate = DateTime.ParseExact(fotballerDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    
                    if (footballer.ContractStartDate > footballer.ContractEndDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    footballer.PositionType = (PositionType)fotballerDto.PositionType;
                    footballer.BestSkillType = (BestSkillType)fotballerDto.BestSkillType;

                    coach.Footballers.Add(footballer);
                }
                sb.AppendLine($"Successfully imported coach - {coach.Name} with {coach.Footballers.Count} footballers.");
                coaches.Add(coach);
            }
            context.Coaches.AddRange(coaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            ImportTeamDto[] importTeamDtos = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            ICollection<Team> teams = new List<Team>();

            foreach (var teamDto in importTeamDtos)
            {
                if (!IsValid(teamDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (teamDto.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team();
                team.Name = teamDto.Name;
                team.Nationality = teamDto.Nationality;
                team.Trophies = teamDto.Trophies;

                foreach (var footballerDto in teamDto.Footballers.Distinct())
                {
                    var foot = context.Footballers.ToList();
                    if(!context.Footballers.Any(f => f.Id == footballerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer footballer = new TeamFootballer();
                    footballer.FootballerId = footballerDto;
                    /*context.TeamsFootballers.Add(footballer);
                    context.SaveChanges();*/

                    team.TeamsFootballers.Add(footballer);
                }
                teams.Add(team);
                sb.AppendLine($"Successfully imported team - {team.Name} with {team.TeamsFootballers.Count} footballers.");
            }

            context.Teams.AddRange(teams);
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
