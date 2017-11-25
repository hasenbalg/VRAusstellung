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

    public abstract class PieceWithFile :Piece
    {
        public string filePath { get; set; }
        public string fileformat { get; set; }

    }

    [DisplayName("Bild")]
    public class Image: PieceWithFile
    {
        
    }

    [DisplayName("Video")]
    public class Video : PieceWithFile
    {
       
    }

    [DisplayName("Audio")]
    public class Audio : PieceWithFile
    {
        public string artwork { get; set; }
    }

    [DisplayName("3D-Modell")]
    public class ThreeDModel : PieceWithFile
    {
        
    }
}
