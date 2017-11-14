using LibVRAusstellung;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace VRAustellungManager
{

    public partial class PiecePropertiesImage : PiecePropertiesBase
    {
        public LibVRAusstellung.Image currentPiece { get; set; }
        protected string FILEFORMATS = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";

        public PiecePropertiesImage()
        {
            InitializeComponent();
            FillComboBox(pieceComboBox);
        }

        public void SetCurrentPiece(LibVRAusstellung.Image currentPiece)
        {
            this.currentPiece = currentPiece;
            BlankForm();
            FillForm(currentPiece);
            this.IsEnabled = true;
        }


        private void FillForm(LibVRAusstellung.Image currentPiece)
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

        protected void PieceFileSelectButton_Click(object sender, RoutedEventArgs e)
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
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            try
            {
                b.UriSource = new Uri(fileName);
            }
            catch (Exception)
            {
                b.UriSource = new Uri(@"pack://application:,,,/Resources/Preview.png");
            }
            finally {
                b.EndInit();
                pieceImagePreview.Source = b;
            }
        }

        private void BlankForm()
        {
            pieceIdTextBox.Text = string.Empty;
            pieceNameTextBox.Text = string.Empty;
            pieceDescriptionTextBox.Text = string.Empty;
            pieceFileSelectURLTextBlock.Text = string.Empty;
            pieceImagePreview.Source = null;
        }
    }
}
