using LibVRAusstellung;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace VRAustellungManager
{
    
    public partial class ExhibitionProperties : UserControl
    {
        Exhibition exhib;

        public ExhibitionProperties()
        {
            InitializeComponent();
            this.IsEnabled = false;
        }

        public void SetExhibition(Exhibition exhib)
        {
            this.exhib = exhib;
            if (exhib != null)
            {
                this.IsEnabled = true;
                FillForm();
            }
        }

        private void FillForm()
        {
            exhibTitleTextBox.TextChanged -= exhibPropertyTextBox_TextChanged;
            exhibDescriptionTextBox.TextChanged -= exhibPropertyTextBox_TextChanged;

            firstColor.SelectedColorChanged -= Color_SelectedColorChanged;
            secondColor.SelectedColorChanged -= Color_SelectedColorChanged;
            thirdColor.SelectedColorChanged -= Color_SelectedColorChanged;

            exhibTitleTextBox.Text = exhib.title;
            exhibDescriptionTextBox.Text = exhib.description;
            exhibWidthTextBox.Text = exhib.width.ToString();
            exhibHeightTextBox.Text = exhib.height.ToString();

            firstColor.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.colorSchema[0]));
            secondColor.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.colorSchema[1]));
            thirdColor.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.colorSchema[2]));

            exhibTitleTextBox.TextChanged += exhibPropertyTextBox_TextChanged;
            exhibDescriptionTextBox.TextChanged += exhibPropertyTextBox_TextChanged;


            firstColor.SelectedColorChanged += Color_SelectedColorChanged;
            secondColor.SelectedColorChanged += Color_SelectedColorChanged;
            thirdColor.SelectedColorChanged += Color_SelectedColorChanged;
        }

        private void exhibPropertyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            exhib.title = exhibTitleTextBox.Text;
            exhib.description = exhibDescriptionTextBox.Text;
            exhib.width = Int32.Parse(exhibWidthTextBox.Text);
            exhib.height = Int32.Parse(exhibHeightTextBox.Text);
        }

        private void exhibDimensionhTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string width = exhibWidthTextBox.Text.Trim();
            string height = exhibHeightTextBox.Text.Trim();
            if (width != String.Empty && height != String.Empty)
            {
                (Window.GetWindow(this) as MainWindow).SetGridDimensions(
                Int32.Parse(width),
                Int32.Parse(height)
                );
            }
        }

        private void Color_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Console.WriteLine((sender as ColorPicker).SelectedColor.ToString().Replace("#FF", "#"));
            exhib.colorSchema[0] = LibVRAusstellung.Color.Hex2Color(firstColor.SelectedColor.ToString().Replace("#FF", "#"));
            exhib.colorSchema[1] = LibVRAusstellung.Color.Hex2Color(secondColor.SelectedColor.ToString().Replace("#FF", "#"));
            exhib.colorSchema[2] = LibVRAusstellung.Color.Hex2Color(thirdColor.SelectedColor.ToString().Replace("#FF", "#"));
        }
    }
}
