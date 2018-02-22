using ICSharpCode.SharpZipLib.Zip;
using LibVRAusstellung;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Text;

public class LoadXML : MonoBehaviour {
    Exhibition exhib;
    public string extractDir;
    string currentpath;

    void Awake () {
        currentpath = Path.GetFullPath(Path.Combine(Application.dataPath, @"..\"));
        extractDir = Path.Combine(currentpath, "VRExhibData");
        //DeleteRestFromLastTime();
        bool shouldUnzip = true;
        if (Directory.Exists(extractDir))
        {
            string[] filesInDir = Directory.GetFiles(extractDir);

            foreach (string file in filesInDir)
            {
                if (Path.GetExtension(file) == ".xml")
                {
                    //If there is already an XML File in the target directory, we'll assume there's already an exhibition extracted there
                    shouldUnzip = false;
                }
            }
        }
        else
        {
            Directory.CreateDirectory(extractDir);
        }

        if (shouldUnzip)
        {
            //http://community.sharpdevelop.net/forums/t/21795.aspx
            // ZipConstants.DefaultCodePage = 0; //Sets CodePage to system default code page. 
            ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage; //Sets CodePage to UTF-8 encoding
            // Without either of these, unzipping files does not work after building the project.

            Unzip();
        }
        exhib = Exhibition.Deserialize(FindXMLFile(extractDir));
        FixPaths();
    }

    private void FixPaths()
    {
        for (int i = 0; i < exhib.pieces.Count; i++)
        {
            for (int j = 0; j < exhib.pieces[i].Count; j++)
            {
                if (exhib.pieces[i][j] is PieceWithFile)
                {
                    PieceWithFile pwfp = exhib.pieces[i][j] as PieceWithFile;
                    pwfp.filePath = Path.Combine(
                        extractDir, pwfp.filePath
                        );
                }
            }
        }
    }

    private void Unzip()
    {
        var info = new DirectoryInfo(currentpath);
        foreach (var file in info.GetFiles()) {
            if (file.Name.EndsWith(".zip"))
            {
                // create directory
                if (!Directory.Exists(extractDir))
                {
                    Directory.CreateDirectory(extractDir);
                }
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(file.FullName)))
                {

                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        // create directory
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        if (fileName != string.Empty)
                        {
                            using (FileStream streamWriter = File.Create(Path.Combine(extractDir, theEntry.Name)))
                            {
                                int size = 2048;
                                byte[] data = new byte[size];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else{
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private string FindXMLFile(string extractDir)
    {
        Debug.Log(extractDir);
        var info = new DirectoryInfo(extractDir);
        foreach (var file in info.GetFiles())
        {
            if (file.Name.EndsWith(".xml"))
            {
                return file.FullName;
            }
        }
        return string.Empty;
    }

    private void OnApplicationQuit()
    {
        DeleteRestFromLastTime();
    }

    private void DeleteRestFromLastTime()
    {
        if (Directory.Exists(extractDir))
        {
            var info = new DirectoryInfo(extractDir);
            foreach (var file in info.GetFiles())
            {
                File.Delete(file.FullName);
            }
            Directory.Delete(extractDir, true);
        }
    }

    public Exhibition GetExhibition() {
        return this.exhib;
    }
}
