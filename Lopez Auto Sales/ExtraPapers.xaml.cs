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
        private string PersonName { get; set; }
        private string VIN { get; set; }
        public ExtraPapers(string personName, string vin)
        {
            InitializeComponent();
            PersonName = personName;
            VIN = vin;
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (papers.Count == 0)
                return;

            if (!Storage.Papers.Exists(paper => paper.Buyer.Name == PersonName && paper.Car.VIN == VIN))
            {
                MessageBox.Show("Could not find paper info.", "Paper Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            PaperInfo paperInfo = Storage.Papers.Find(paper => paper.Buyer.Name == PersonName && paper.Car.VIN == VIN);

            if (papers.Contains("Contract"))
                MSEdit.PrintContract(paperInfo);
            if (papers.Contains("Warranty"))
                MSEdit.PrintWarranty(paperInfo);
            if (papers.Contains("Transfer Agreement"))
                MSEdit.PrintTransferAgreement(paperInfo);
            if (papers.Contains("Legal"))
                MSEdit.PrintLegal(paperInfo);
            if (papers.Contains("Lien Release"))
                MSEdit.PrintLien(paperInfo);

            MessageBox.Show("Papers are being printed.");
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "Extra Papers for " + PersonName;
            Storage.LoadPapers();
        }
    }
}
