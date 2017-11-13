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
                    exhib.setFilePath(saveFileDialog.FileName);
                }
                Save();
            }
        }

        private void Export()
        {

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
        
    }




}
