using LibVRAusstellung;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VRAustellungManager
{

    public partial class PiecesGrid : UserControl
    {
        List<List<Piece>> pieces;
        public PiecesGrid()
        {
            InitializeComponent();
            
        }

        public void SetPieces(List<List<Piece>> pieces) {
            this.pieces = pieces;
            Grid.ItemsSource = this.pieces;
            InitializeComponent();

        }


        //Drag'Drop
        bool m_inMouseMove;

        public bool InvokeRequired { get; private set; }

        private void PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //https://stackoverflow.com/a/15780620
            if (!m_inMouseMove)
            {
                m_inMouseMove = true;
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    var btn = sender as TextBlock;
                    System.Console.WriteLine("Dragged: " + (string)(btn.DataContext as Piece).title);
                    DragDrop.DoDragDrop(btn, (Piece)btn.DataContext, DragDropEffects.All);
                    e.Handled = true;
                }
                m_inMouseMove = false;
            }
        }

        private void Drop(object sender, DragEventArgs e)
        {
            //https://stackoverflow.com/a/15780620
            Console.WriteLine("Dropped: " + (string)(e.Data.GetData(typeof(Piece)) as Piece).title);
            Console.WriteLine("on: " + (string)(((TextBlock)sender).DataContext as Piece).title);
            Piece dropped = e.Data.GetData(typeof(Piece)) as Piece;
            Piece sndr = ((TextBlock)sender).DataContext as Piece;
            Piece tmp = dropped;
            dropped = sndr;
            sndr = tmp;

            InitializeComponent();
        }



        private void PieceButton_Click(object sender, RoutedEventArgs e)
        {
            Piece piece2Edit = (Piece)(sender as Button).DataContext;
            ((Window.GetWindow(this) as MainWindow)
                .piecesPropertiesControl as PieceProperties).SetCurrentPiece(piece2Edit);
        }



    }
}
