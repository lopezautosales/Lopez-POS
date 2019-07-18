using Lopez_Auto_Sales.JSON;
using System;
using System.Windows;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for CarAdder.xaml
    /// </summary>
    public partial class CarAdder : Window
    {
        public SalesCar Car { get; private set; }

        public CarAdder()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(VINBox.Text) || String.IsNullOrEmpty(MakeBox.Text) || String.IsNullOrEmpty(ModelBox.Text) || String.IsNullOrEmpty(ColorComboBox.Text))
            {
                MessageBox.Show("Make sure there are no empty fields.");
                return;
            }
            if (!int.TryParse(YearBox.Text, out int year))
            {
                MessageBox.Show("Enter a correct year.");
                return;
            }

            int? mileage;
            if (int.TryParse(MileageBox.Text, out int miles))
            {
                mileage = miles;
            }
            else if (String.IsNullOrEmpty(MileageBox.Text) || MileageBox.Text.ToLower() == "exempt")
            {
                mileage = null;
            }
            else
            {
                MessageBox.Show("Enter a correct mileage.");
                return;
            }

            if (!decimal.TryParse(PriceBox.Text, out decimal price))
            {
                MessageBox.Show("Enter a correct price.");
                return;
            }
            if (!decimal.TryParse(LowestPriceBox.Text, out decimal lowestPrice))
            {
                MessageBox.Show("Enter a correct lowest price.");
                return;
            }
            if (!decimal.TryParse(BoughtPriceBox.Text, out decimal boughtPrice))
            {
                MessageBox.Show("Enter a correct bought price.");
                return;
            }

            if (!int.TryParse(WarrantyBox.Text, out int warranty))
            {
                MessageBox.Show("Enter a valid warranty.");
                return;
            }

            Car = new SalesCar(year, MakeBox.Text.ToCapital(), ModelBox.Text.ToCapital(), VINBox.Text.ToUpper(), mileage, price, lowestPrice, DateTime.Now, ColorComboBox.Text.ToCapital(), boughtPrice, SalvageCheckBox.IsChecked.Value, ExtraCheckBox.IsChecked.Value);
            if (WarrantyCheckBox.IsChecked.Value)
                MSEdit.PrintWarranty(Car, warranty);
            DialogResult = true;
            Close();
        }

        private void VINButton_Click(object sender, RoutedEventArgs e)
        {
            if (VINBox.Text.Length < 11 || VINBox.Text.Length > 17)
            {
                MessageBox.Show("Invalid VIN length.");
                return;
            }

            JSONClass jsonClass = WebManager.DecodeVIN(VINBox.Text);

            try
            {
                YearBox.Text = jsonClass.Results[8].Value.ToCapital();
                MakeBox.Text = jsonClass.Results[5].Value.ToCapital();
                ModelBox.Text = jsonClass.Results[7].Value.ToCapital();
            }
            catch
            {
                MessageBox.Show("VIN could not be verified.", "VIN Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}