using LibVRAusstellung;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace VRAustellungManager
{

    public partial class PieceProperiesText : PiecePropertiesBase
    {
        protected Piece currentPiece { get; set; }

        public PieceProperiesText()
        {
            InitializeComponent();
            FillComboBox(pieceComboBox);
        }

        
        public void SetCurrentPiece(Text currentPiece)
        {
            this.currentPiece = currentPiece;
            Console.WriteLine(currentPiece.id);
            BlankForm();
            FillForm();
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

        private void FillForm()
        {
            Console.WriteLine("Fill Form Txt");
            pieceComboBox.SelectionChanged -= PieceComboBox_SelectionChanged;
            pieceNameTextBox.TextChanged -= PiecePropertyChanged;
            pieceDescriptionTextBox.TextChanged -= PiecePropertyChanged;

            pieceComboBox.SelectedValue = GetDisplayNameOfPiece(currentPiece.GetType());
            pieceIdTextBox.Text = currentPiece.id.ToString();
            pieceNameTextBox.Text = currentPiece.title;
            pieceDescriptionTextBox.Text = currentPiece.description;

            pieceComboBox.SelectionChanged += PieceComboBox_SelectionChanged;
            pieceNameTextBox.TextChanged += PiecePropertyChanged;
            pieceDescriptionTextBox.TextChanged += PiecePropertyChanged;
        }

        protected void PiecePropertyChanged(object sender, TextChangedEventArgs e)
        {
            PiecePropertyChanged();
        }

        protected void PiecePropertyChanged()
        {
            currentPiece.title = pieceNameTextBox.Text;
            currentPiece.description = pieceDescriptionTextBox.Text;
        }

        protected void BlankForm()
        {
            pieceIdTextBox.Text = string.Empty;
            pieceNameTextBox.Text = string.Empty;
            pieceDescriptionTextBox.Text = string.Empty;
        }

       
    }
}
