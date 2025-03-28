using System.Globalization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Theatre.DataProcessor.ExportDto;
using Theatre.Utilities;

namespace Theatre.DataProcessor
{

    using System;
    using Theatre.Data;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theaters = context
                .Theatres
                .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count > 20)
                .Select(t => new
                {
                    t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets.Where(t => t.RowNumber > 0 && t.RowNumber <= 5).Sum(t => t.Price),
                    Tickets = t.Tickets
                        .Where(t => t.RowNumber > 0 && t.RowNumber <= 5)
                        .Select(t => new
                        {
                            Price = t.Price,
                            RowNumber = t.RowNumber,
                        })
                        .OrderByDescending(ti => ti.Price)
                        .ToArray()
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            string theatersExport = JsonConvert.SerializeObject(theaters, Formatting.Indented);

            return theatersExport;
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            var plays = context
                .Plays
                .Where(p => p.Rating <= raiting)
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray()
                .Select(p => new PlayDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c", CultureInfo.InvariantCulture),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString("f1"),
                    Genre = p.Genre.ToString(),
                    Actors = p.Cast
                        .Where(a => a.IsMainCharacter == true)
                        .Select(a => new ActorDto()
                        {
                            FullName = a.FullName,
                            MainCharacter = $"Plays main character in '{a.Play.Title}'."
                        })
                        .OrderByDescending(a => a.FullName)
                        .ToArray()
                })
                .ToArray();

            string exportPlays = XmlHelper.Serialize(plays, "Plays");
            return exportPlays;
        }
    }
}
