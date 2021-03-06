﻿using System;
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

        public Color[] colorSchema;

        private string dir;
        private string xmlFileName;

        public static Exhibition newInstance()
        {
            Exhibition exhib = new Exhibition();
            exhib.title = "Neue Ausstellung";
            exhib.description = "Beschreibung der neuen Ausstellung";

            exhib.width = exhib.height = 3;

            exhib.pieces = GetNewList(exhib.width, exhib.height);

            exhib.colorSchema = new Color[]{
                new Color(0, 0, 0),
                new Color(5, 5, 5),
                new Color(15, 15, 15)
            };
           

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

        public static List<List<Piece>> GetNewList(int width, int height)
        {
            List<List<Piece>>pieces = new List<List<Piece>>();
            int k = 0;
            for (int i = 0; i < height; i++)
            {
                List<Piece> newRow = new List<Piece>();
                for (int j = 0; j < width; j++)
                {
                    newRow.Add(new Text()
                    {
                        id = k++,
                        title = "Neues Ausstellungsstueck",
                        description = "Beschreibung des neuen Ausstellungsssteucks"
                    });
                }
                pieces.Add(newRow);
            }
            return pieces;
        }

        public static List<List<Piece>> GetNewList(int width, int height, List<List<Piece>> oldPieces)
        {
            List<List<Piece>> emptyPieces = GetNewList(width, height);

            for (int i = 0; i < oldPieces.Count; i++)
            {
                for (int j = 0; j < oldPieces[i].Count; j++)
                {
                    if (i < emptyPieces.Count && j < emptyPieces[i].Count)
                    {
                        emptyPieces[i][j] = oldPieces[i][j];
                    }  
                }
            }

            return emptyPieces;
        }
    }
}