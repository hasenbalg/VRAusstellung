using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VRAustellungManager
{

    [XmlRoot("exhibition")]
    public class Exhibition
    {
        [XmlElement("titel")]
        public string title { get; set; }

        [XmlElement("description")]
        public string description { get; set; }

        [XmlElement("width")]
        public int width { get; set; }

        [XmlElement("height")]
        public int height { get; set; }

        [XmlElement("iconpath")]
        public string iconpath { get; set; }

        [XmlElement("piece")]
        public List<Piece> pieces { get; set; }
    }
}
