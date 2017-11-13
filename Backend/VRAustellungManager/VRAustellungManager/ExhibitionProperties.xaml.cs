using LibVRAusstellung;
using System;
using System.Windows.Controls;


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

            exhibTitleTextBox.Text = exhib.title;
            exhibDescriptionTextBox.Text = exhib.description;
            exhibWidthTextBox.Text = exhib.width.ToString();
            exhibHeightTextBox.Text = exhib.height.ToString();

            exhibTitleTextBox.TextChanged += exhibPropertyTextBox_TextChanged;
            exhibDescriptionTextBox.TextChanged += exhibPropertyTextBox_TextChanged;
        }

        private void exhibPropertyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            exhib.title = exhibTitleTextBox.Text;
            exhib.description = exhibDescriptionTextBox.Text;
            exhib.width = Int32.Parse(exhibWidthTextBox.Text);
            exhib.height = Int32.Parse(exhibHeightTextBox.Text);

        }

       
    }
}
