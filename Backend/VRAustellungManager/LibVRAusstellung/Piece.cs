using System.ComponentModel;
using System.Xml.Serialization;

namespace LibVRAusstellung
{
    [XmlInclude(typeof(Text))]
    [XmlInclude(typeof(Image))]
    [XmlInclude(typeof(Video))]
    [XmlInclude(typeof(Audio))]
    [XmlInclude(typeof(ThreeDModel))]

    public abstract class Piece
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }

    }

    [DisplayName("Text")]
    public class Text : Piece
    {

    }

    [DisplayName("Bild")]
    public class Image: Piece{
         public string filePath { get; set; }
        public string fileformat { get; set; }
    }

    [DisplayName("Video")]
    public class Video : Image
    {
        public string filePath { get; set; }
        public string fileformat { get; set; }
    }

    [DisplayName("Audio")]
    public class Audio : Image
    {
        public string artwork { get; set; }
    }

    [DisplayName("3D-Modell")]
    public class ThreeDModel : Image
    {
        
    }
}
