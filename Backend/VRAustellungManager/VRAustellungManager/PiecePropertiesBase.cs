using LibVRAusstellung;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VRAustellungManager
{
    public  class PiecePropertiesBase : UserControl
    {
        protected Piece currentPiece;

        public PiecePropertiesBase()
        {

        }

        public void FillComboBox(ComboBox comboBox)
        {
            List<string> types = new List<string>();
            foreach (Type type in GetTypesOfPieces())
            {
                types.Add(GetDisplayNameOfPiece(type));
            }

            comboBox.ItemsSource = types;
        }

        

        private void FillForm(Text currentPiece)
        {
            throw new NotImplementedException();
        }

        private void BlankForm()
        {
            throw new NotImplementedException();
        }

        

       

        

        protected List<Type> GetTypesOfPieces()
        {
            return typeof(Piece).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Piece)) && !type.Equals(typeof(PieceWithFile))).ToList();
        }

        protected string GetDisplayNameOfPiece(Type type)
        {

            try
            {
                return (type.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute).DisplayName.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected void SendBackToMainWindow(Piece p)
        {
            (Window.GetWindow(this) as MainWindow).SetPiece(p);
        }

        protected void ChangeUserControl(Piece piece)
        {
            (Window.GetWindow(this) as MainWindow).SetPiecePropertiesPanel(piece);
        }
    }
}
