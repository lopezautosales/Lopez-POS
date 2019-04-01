using System.Windows;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for InformationWindow.xaml
    /// </summary>
    public partial class InformationWindow : Window
    {
        public InformationWindow()
        {
            InitializeComponent();
        }

        private void PapersButton_Click(object sender, RoutedEventArgs e)
        {
            PapersWindow papersWindow = new PapersWindow();
            papersWindow.Show();
            this.Close();
        }

        private void ExpiredContractsButton_Click(object sender, RoutedEventArgs e)
        {
            ExpiredContractsWindow expiredWindow = new ExpiredContractsWindow();
            expiredWindow.Show();
            this.Close();
        }

        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not Implemented Yet: Gathering Data");
        }
    }
}
