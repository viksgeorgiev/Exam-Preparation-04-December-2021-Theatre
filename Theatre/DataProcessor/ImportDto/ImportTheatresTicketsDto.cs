using Newtonsoft.Json;

namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportTheatresTicketsDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(typeof(sbyte), "1", "10")]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Director { get; set; } = null!;

        public ImportTicket[] Tickets { get; set; } = Array.Empty<ImportTicket>();
    }
}
