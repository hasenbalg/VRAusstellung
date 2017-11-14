using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using LibVRAusstellung;
using Microsoft.Win32;
using System.Windows.Controls;

namespace VRAustellungManager
{

    public partial class PieceProperties3D : PiecePropertiesBase
    {
        public PieceProperties3D() 
        {
            InitializeComponent();
            FillComboBox(pieceComboBox);
        }

        public LibVRAusstellung.ThreeDModel currentPiece { get; set; }
        protected string FILEFORMATS = "OBJ files (*.obj)|*.obj;";

        public void SetCurrentPiece(LibVRAusstellung.ThreeDModel currentPiece)
        {
            this.currentPiece = currentPiece;
            BlankForm();
            FillForm(currentPiece);
            this.IsEnabled = true;
        }

        private void FillForm(ThreeDModel currentPiece)
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

        protected void SetPreview(string fileName)
        {
            ModelVisual3D device3D = new ModelVisual3D();
            device3D.Content = Display3d(fileName);
            piece3DPreview.Children.Clear();
            piece3DPreview.Children.Add(device3D);
            piece3DPreview.Children.Add(new DefaultLights());
        }

        private void BlankForm()
        {
            pieceIdTextBox.Text = string.Empty;
            pieceNameTextBox.Text = string.Empty;
            pieceDescriptionTextBox.Text = string.Empty;
            pieceFileSelectURLTextBlock.Text = string.Empty;
            piece3DPreview.Children.Clear();
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
