

using LibVRAusstellung;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace VRAustellungManager
{
    /// <summary>
    /// Interaction logic for PiecePropertiesAudio.xaml
    /// </summary>
    public partial class PiecePropertiesAudio : PiecePropertiesBase
    {
        public Audio currentPiece { get; set; }
        protected string FILEFORMATS = "Video files (*.mp3)|*.mp3";
        public PiecePropertiesAudio()
        {
            InitializeComponent();
            FillComboBox(pieceComboBox);
            InitDispatcherTime();
        }

        public void SetCurrentPiece(Audio currentPiece)
        {
            this.currentPiece = currentPiece;
            BlankForm();
            FillForm(currentPiece);
            this.IsEnabled = true;
        }

        protected void FillForm(Audio currentPiece)
        {
            pieceComboBox.SelectionChanged -= PieceComboBox_SelectionChanged;
            pieceNameTextBox.TextChanged -= PiecePropertyChanged;
            pieceDescriptionTextBox.TextChanged -= PiecePropertyChanged;

            pieceComboBox.SelectedValue = GetDisplayNameOfPiece(currentPiece.GetType());
            pieceIdTextBox.Text = currentPiece.id.ToString();
            pieceNameTextBox.Text = currentPiece.title;
            pieceDescriptionTextBox.Text = currentPiece.description;
            pieceFileSelectURLTextBlock.Text = currentPiece.filePath;

            pieceComboBox.SelectionChanged += PieceComboBox_SelectionChanged;
            pieceNameTextBox.TextChanged += PiecePropertyChanged;
            pieceDescriptionTextBox.TextChanged += PiecePropertyChanged;
            SetPreview(currentPiece.filePath);
        }

        protected void PiecePropertyChanged(object sender, TextChangedEventArgs e)
        {
            PiecePropertyChanged();
        }

        protected void PieceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Type type in GetTypesOfPieces())
            {
                if (GetDisplayNameOfPiece(type).Equals((sender as ComboBox).SelectedValue))
                {
                    var newInstance = Activator.CreateInstance(type);
                    (newInstance as Piece).id = currentPiece.id;
                    (newInstance as Piece).title = currentPiece.title;
                    (newInstance as Piece).description = currentPiece.description;
                    ChangeUserControl((newInstance as Piece));
                }
            }
        }

        protected void PiecePropertyChanged()
        {
            currentPiece.title = pieceNameTextBox.Text;
            currentPiece.description = pieceDescriptionTextBox.Text;
            currentPiece.filePath = pieceFileSelectURLTextBlock.Text;
        }

        protected void PieceFileSelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FILEFORMATS;

            if (openFileDialog.ShowDialog() == true)
            {
                SetPreview(openFileDialog.FileName);
                pieceFileSelectURLTextBlock.Text = openFileDialog.FileName;
                PiecePropertyChanged();
            }
        }

        protected void SetPreview(string fileName)
        {
            pieceAVPreview.Visibility = Visibility.Visible;
            try
            {
                mePlayer.Source = new Uri(fileName);
            }
            catch (ArgumentNullException)
            {
                mePlayer.Source = new Uri(@"pack://application:,,,/Resources/Preview.png");
            }
            finally
            {
                mePlayer.Play();
                mePlayer.Stop();
            }
        }

        private void BlankForm()
        {
            pieceIdTextBox.Text = string.Empty;
            pieceNameTextBox.Text = string.Empty;
            pieceDescriptionTextBox.Text = string.Empty;
            pieceFileSelectURLTextBlock.Text = string.Empty;
            pieceFileSelectURLTextBlock.Text = string.Empty;
            mePlayer.Source = null;
        }

        //Audio
        protected void InitDispatcherTime()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null)
            {
                if (mePlayer.NaturalDuration.HasTimeSpan)
                    lblStatus.Content = String.Format("{0} / {1}", mePlayer.Position.ToString(@"mm\:ss"), mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                lblStatus.Content = "No file selected...";
        }

        protected void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Play();
        }

        protected void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Pause();
        }

        protected void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Stop();
        }
    }
}
