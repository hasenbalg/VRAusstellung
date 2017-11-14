using System;
using System.Collections.Generic;
using System.IO;


namespace LibVRAusstellung
{

    public class Exhibition
    {
        public string title { get; set; }

        public string description { get; set; }

        public int width { get; set; }

        public int height { get; set; }

        public List<List<Piece>> pieces { get; set; }

        private string dir;
        private string xmlFileName;

        public static Exhibition newInstance()
        {
            Exhibition exhib = new Exhibition();
            exhib.title = "Neue Ausstellung";
            exhib.description = "Beschreibung der neuen Ausstellung";

            exhib.width = exhib.height = 3;

            exhib.pieces = new List<List<Piece>>();
            int k = 0;
            for (int i = 0; i < exhib.height; i++)
            {
                List<Piece> newRow = new List<Piece>();
                for (int j = 0; j < exhib.width; j++)
                {
                    newRow.Add(new Text() {
                        id = k++,
                        title = "Neues Ausstellungsstueck",
                        description = "Beschreibung des neuen Ausstellungsssteucks"
                    });
                }
                exhib.pieces.Add(newRow);
            }

            return exhib;
        }

        public string GetFilePath()
        {
            if (dir != null && xmlFileName != null)
            {
                return Path.Combine(dir, xmlFileName);
            }
            else {
                return null;
            }
        }

        public void SetFilePath(string path)
        {
            this.dir = Path.GetDirectoryName(path);
            this.xmlFileName = Path.GetFileName(path);
        }

        public void Serialize()
        {

            foreach (var items in pieces)
            {
                Console.WriteLine("-------------------");
                foreach (var item in items)
                {
                    Console.WriteLine(item.GetType().ToString());
                }
            }


            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/how-to-read-object-data-from-an-xml-file
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(Exhibition));

            FileStream file = File.Create(GetFilePath());

            writer.Serialize(file, this);
            Console.WriteLine("Saved current data to:" + GetFilePath());
            file.Close();
        }

        public static Exhibition Deserialize(string fileName)
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/how-to-read-object-data-from-an-xml-file
            System.Xml.Serialization.XmlSerializer reader =
        new System.Xml.Serialization.XmlSerializer(typeof(Exhibition));
            StreamReader file = new StreamReader(
               fileName);
            Exhibition exhib = (Exhibition)reader.Deserialize(file);
            exhib.SetFilePath(fileName);
            file.Close();
            return exhib;
        }
    }
}