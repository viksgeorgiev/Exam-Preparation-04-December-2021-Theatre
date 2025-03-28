using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    [XmlType("Cast")]
    public class ImportCastDto
    {
        [XmlElement(nameof(FullName))]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string FullName { get; set; } = null!;

        [XmlElement(nameof(IsMainCharacter))]
        [Required]
        public bool IsMainCharacter { get; set; }

        [XmlElement(nameof(PhoneNumber))]
        [Required]
        [RegularExpression(@"\+44\-\d{2}\-\d{3}\-\d{4}")]
        public string PhoneNumber { get; set; } = null!;

        [XmlElement(nameof(PlayId))]
        public int PlayId { get; set; }
    }
}
