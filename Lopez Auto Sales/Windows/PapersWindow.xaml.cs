using System.Windows;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for PapersWindow.xaml
    /// </summary>
    public partial class PapersWindow : Window
    {
        public PapersWindow()
        {
            InitializeComponent();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (PaperInfoGrid.SelectedItem == null)
            {
                MessageBox.Show("Select some paper info to print", "Selection Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PaperInfo paperInfo = PaperInfoGrid.SelectedItem as PaperInfo;

            if (ContractCheckBox.IsChecked.Value)
                MSEdit.PrintContract(paperInfo);
            if (TransferCheckBox.IsChecked.Value)
                MSEdit.PrintTransferAgreement(paperInfo);
            if (WarrantyCheckBox.IsChecked.Value)
                MSEdit.PrintWarranty(paperInfo);
            if (LegalCheckBox.IsChecked.Value)
                MSEdit.PrintLegal(paperInfo);
            if (LienCheckBox.IsChecked.Value)
                MSEdit.PrintLien(paperInfo);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PaperInfoGrid.ItemsSource = null;
            PaperInfoGrid.ItemsSource = Storage.Papers;
        }
    }
}