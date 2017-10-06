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
        public string title;

        [XmlElement("iconpath")]
        public string iconpath;

        [XmlElement("piece")]
        public List<Piece> pieces { get; set; }
    }
}
