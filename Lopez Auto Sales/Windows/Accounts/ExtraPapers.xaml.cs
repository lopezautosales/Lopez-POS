using Lopez_Auto_Sales.Static;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for ExtraPapers.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class ExtraPapers : Window
    {
        /// <summary>
        /// The papers
        /// </summary>
        internal List<string> papers = new List<string>();
        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        /// <value>
        /// The name of the person.
        /// </value>
        private string PersonName { get; set; }
        /// <summary>
        /// Gets or sets the vin.
        /// </summary>
        /// <value>
        /// The vin.
        /// </value>
        private string VIN { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtraPapers"/> class.
        /// </summary>
        /// <param name="personName">Name of the person.</param>
        /// <param name="vin">The vin.</param>
        public ExtraPapers(string personName, string vin)
        {
            InitializeComponent();
            PersonName = personName;
            VIN = vin;
        }

        /// <summary>
        /// Handles the Click event of the PrintButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (papers.Count == 0)
                return;

            if (!Storage.PapersList.Exists(paper => paper.Buyer.Name == PersonName && paper.Car.VIN == VIN))
            {
                MessageBox.Show("Could not find paper info.", "Paper Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            PaperInfo paperInfo = Storage.PapersList.Find(paper => paper.Buyer.Name == PersonName && paper.Car.VIN == VIN);

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

        /// <summary>
        /// Handles the Checked event of the CheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "Extra Papers for " + PersonName;
        }
    }
}