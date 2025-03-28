using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Actor")]
    public class ActorDto
    {
        [XmlAttribute(nameof(FullName))]
        public string FullName { get; set; } = null!;

        [XmlAttribute(nameof(MainCharacter))]
        public string MainCharacter { get; set; } = null!;
    }
}