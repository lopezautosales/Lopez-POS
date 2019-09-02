using Lopez_POS.Web;
using Lopez_POS.Windows.Information;
using System.Windows;

namespace Lopez_POS
{
    /// <summary>
    /// Interaction logic for InformationWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class InformationWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InformationWindow"/> class.
        /// </summary>
        public InformationWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the PapersButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PapersButton_Click(object sender, RoutedEventArgs e)
        {
            PapersWindow papersWindow = new PapersWindow();
            papersWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the ExpiredContractsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ExpiredContractsButton_Click(object sender, RoutedEventArgs e)
        {
            ExpiredContractsWindow expiredWindow = new ExpiredContractsWindow();
            expiredWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the SalesButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            SalesWindow salesWindow = new SalesWindow();
            salesWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the JsonButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void JsonButton_Click(object sender, RoutedEventArgs e)
        {
            WebManager.CheckForUpdates();
            MessageBox.Show("Updated Website Cars");
        }
    }
}