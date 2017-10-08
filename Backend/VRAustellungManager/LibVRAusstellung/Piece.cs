using System.Xml.Serialization;

namespace LibVRAusstellung
{
    public class Piece
    {
        [XmlElement("id")]
        public int id { get; set; }
        [XmlElement("title")]
        public string title { get; set; }

        [XmlElement("description")]
        public string description { get; set; }

        [XmlElement("filePath")]
        public string filePath { get; set; }

        [XmlElement("fileformat")]
        public string fileformat { get; set; }
    }
}
