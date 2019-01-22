using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for ExtraPapers.xaml
    /// </summary>
    public partial class ExtraPapers : Window
    {
        internal List<string> papers = new List<string>();
        public ExtraPapers()
        {
            InitializeComponent();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                if (!papers.Contains(checkBox.Content.ToString()))
                    papers.Add(checkBox.Content.ToString());
            }
            else
            {
                if (papers.Contains(checkBox.Content.ToString()))
                    papers.Remove(checkBox.Content.ToString());
            }
        }
    }
}
