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

        protected List<Type> GetTypesOfPieces()
        {
            return typeof(Piece).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Piece)) && !type.Equals(typeof(PieceWithFile))).ToList();
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
            //Console.WriteLine("Dropped: " + (string)(e.Data.GetData(typeof(Piece)) as Piece).title);
            //Console.WriteLine("on: " + (string)(((TextBlock)sender).DataContext as Piece).title);
            Piece dropped = null;
            foreach (Type type in GetTypesOfPieces())
            {
                if (dropped == null)
                {
                    dropped = e.Data.GetData(type) as Piece;
                }
            }
            Piece sndr = ((TextBlock)sender).DataContext as Piece;

            Console.WriteLine("swapped " + sndr.title + " with" + dropped.title);

            for (int i = 0; i < pieces.Count; i++)
            {
                for (int j = 0; j < pieces[i].Count; j++)
                {
                    if (pieces[i][j].id == dropped.id)
                    {
                        pieces[i][j] = sndr;
                    }
                    if (pieces[i][j].id == sndr.id)
                    {
                        pieces[i][j] = dropped;
                    }
                }
            }
            foreach (var item in pieces.SelectMany(i => i))
            {
                Console.WriteLine(item.id);
            }

            (Window.GetWindow(this) as MainWindow).SetPieces(pieces);
        }



        private void PieceButton_Click(object sender, RoutedEventArgs e)
        {
            Piece piece2Edit = (Piece)(sender as Button).DataContext;
            Console.WriteLine(piece2Edit.GetType());
            (Window.GetWindow(this) as MainWindow).SetPiecePropertiesPanel(piece2Edit);
        }



    }
}
