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
using NReco.VideoConverter;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace VRAustellungManager
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//VRAusstellungX";
        string xmlPath = "//Object.xml";
        string videoThumbNailExtension = "_thumbnail.png";
        FFMpegConverter ffMpeg = new FFMpegConverter(); //https://www.nrecosite.com/video_converter_net.aspx
        int gridHeight = 3, gridWidth = 3;
        List<List<Piece>> pieces;

        Exhibition exeb;

        public MainWindow()
        {
            xmlPath = dir + xmlPath;
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
                exeb = new Exhibition() { title = "Neue Ausstellung", iconpath = "kein Bild gesetzt", pieces = new List<Piece>() };
                BuildGrid();
                Flush();
            }
            finally
            {
                FillForm();
            }

        }

        private void FillForm()
        {
            ImageFilePath.Text = exeb.iconpath;
        }

        private void ExebLogo_ButtonClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog() { Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg" }; //https://stackoverflow.com/a/16862178
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    string filePath = fileDialog.FileName;
                    ImageFilePath.Text = filePath;

                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    //TxtFile.Text = null;
                    //TxtFile.ToolTip = null;
                    break;
            }
        }

        private void PieceChoose_ButtonClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog() { Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|MP4 Files (*.mp4)|*.mp4|MP3 Files (*.mp3)|*.mp3|WAV Files (*.wav)|*.wav|OBJ Files (*.obj)|*.obj" }; //https://stackoverflow.com/a/16862178
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    string filePath = fileDialog.FileName;
                    PieceFileUrlTextBlock.Text = filePath;
                    PiecePreviewImage.Source = SetImageSource(filePath);
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    //TxtFile.Text = null;
                    //TxtFile.ToolTip = null;
                    break;
            }
        }


        private void BuildGrid()
        {
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
                    else
                    {
                        pieces[i].Add(new Piece() { id = k, title = "leer", description = "", fileformat = "", filePath = "" });
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
            IdTextBlock.Text = piece2Edit.id.ToString();
            NameTextBox.Text = piece2Edit.title;
            DescriptionTextBox.Text = piece2Edit.description;
            PieceFileUrlTextBlock.Text = piece2Edit.filePath;
            PiecePreviewImage.Source = SetImageSource(piece2Edit.filePath);
            switch (piece2Edit.fileformat)
            {
                case ".obj":
                    viewPort3d.Visibility = Visibility.Visible;
                    PiecePreviewImage.Visibility = Visibility.Collapsed;
                    break;
                default:
                    viewPort3d.Visibility = Visibility.Collapsed;
                    PiecePreviewImage.Visibility = Visibility.Visible;
                    break;
            }
           
            EditPanel.IsEnabled = true;
        }

        private ImageSource SetImageSource(string filePath)
        {
            //https://www.dotnetperls.com/image-wpf
            BitmapImage b = new BitmapImage();
            b.BeginInit();

            string extension = System.IO.Path.GetExtension(filePath);
            switch (extension)
            {
                case ".mp3":
                case ".wav":
                    b.UriSource = new Uri(@"pack://application:,,,/Resources/Audio.png"); //https://stackoverflow.com/q/12690774
                    break;
                case ".mp4":
                    //b.UriSource = new Uri(@"pack://application:,,,/Resources/Video.png");
                    string thumbNailPath =
                        thumbNailPath = System.IO.Path.GetTempPath() + System.IO.Path.GetFileNameWithoutExtension(filePath)
                        + videoThumbNailExtension;
                    if (!File.Exists(thumbNailPath))
                    {
                        ffMpeg.GetVideoThumbnail(filePath, thumbNailPath);
                    }
                    b.UriSource = new Uri(thumbNailPath);
                    break;
                case ".obj":
                    b.UriSource = new Uri(@"pack://application:,,,/Resources/3DModell.png");
                    //https://www.codeproject.com/Tips/882885/Display-D-Model-using-Window-Presentation-Foundat
                    ModelVisual3D device3D = new ModelVisual3D();
                    device3D.Content = Display3d(filePath);
                    viewPort3d.Children.Clear();
                    // Add to view port
                    viewPort3d.Children.Add(device3D);

                    break;
                case ".png":
                case ".jpg":
                case ".jpeg":
                    b.UriSource = new Uri(filePath);
                    break;
                default:
                    b.UriSource = new Uri(@"pack://application:,,,/Resources/Preview.png");
                    break;
            }

            b.EndInit();

            return b;

        }

        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                //Adding a gesture here
                viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);

                //Import 3D model file
                ModelImporter import = new ModelImporter();

                //Load the 3D model file
                device = import.Load(model);
            }
            catch (Exception e)
            {
                // Handle exception in case can not file 3D model
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }

        private void PieceOKButton_Click(object sender, RoutedEventArgs e)
        {
            Piece newPiece = new Piece
            {
                id = Int32.Parse(IdTextBlock.Text),
                title = NameTextBox.Text,
                description = DescriptionTextBox.Text,
                filePath = dir + "//" + System.IO.Path.GetFileName(PieceFileUrlTextBlock.Text),
                fileformat = System.IO.Path.GetExtension(PieceFileUrlTextBlock.Text) //fuehr vllt zu problemen
            };

            try
            {
                if (newPiece.filePath != PieceFileUrlTextBlock.Text)
                {
                    string destinationPath = dir + "//" + System.IO.Path.GetFileName(PieceFileUrlTextBlock.Text);
                    File.Copy(PieceFileUrlTextBlock.Text, destinationPath, true);//https://stackoverflow.com/a/44610221
                    if (System.IO.Path.GetExtension(newPiece.filePath) == ".mp4")
                    {
                        string thumbnailPath =
                             System.IO.Path.GetDirectoryName(destinationPath) +
                            System.IO.Path.GetFileNameWithoutExtension(destinationPath) + videoThumbNailExtension;
                        ffMpeg.GetVideoThumbnail(destinationPath, thumbnailPath);
                    }

                }
                exeb.pieces = exeb.pieces.Where(x => x.id != Int32.Parse(IdTextBlock.Text)).ToList();
                exeb.pieces.Add(newPiece);

                Flush();
                ResetForm();
                BuildGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }

        private void PieceDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Piece piece2Edit = exeb.pieces.Find(x => x.id == Int32.Parse(IdTextBlock.Text));
            exeb.pieces.Remove(piece2Edit);
            Flush();
            ResetForm();
            BuildGrid();
        }

        private void ChangeGridDimensions()
        {
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

        private void CancleButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            EditPanel.IsEnabled = false;


            IdTextBlock.Text = string.Empty;
            NameTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            PieceFileUrlTextBlock.Text = string.Empty;
            PiecePreviewImage.Source = null;

        }

        private void Flush()
        {
            exeb.pieces = exeb.pieces.OrderBy(x => x.id).ToList();
            Directory.CreateDirectory(dir);//https://msdn.microsoft.com/en-us/library/54a0at6s.aspx
            try
            {
                XmlSerializer writer = new XmlSerializer(typeof(Exhibition));
                FileStream file = File.Create(xmlPath);
                writer.Serialize(file, exeb);
                file.Close();
            }
            catch (IOException e)
            {
                MessageBox.Show(xmlPath + "ist nicht schreibbar, die Festplatte ist voll, der Dateiname zu lang oder ???\n" + e.Message);
            }

        }
    }
}
