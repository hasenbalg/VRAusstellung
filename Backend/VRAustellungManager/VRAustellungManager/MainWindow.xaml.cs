using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace VRAustellungManager
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string xmlPath = @"D:\VRAusstellung\Backend\VRAustellungManager\VRAustellungManager\Objects.xml";
        int gridHeight = 3, gridWidth = 3;
        List<List<Piece>> pieces;

        Exhibition exeb;

        public MainWindow()
        {
            InitializeComponent();
            ReadXMLFile();
            BuildGrid();
            ResetForm();
        }

        private void ReadXMLFile()
        {
            XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Exhibition));
            try
            {
                using (FileStream fileStream = new FileStream(xmlPath, FileMode.Open))
                {
                    exeb = (Exhibition)serializer.Deserialize(fileStream);
                }
            }
            catch (Exception e)
            {

                throw;
            }
            finally {
                FillForm();
            }
           
        }

        private void FillForm()
        {
            ImageFilePath.Text = exeb.iconpath;
        }

        private void ExebLogo_ButtonClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    ImageFilePath.Text = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    //TxtFile.Text = null;
                    //TxtFile.ToolTip = null;
                    break;
            }
        }


        private void BuildGrid() {
            pieces = new List<List<Piece>>();
            int k = 0;
            for (int i = 0; i < gridWidth; i++)
            {
                pieces.Add(new List<Piece>());

                for (int j = 0; j < gridHeight; j++)
                {
                    if (exeb.pieces.Count > k && !pieces[i].Contains(exeb.pieces[k]))
                    {
                        exeb.pieces[k].id = k;
                        pieces[i].Add(exeb.pieces[k]);
                    }
                    else {
                        pieces[i].Add(new Piece() {id = k, title = "", description = "", fileformat = "", filePath = "" });
                    }
                    k++;
                }
            }
            InitializeComponent();
            PiecesGrid.ItemsSource = pieces;

            this.SizeToContent = SizeToContent.WidthAndHeight;
           
        }

        private void GridDimensionsChanged_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeGridDimensions();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Bitte geben Sie nur ganze Zahlen ein." + exc.Message);
            }
        }

        private void PieceButton_Click(object sender, RoutedEventArgs e)
        {
            Piece piece2Edit = (Piece)(sender as Button).DataContext;
            NameTextBox.Text = piece2Edit.title;
            DescriptionTextBox.Text = piece2Edit.description;
            PieceFileUrlTextBlock.Text = piece2Edit.filePath;
            NewPieceOKButton.Visibility = Visibility.Hidden;
            EditPieceOKButton.Visibility = Visibility.Visible;
            CancleButton.Visibility = Visibility.Visible;
        }

        private void NewPieceOKButton_Click(object sender, RoutedEventArgs e)
        {
            exeb.pieces.Add(new Piece {
                id = exeb.pieces.OrderBy(x => x.id).Last().id + 1,
                title = NameTextBox.Text,
                description = DescriptionTextBox.Text,
                filePath = PieceFileUrlTextBlock.Text
            });
        }

        private void EditPieceOKButton_Click(object sender, RoutedEventArgs e)
        {
            Piece piece2Edit = exeb.pieces.Find(x => x.id == Int32.Parse(IdTextBlock.Text));
            exeb.pieces.Add(new Piece
            {
                id = Int32.Parse(IdTextBlock.Text),
                title = NameTextBox.Text,
                description = DescriptionTextBox.Text,
                filePath = PieceFileUrlTextBlock.Text
            });
            CancleButton.Visibility = Visibility.Visible;
        }

        private void ChangeGridDimensions() {
            int newWidth, newHeight;
            newWidth = Int32.Parse(GridWidthTextBox.Text);
            newHeight = Int32.Parse(GridHeightTextBox.Text);
            if (newWidth < gridWidth || newHeight < gridHeight)
            {
                MessageBox.Show("Die aktuellen Dimensionen sind kleiner als die Austellung. Die aeusseren Objekte werden geloescht.");
            }
            gridWidth = Int32.Parse(GridWidthTextBox.Text);
            gridHeight = Int32.Parse(GridHeightTextBox.Text);

            GridWidthTextBox.Text = newWidth.ToString();
            GridHeightTextBox.Text = newHeight.ToString();
            BuildGrid();
        }

        private void CancleButton_Click(object sender, RoutedEventArgs e) {
            ResetForm();
        }

        private void ResetForm() {
            NewPieceOKButton.Visibility = Visibility.Visible;
            EditPieceOKButton.Visibility = Visibility.Hidden;
            CancleButton.Visibility = Visibility.Hidden;

            IdTextBlock.Text = string.Empty;
            NameTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            PieceFileUrlTextBlock.Text = string.Empty;
        }
    }
}
