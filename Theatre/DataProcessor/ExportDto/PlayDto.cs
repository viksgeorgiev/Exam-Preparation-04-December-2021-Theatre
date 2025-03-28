using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Play")]
    public class PlayDto
    {
        [XmlAttribute(nameof(Title))]
        public string Title { get; set; } = null!;

        [XmlAttribute(nameof(Duration))]
        public string Duration { get; set; } = null!;

        [XmlAttribute(nameof(Rating))]
        public string Rating { get; set; } = null!;

        [XmlAttribute(nameof(Genre))]
        public string Genre { get; set; } = null!;

        [XmlArray(nameof(Actors))]
        public ActorDto[] Actors { get; set; } = null!;
    }
}
