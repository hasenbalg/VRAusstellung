using System;
using System.Collections.Generic;
using System.Windows;
using LibVRAusstellung;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;

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

        internal void SetPieces(List<List<Piece>> pieces)
        {
            exhib.pieces = pieces;
            RefreshPiecesGrid();
        }

        private void Export()
        {
            RefreshExhibProperies();
            RefreshPiecesGrid();
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
            
            piecePropertiesControlHolderPanel.Children.Clear();
            piecePropertiesControlHolderPanel.Children.Add(newPiecePropertiesPanel);
            
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
        }
    }




}
