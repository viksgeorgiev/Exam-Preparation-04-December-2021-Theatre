using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Enums;
    [XmlType("Play")]
    public class ImportPlays
    {
        [XmlElement(nameof(Title))]
        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [XmlElement(nameof(Duration))]
        [Required]
        public string Duration { get; set; } = null!;

        [XmlElement(nameof(Raiting))]
        [Required]
        [Range(0.00, 10.00)]
        public float Raiting { get; set; } 

        [XmlElement(nameof(Genre))] 
        [Required] 
        public string Genre { get; set; } = null!;

        [XmlElement(nameof(Description))]
        [Required]
        [StringLength(700)]
        public string Description { get; set; } = null!;

        [XmlElement(nameof(Screenwriter))]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Screenwriter { get; set; } = null!;
    }
}
