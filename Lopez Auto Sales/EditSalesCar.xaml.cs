using Lopez_Auto_Sales.Cars;
using System;
using System.Windows;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for EditSalesCar.xaml
    /// </summary>
    public partial class EditSalesCar : Window
    {
       public SalesCar Car { get; private set; }
        public EditSalesCar(SalesCar car)
        {
            InitializeComponent();
            Car = car;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VINBox.Text = Car.VIN;
            YearBox.Text = Car.Year.ToString();
            MakeBox.Text = Car.Make;
            ModelBox.Text = Car.Model;
            MileageBox.Text = Car.Mileage.ToString();
            ColorComboBox.Text = Car.Color;
            ExtraCheckBox.IsChecked = Car.ExtraKey;
            PriceBox.Text = Car.Price.ToString("N2");
            LowestPriceBox.Text = Car.LowestPrice.ToString("N2");
            BoughtPriceBox.Text = Car.BoughtPrice.ToString("N2");
            SalvageCheckBox.IsChecked = Car.Salvage;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Car = null;
            DialogResult = true;
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(VINBox.Text) || String.IsNullOrEmpty(MakeBox.Text) || String.IsNullOrEmpty(ModelBox.Text) || String.IsNullOrEmpty(ColorComboBox.Text))
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
            else if(String.IsNullOrEmpty(MileageBox.Text) || MileageBox.Text.ToLower() == "exempt")
            {
                mileage = null;
            }
            else
            {
                MessageBox.Show("Enter a correct mileage.");
                return;
            }

            if(!decimal.TryParse(PriceBox.Text, out decimal price))
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

            Car = new SalesCar(year, MakeBox.Text.ToCapital(), ModelBox.Text.ToCapital(), VINBox.Text.ToUpper(), mileage, price, lowestPrice, Car.ListDate, ColorComboBox.Text.ToCapital(), boughtPrice, SalvageCheckBox.IsChecked.Value, ExtraCheckBox.IsChecked.Value);
            DialogResult = true;
            Close();
        }

        private async void VINButton_Click(object sender, RoutedEventArgs e)
        {
            if (VINBox.Text.Length < 11 || VINBox.Text.Length > 17)
            {
                MessageBox.Show("Invalid VIN length.");
                return;
            }
            JSONClass jsonClass = await VINDecoder.DecodeVINAsync(VINBox.Text);

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
