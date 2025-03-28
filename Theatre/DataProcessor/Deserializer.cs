
namespace Theatre.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;
    using Theatre.Utilities;


    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPlays[]? importPlaysDto = XmlHelper.Deserialize<ImportPlays[]>(xmlString, "Plays");

            if (importPlaysDto != null && importPlaysDto.Length > 0)
            {
                ICollection<Play> playsToAdd = new List<Play>();

                foreach (ImportPlays dto in importPlaysDto)
                {
                    if (!IsValid(dto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isValidGenre = Enum.TryParse<Genre>(dto.Genre, out Genre parsedGenre);

                    if (!isValidGenre)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    bool isValidTimeSpan = TimeSpan.TryParseExact(dto.Duration, "c", CultureInfo.InvariantCulture,
                        out TimeSpan parsedTimeSpan);

                    if (!isValidTimeSpan)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TimeSpan minTime = TimeSpan.ParseExact("01:00:00", "c", CultureInfo.InvariantCulture);

                    if (parsedTimeSpan < minTime)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Play play = new Play()
                    {
                        Title = dto.Title,
                        Duration = parsedTimeSpan,
                        Rating = dto.Raiting,
                        Genre = parsedGenre,
                        Description = dto.Description,
                        Screenwriter = dto.Screenwriter,
                    };

                    playsToAdd.Add(play);
                    sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating));
                }
                context.AddRange(playsToAdd);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            ImportCastDto[]? importCastDtos = XmlHelper.Deserialize<ImportCastDto[]>(xmlString, "Casts");

            if (importCastDtos != null && importCastDtos.Length > 0)
            {
                ICollection<Cast> castToAdd = new List<Cast>();

                foreach (ImportCastDto castDto in importCastDtos)
                {
                    if (!IsValid(castDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Cast cast = new Cast()
                    {
                        FullName = castDto.FullName,
                        IsMainCharacter = castDto.IsMainCharacter,
                        PhoneNumber = castDto.PhoneNumber,
                        PlayId = castDto.PlayId,
                    };

                    castToAdd.Add(cast);
                    sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName,
                        (cast.IsMainCharacter ? "main" : "lesser")));
                }

                context.AddRange(castToAdd);
                context.SaveChanges();
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var theatreTicketsJson = JsonConvert.DeserializeObject<ImportTheatresTicketsDto[]>(jsonString);
            var theatres = new List<Theatre>();
            var sb = new StringBuilder();

            foreach (var dto in theatreTicketsJson)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var theatre = new Theatre
                {
                    Name = dto.Name,
                    NumberOfHalls = dto.NumberOfHalls,
                    Director = dto.Director,
                };
                var tickets = new List<Ticket>();

                foreach (var tc in dto.Tickets)
                {
                    if (!IsValid(tc))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var currenTicket = new Ticket
                    {
                        Price = tc.Price,
                        RowNumber = tc.RowNumber,
                        Theatre = theatre,
                        PlayId = tc.PlayId,
                    };

                    tickets.Add(currenTicket);
                }

                theatre.Tickets = tickets;
                theatres.Add(theatre);
                var totalTickets = theatre.Tickets.Count().ToString();
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, totalTickets));
            }

            context.Theatres.AddRange(theatres);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
