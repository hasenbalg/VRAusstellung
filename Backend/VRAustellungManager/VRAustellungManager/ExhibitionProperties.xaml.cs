using LibVRAusstellung;
using System;
using System.Linq;
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

            floorColorPicker.SelectedColorChanged -= Color_SelectedColorChanged;
            doorColorPicker.SelectedColorChanged -= Color_SelectedColorChanged;
            skyColorPicker.SelectedColorChanged -= Color_SelectedColorChanged;
            textColorPicker.SelectedColorChanged -= Color_SelectedColorChanged;
            guideColorPicker.SelectedColorChanged -= Color_SelectedColorChanged;
            audioTimeLineColorPicker.SelectedColorChanged -= Color_SelectedColorChanged;
            videooTimeLineColorPicker.SelectedColorChanged -= Color_SelectedColorChanged;
            audioTimeMarkerColorPicker.SelectedColorChanged -= Color_SelectedColorChanged;
            videoTimeMarkerColorPicker.SelectedColorChanged -= Color_SelectedColorChanged;
            visitorMarkerColorPicker.SelectedColorChanged -= Color_SelectedColorChanged;

            exhibTitleTextBox.Text = exhib.title;
            exhibDescriptionTextBox.Text = exhib.description;
            exhibWidthTextBox.Text = exhib.width.ToString();
            exhibHeightTextBox.Text = exhib.height.ToString();

            floorColorPicker.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.floorColor));
            doorColorPicker.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.doorColor));
            skyColorPicker.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.skyColor));
            textColorPicker.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.textColor));
            guideColorPicker.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.guideColor));
            audioTimeLineColorPicker.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.audioTimeLineColor));
            videooTimeLineColorPicker.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.videoTimeLineColor));
            audioTimeMarkerColorPicker.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.audioMarkerColor));
            videoTimeMarkerColorPicker.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.videoMarkerColor));
            visitorMarkerColorPicker.SelectedColor = (System.Windows.Media.Color)ColorConverter.ConvertFromString(LibVRAusstellung.Color.ToHex(exhib.visitorMarkerColor));

            exhibTitleTextBox.TextChanged += exhibPropertyTextBox_TextChanged;
            exhibDescriptionTextBox.TextChanged += exhibPropertyTextBox_TextChanged;


            floorColorPicker.SelectedColorChanged += Color_SelectedColorChanged;
            doorColorPicker.SelectedColorChanged += Color_SelectedColorChanged;
            skyColorPicker.SelectedColorChanged += Color_SelectedColorChanged;
            textColorPicker.SelectedColorChanged += Color_SelectedColorChanged;
            guideColorPicker.SelectedColorChanged += Color_SelectedColorChanged;
            audioTimeLineColorPicker.SelectedColorChanged += Color_SelectedColorChanged;
            videooTimeLineColorPicker.SelectedColorChanged += Color_SelectedColorChanged;
            audioTimeMarkerColorPicker.SelectedColorChanged += Color_SelectedColorChanged;
            videoTimeMarkerColorPicker.SelectedColorChanged += Color_SelectedColorChanged;
            visitorMarkerColorPicker.SelectedColorChanged += Color_SelectedColorChanged;
        }

        private void exhibPropertyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            exhib.title = exhibTitleTextBox.Text;
            exhib.description = exhibDescriptionTextBox.Text;
            exhib.width = Int32.Parse(exhibWidthTextBox.Text);
            exhib.height = Int32.Parse(exhibHeightTextBox.Text);

            Entrance entrance = exhib.pieces.SelectMany(p => p).OfType<Entrance>().First();
            entrance.title = exhib.title;
            entrance.description = exhib.description;
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
            //Console.WriteLine((sender as ColorPicker).SelectedColor.ToString().Replace("#FF", "#"));
            exhib.floorColor = LibVRAusstellung.Color.Hex2Color(floorColorPicker.SelectedColor.ToString().Replace("#FF", "#"));
            exhib.doorColor = LibVRAusstellung.Color.Hex2Color(doorColorPicker.SelectedColor.ToString().Replace("#FF", "#"));
            exhib.skyColor = LibVRAusstellung.Color.Hex2Color(skyColorPicker.SelectedColor.ToString().Replace("#FF", "#"));
            exhib.textColor = LibVRAusstellung.Color.Hex2Color(textColorPicker.SelectedColor.ToString().Replace("#FF", "#"));
            exhib.guideColor = LibVRAusstellung.Color.Hex2Color(guideColorPicker.SelectedColor.ToString().Replace("#FF", "#"));
            exhib.audioTimeLineColor = LibVRAusstellung.Color.Hex2Color(audioTimeLineColorPicker.SelectedColor.ToString().Replace("#FF", "#"));
            exhib.videoTimeLineColor = LibVRAusstellung.Color.Hex2Color(videooTimeLineColorPicker.SelectedColor.ToString().Replace("#FF", "#"));
            exhib.audioMarkerColor = LibVRAusstellung.Color.Hex2Color(audioTimeMarkerColorPicker.SelectedColor.ToString().Replace("#FF", "#"));
            exhib.videoMarkerColor = LibVRAusstellung.Color.Hex2Color(videoTimeMarkerColorPicker.SelectedColor.ToString().Replace("#FF", "#"));
            exhib.visitorMarkerColor = LibVRAusstellung.Color.Hex2Color(visitorMarkerColorPicker.SelectedColor.ToString().Replace("#FF", "#"));

        }
    }
}
