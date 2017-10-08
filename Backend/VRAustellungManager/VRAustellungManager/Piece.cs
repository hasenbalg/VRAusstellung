using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VRAustellungManager
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
