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
            InitializeComponent();
            Grid.ItemsSource = this.pieces;
            Grid.Items.Refresh();

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

            int tmpId = dropped.id;
            dropped.id = sndr.id;
            sndr.id = tmpId;

            List<Piece> orderedPieces = pieces.SelectMany(i => i).OrderBy(i => i.id).ToList();
            int k = 0;
            for (int i = 0; i < pieces.Count; i++)
            {
                for (int j = 0; j < pieces[i].Count; j++)
                {
                    pieces[i][j] = orderedPieces.First(p => p.id == k) as Piece;
                    k++;
                }
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

    public class PieceDataTemplateSelector : DataTemplateSelector
    {
        //http://dotnetpattern.com/wpf-listview-itemtemplateselector
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement elemnt = container as FrameworkElement;
           

            if (item is Entrance)
            {
                Console.WriteLine("Entrance");
                return elemnt.FindResource("DataTemplateEntrance") as DataTemplate;
            }
            else {
                return elemnt.FindResource("DataTemplatePiece") as DataTemplate;
            }
        }
    }
}
