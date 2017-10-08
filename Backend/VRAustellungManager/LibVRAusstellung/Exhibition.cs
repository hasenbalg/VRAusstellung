using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace LibVRAusstellung
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










        static string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//VRAusstellungX";
        static string xmlPath = "//Object.xml";

        public static Exhibition ReadXMLFile()
        {
            XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Exhibition));
            try
            {
                using (FileStream fileStream = new FileStream(dir + xmlPath, FileMode.Open))
                {
                    return (Exhibition)serializer.Deserialize(fileStream);
                }
            }
            catch (Exception)
            {
                return new Exhibition() { title = "Neue Ausstellung", iconpath = "kein Bild gesetzt", width = 3, height = 3, pieces = new List<Piece>() };
            }
        }

        
    }
}