using Lopez_POS.JSON;
using Lopez_POS.Static;
using Lopez_POS.Web;
using System;
using System.Windows;

namespace Lopez_POS
{
    /// <summary>
    /// Interaction logic for CarAdder.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class CarAdder : Window
    {
        /// <summary>
        /// Gets the car.
        /// </summary>
        /// <value>
        /// The car.
        /// </value>
        public SalesCar Car { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CarAdder"/> class.
        /// </summary>
        public CarAdder()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the AddButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

            Car = new SalesCar(year, MakeBox.Text.ToCapital(), ModelBox.Text.ToCapital(), VINBox.Text.ToUpper(), mileage, price, lowestPrice, DateTime.Now, ColorComboBox.Text.ToCapital(), boughtPrice, SalvageCheckBox.IsChecked.Value, ExtraCheckBox.IsChecked.Value, ByOwnerCheckBox.IsChecked.Value);
            if (WarrantyCheckBox.IsChecked.Value)
                MSEdit.PrintWarranty(Car, warranty);
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Handles the Click event of the VINButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void VINButton_Click(object sender, RoutedEventArgs e)
        {
            if (VINBox.Text.Length < 11 || VINBox.Text.Length > 17)
            {
                MessageBox.Show("Invalid VIN length.");
                return;
            }

            JSONClass jsonClass = WebManager.DecodeVIN(VINBox.Text);
            Car car = jsonClass.GetBasics();

            if (car == null)
            {
                MessageBox.Show("VIN could not be verified.", "VIN Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (jsonClass.HasErrorCode())
                MessageBox.Show("VIN detected an error code. Double check to see if you entered it correctly.");

            YearBox.Text = car.Year.ToString();
            MakeBox.Text = car.Make;
            ModelBox.Text = car.Model;
        }
    }
}