﻿using System;
using System.Collections.Generic;
using System.Windows;
using LibVRAusstellung;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using Ionic.Zip;
using NReco.VideoConverter;

namespace VRAustellungManager
{

    public partial class MainWindow : Window
    {

       public List<List<Piece>> pieces;
        public Exhibition exhib;

        List<string> tmpFiles2Delete;

        public MainWindow()
        {

            InitializeComponent();
            tmpFiles2Delete = new List<string>();
            
        }

        private void RefreshExhibProperies()
        {
            (exhibitionPropertiesControl as ExhibitionProperties).SetExhibition(exhib);
        }

        private void RefreshPiecesGrid()
        {
            (piecesGridControl as PiecesGrid).SetPieces(exhib.pieces);
        }


        private void New()
        {
            exhib = Exhibition.newInstance();
            RefreshExhibProperies();
            RefreshPiecesGrid();
            NoPiecePropertiesPanel();

        }

        private void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml;*.XML)|*.xml;*.XML";
            if (openFileDialog.ShowDialog() == true)
            {
                exhib = Exhibition.Deserialize(openFileDialog.FileName);

                RefreshExhibProperies();
                RefreshPiecesGrid();
            }
        }

        private void Open(string fileName)
        {
          exhib = Exhibition.Deserialize(fileName);
          RefreshExhibProperies();
          RefreshPiecesGrid();
        }


        private void Save()
        {
            if (exhib != null)
            {
                if (exhib.GetFilePath() != null)
                {
                    exhib.Serialize();
                }
                else
                {
                    SaveAs();
                }
                RefreshPiecesGrid();
            }
            else
            {
                MessageBox.Show("Nix da zum speichern.");
            }
        }

        private void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML files (*.xml)|*.xml;";
            if (saveFileDialog.ShowDialog() == true) {
                if (exhib != null)
                {
                    exhib.SetFilePath(saveFileDialog.FileName);
                }
                Save();
            }
        }

        private void SaveAs(string fileName)
        {

                if (exhib != null)
                {
                    exhib.SetFilePath(fileName);
                }
                Save();

        }

        internal void SetPieces(List<List<Piece>> pieces)
        {
            exhib.pieces = pieces;
            RefreshPiecesGrid();
            NoPiecePropertiesPanel();
        }

        private void Import()
        {
            throw new NotImplementedException();
        }


        private void Export()
        {
            if (exhib == null)
            {
                return;
            }

            RefreshPiecesGrid();
            NoPiecePropertiesPanel();

            Save();
            ZipAllFiles();
            TidyUpTmpFiles();



        }

        private void TidyUpTmpFiles()
        {
            foreach (string filePath in tmpFiles2Delete)
            {
                File.Delete(filePath);
            }
        }

        private void ZipAllFiles()
        {

            ZipFile zip = new ZipFile();
            for (int i = 0; i < exhib.pieces.Count; i++)
            {
                for (int j = 0; j < exhib.pieces[i].Count; j++)
                {
                    var p = exhib.pieces[i][j] as PieceWithFile;
                    if (p != null)
                    {
                        string newPath = Path.GetFileName(p.filePath);
                        if (p.filePath.ToLower().EndsWith(".obj"))
                        {
                            ZipObjMatTexFiles(zip, p);
                        }
                        else if (p.filePath.ToLower().EndsWith(".mp3") || p.filePath.ToLower().EndsWith(".wav"))
                        {
                            ZipAudio(zip, p);
                        }
                        else {
                            zip.AddFile(p.filePath, string.Empty);
                            Console.WriteLine("Added " + p.filePath + " to Zip");
                            p.filePath = newPath;
                        }
                    }
                }
            }

            string oldXMLFilePath = exhib.GetFilePath();
            string tmpXMLFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(oldXMLFilePath) +  ".xml");
            SaveAs(tmpXMLFilePath);
            zip.AddFile(exhib.GetFilePath(), string.Empty);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Zip file (*.zip)|*.zip";
            saveFileDialog.FileName = exhib.title.Replace(' ', '_') + ".zip";
            if (saveFileDialog.ShowDialog() == true)
            {
                string destPath = Path.Combine(
                    Path.GetDirectoryName(saveFileDialog.FileName),
                    Path.GetFileNameWithoutExtension(saveFileDialog.FileName) +
                    ".zip"
                    );
                zip.Save(destPath);
                tmpFiles2Delete.Add(tmpXMLFilePath);
                Open(oldXMLFilePath);
            }
            else{
                return;
            }
        }

        private void ZipAudio(ZipFile zip, PieceWithFile p)
        {
            string filePath = p.filePath;
            string tmpAiffPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(filePath)+ ".aiff");
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            ffMpeg.ConvertMedia(filePath, tmpAiffPath, Format.aiff);
            zip.AddFile(tmpAiffPath, string.Empty);
            tmpFiles2Delete.Add(tmpAiffPath);
            p.filePath = Path.GetFileName(tmpAiffPath);
        }

        private void ZipObjMatTexFiles(ZipFile zip, PieceWithFile p)
        {
            //OBJ FILE

            string filePath = p.filePath;
            string objFileText = string.Empty;
            string mtlFileName = string.Empty;

            using (StreamReader sr = File.OpenText(filePath))
            {
                string line = String.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Trim().StartsWith("mtllib"))
                    {
                        foreach (string token in line.Split(' '))
                        {
                            if (token.Contains(".mtl"))
                            {
                                mtlFileName = Path.GetFileName(token);
                            }
                        }
                        //substitute mtllib line
                        objFileText += "mtllib " + mtlFileName + "\n";
                    }
                    else {
                        objFileText += line + "\n";
                    }
                }
            }
            //write obj file
            string tmpObjPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(filePath));
            File.WriteAllText(tmpObjPath, objFileText);
            tmpFiles2Delete.Add(tmpObjPath);
            zip.AddFile(tmpObjPath, string.Empty);
            p.filePath = Path.GetFileName(filePath);

            //MTL FILE
            if (mtlFileName != string.Empty)
            {
                string mtlFileText = string.Empty;
                List<string> texFileNames = new List<string>();
                string currentTexFileName = string.Empty;
                using (StreamReader sr = File.OpenText(Path.Combine(Path.GetDirectoryName(filePath),mtlFileName)))
                {
                    string line = String.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim().StartsWith("map_Kd"))
                        {
                            foreach (string token in line.Split(' '))
                            {
                                if (token.ToLower().Contains(".png")
                                    || token.ToLower().Contains(".jpg"))
                                {
                                    currentTexFileName = Path.GetFileName(token);
                                    texFileNames.Add(currentTexFileName);

                                }
                            }
                            //substitute mtllib line
                            mtlFileText += "map_Kd " + currentTexFileName + "\n";
                        }
                        else
                        {
                            mtlFileText += line + "\n";
                        }
                    }
                }

                //write mtl file
                string tmpMTLPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(mtlFileName));
                File.WriteAllText(tmpMTLPath, mtlFileText);
                tmpFiles2Delete.Add(tmpMTLPath);
                zip.AddFile(tmpMTLPath, string.Empty);
                

                // Add Textures to zip
                foreach (string texPath in texFileNames)
                {
                    string dir = Path.GetDirectoryName(filePath);

                    zip.AddFile(Path.Combine(dir, texPath), string.Empty);
                }
            }
        }

        private void CloseAll()
        {
            if (exhib != null)
            {
                MessageBox.Show("Wollen Sie alle Aenderungen verwerfen?");
            }
            this.Close();
        }



        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            New();
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Open();
        }

        private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAs();
        }

        private void ExportCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Export();
        }

        private void ImportCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Import();
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CloseAll();
        }

        internal void SetPiecePropertiesPanel(Piece piece)
        {
            PiecePropertiesBase newPiecePropertiesPanel = null;
            switch (piece.GetType().ToString()) {
                case "LibVRAusstellung.Text":
                    newPiecePropertiesPanel = new PieceProperiesText();
                    (newPiecePropertiesPanel as PieceProperiesText).SetCurrentPiece(piece as Text);
                    break;
                case "LibVRAusstellung.Image":
                    newPiecePropertiesPanel = new PiecePropertiesImage();
                    (newPiecePropertiesPanel as PiecePropertiesImage).SetCurrentPiece(piece as Image);
                    break;
                case "LibVRAusstellung.Video":
                    newPiecePropertiesPanel = new PiecePropertiesVideo();
                    (newPiecePropertiesPanel as PiecePropertiesVideo).SetCurrentPiece(piece as Video);
                    break;
                case "LibVRAusstellung.Audio":
                    newPiecePropertiesPanel = new PiecePropertiesAudio();
                    (newPiecePropertiesPanel as PiecePropertiesAudio).SetCurrentPiece(piece as Audio);
                    break;
                case "LibVRAusstellung.ThreeDModel":
                    newPiecePropertiesPanel = new PieceProperties3D();
                    (newPiecePropertiesPanel as PieceProperties3D).SetCurrentPiece(piece as ThreeDModel);
                    break;
                default:
                    return;
            }

            NoPiecePropertiesPanel();
            piecePropertiesControlHolderPanel.Children.Add(newPiecePropertiesPanel);
            
        }

        private void NoPiecePropertiesPanel() {
            piecePropertiesControlHolderPanel.Children.Clear();
        }

        internal void SetPiece(Piece currentPiece)
        {
            for (int i = 0; i < exhib.pieces.Count; i++)
            {
                for (int j = 0; j < exhib.pieces[i].Count; j++)
                {
                    if (exhib.pieces[i][j].id == currentPiece.id)
                    {
                        exhib.pieces[i][j] = currentPiece;
                    }
                }
            }

            RefreshPiecesGrid();
        }
    }




}
