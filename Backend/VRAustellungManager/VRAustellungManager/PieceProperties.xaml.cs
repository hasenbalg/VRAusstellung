using HelixToolkit.Wpf;
using LibVRAusstellung;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace VRAustellungManager
{

    public partial class PieceProperties : UserControl
    {
       

        public Piece currentPiece { get; set; }
       


        public PieceProperties()
        {
            InitializeComponent();
            FillComboBox();


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

        }

        private void FillComboBox()
        {
            List<string> types = new List<string>();
            foreach (Type type in GetTypesOfPieces())
            {
                types.Add(GetDisplayNameOfPiece(type));
            }

            pieceComboBox.ItemsSource = types;
        }

        private List<Type> GetTypesOfPieces() {
            return typeof(Piece).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Piece))).ToList();
        }

        private string GetDisplayNameOfPiece(Type type) {
            return (type.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute).DisplayName.ToString();
        }


        private void PieceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Type type in GetTypesOfPieces()) {
                if (GetDisplayNameOfPiece(type).Equals(pieceComboBox.SelectedValue))
                {
                   Piece tmpPiece = Activator.CreateInstance(type) as Piece;
                    tmpPiece.id = currentPiece.id;
                    tmpPiece.title = currentPiece.title;
                    tmpPiece.description = currentPiece.description;

                    currentPiece = tmpPiece;
                }
            }

            pieceFileSelectButton.Visibility = System.Windows.Visibility.Collapsed;
            pieceAVPreview.Visibility = System.Windows.Visibility.Collapsed;
            pieceImagePreview.Visibility = System.Windows.Visibility.Collapsed;
            piece3DPreview.Visibility = System.Windows.Visibility.Collapsed;
            pieceFileSelectURLTextBlock.Visibility = System.Windows.Visibility.Collapsed;

            if (!pieceComboBox.SelectedValue.ToString().Equals("Text"))
            {
                pieceFileSelectButton.Visibility = System.Windows.Visibility.Collapsed;
                pieceFileSelectURLTextBlock.Visibility = System.Windows.Visibility.Collapsed;
            }
            switch (currentPiece.GetType().ToString())
            {
                case "LibVRAusstellung.Video":
                    pieceAVPreview.Visibility = System.Windows.Visibility.Visible;
                        break;
                case "LibVRAusstellung.Audio":
                    pieceAVPreview.Visibility = System.Windows.Visibility.Visible;
                    break;
                case "LibVRAusstellung.Image":
                    pieceImagePreview.Visibility = System.Windows.Visibility.Visible;
                    break;
                case "LibVRAusstellung.ThreeDModel":
                    piece3DPreview.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void PieceFileSelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            switch (currentPiece.GetType().ToString())
            {
                case "LibVRAusstellung.Video":
                    openFileDialog.Filter = "Video files (*.mp4)|*.mp4";
                    break;
                case "LibVRAusstellung.Audio":
                    openFileDialog.Filter = "Audio files (*.mp3;*.wav)|*.mp3;*.wav";
                    break;
                case "LibVRAusstellung.Image":
                    openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
                    break;
                case "LibVRAusstellung.ThreeDModel":
                    openFileDialog.Filter = "OBJ files (*.obj)|*.obj";
                    break;
                default:
                    break;
            }
            if (openFileDialog.ShowDialog() == true) {
                SetPreviews(openFileDialog.FileName);
            }
        }

        private void SetPreviews(string filePath) {
            switch (currentPiece.GetType().ToString())
            {
                case "LibVRAusstellung.Video":
                    mePlayer.Source = new Uri(filePath);
                    mePlayer.Play();
                    mePlayer.Stop();
                    break;
                case "LibVRAusstellung.Audio":
                    mePlayer.Source = new Uri(filePath);
                    mePlayer.Play();
                    mePlayer.Stop();
                    break;
                case "LibVRAusstellung.Image":
                    BitmapImage b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = new Uri(filePath);
                    b.EndInit();
                    pieceImagePreview.Source = b;
                    break;
                case "LibVRAusstellung.ThreeDModel":
                    ModelVisual3D device3D = new ModelVisual3D();
                    device3D.Content = Display3d(filePath);
                    piece3DPreview.Children.Clear();
                    piece3DPreview.Children.Add(device3D);
                    break;
                default:
                    break;
            }

        }

        public void SetCurrentPiece(Piece currentPiece) {
            this.currentPiece = currentPiece;
            FillForm();
        }

        public void FillForm()
        {
            pieceComboBox.SelectionChanged -= PieceComboBox_SelectionChanged;
            pieceNameTextBox.TextChanged -= PiecePropertyChanged;

            pieceIdTextBox.Text = currentPiece.id.ToString();
            pieceNameTextBox.Text = currentPiece.title;
            pieceDescriptionTextBox.Text = currentPiece.description;
            if (!currentPiece.GetType().ToString().Equals("LibVRAusstellung.Text"))
            {
                pieceFileSelectURLTextBlock.Text = (currentPiece as LibVRAusstellung.Image).filePath;
                SetPreviews((currentPiece as LibVRAusstellung.Image).filePath);
            }

        }

        private void PiecePropertyChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //Play Audio or Video
        void timer_Tick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null)
            {
                if (mePlayer.NaturalDuration.HasTimeSpan)
                    lblStatus.Content = String.Format("{0} / {1}", mePlayer.Position.ToString(@"mm\:ss"), mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                lblStatus.Content = "No file selected...";
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Stop();
        }

        //Display 3D
        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                piece3DPreview.RotateGesture = new MouseGesture(MouseAction.LeftClick);
                ModelImporter import = new ModelImporter();
                device = import.Load(model);
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }
    }
}
