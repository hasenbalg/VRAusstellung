using System;
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

namespace VRAustellungManager
{

    public partial class MainWindow : Window
    {

       public List<List<Piece>> pieces;
        public Exhibition exhib;

        public MainWindow()
        {

            InitializeComponent();
            
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
                        zip.AddFile(p.filePath, string.Empty);
                        Console.WriteLine("Added " + p.filePath + " to Zip");
                        p.filePath = newPath;
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
                File.Delete(tmpXMLFilePath);
                Open(oldXMLFilePath);
            }
            else{
                return;
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
