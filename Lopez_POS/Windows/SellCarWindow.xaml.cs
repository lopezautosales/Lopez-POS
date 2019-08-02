using Lopez_POS.JSON;
using Lopez_POS.Static;
using Lopez_POS.Web;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lopez_POS
{
    /// <summary>
    /// Interaction logic for SellCarWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class SellCarWindow : Window
    {
        /// <summary>
        /// Gets or sets the due.
        /// </summary>
        /// <value>
        /// The due.
        /// </value>
        private decimal Due { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        private decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the window's car.
        /// </summary>
        /// <value>
        /// The in car.
        /// </value>
        private SalesCar InCar { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SellCarWindow"/> class.
        /// </summary>
        /// <param name="car">The car.</param>
        public SellCarWindow(SalesCar car = null)
        {
            InitializeComponent();
            InCar = car;
        }

        /// <summary>
        /// Checks the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        private bool CheckPerson(out Person person)
        {
            person = null;
            if (String.IsNullOrEmpty(BuyerBox.Text) || String.IsNullOrEmpty(AddressBox.Text) || String.IsNullOrEmpty(CityBox.Text) || String.IsNullOrEmpty(StateBox.Text) || String.IsNullOrEmpty(ZipBox.Text))
            {
                MessageBox.Show("Some buyer info is blank.");
                return false;
            }
            if (!String.IsNullOrEmpty(PhoneBox.Text) && PhoneBox.Text.Length != 12)
            {
                MessageBox.Show("Enter a 10 digit phone number or leave blank. ###-###-####");
                return false;
            }
            person = new Person(0, BuyerBox.Text.ToCapital(), PhoneBox.Text, AddressBox.Text.ToCapital(), CityBox.Text.ToCapital(), StateBox.Text.ToCapital(), ZipBox.Text, null);
            return true;
        }

        /// <summary>
        /// Checks the selling car.
        /// </summary>
        /// <param name="car">The car.</param>
        /// <returns></returns>
        private bool CheckCar(out Car car)
        {
            car = null;
            if (VINBox.Text.Length < 11 || VINBox.Text.Length > 17)
            {
                MessageBox.Show("Invalid VIN length.", "VIN Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!int.TryParse(YearBox.Text, out int year))
            {
                MessageBox.Show("Enter a valid year.", "Year Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (String.IsNullOrEmpty(MakeBox.Text) || String.IsNullOrEmpty(ModelBox.Text) || String.IsNullOrEmpty(ColorBox.Text))
            {
                MessageBox.Show("Check for empty car fields.", "Car Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            int? mileage;
            if (String.IsNullOrEmpty(MileageBox.Text) || MileageBox.Text.ToLower() == "exempt")
            {
                mileage = null;
            }
            else if (int.TryParse(MileageBox.Text, out int milesCheck))
            {
                mileage = milesCheck;
            }
            else
            {
                MessageBox.Show("Car mileage is invalid. Enter a number or exempt/leave empty.", "Mileage Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(PriceBox.Text, out decimal price))
            {
                MessageBox.Show("Enter a valid car price.", "Price Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            car = new Car(year, MakeBox.Text.ToCapital(), ModelBox.Text.ToCapital(), ColorBox.Text.ToCapital(), VINBox.Text.ToUpper(), price, mileage);
            return true;
        }

        /// <summary>
        /// Checks the trade-in.
        /// </summary>
        /// <param name="car">The car.</param>
        /// <returns></returns>
        private bool CheckTrade(out Car car)
        {
            car = null;
            if (TradeVINBox.Text.Length < 11 || TradeVINBox.Text.Length > 17)
            {
                MessageBox.Show("Invalid VIN length.", "Trade VIN Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!int.TryParse(TradeYearBox.Text, out int year))
            {
                MessageBox.Show("Enter a valid year.", "Trade Year Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (String.IsNullOrEmpty(TradeMakeBox.Text) || String.IsNullOrEmpty(TradeModelBox.Text) || String.IsNullOrEmpty(TradeColorBox.Text))
            {
                MessageBox.Show("Check for empty car fields.", "Trade Info Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            int? mileage;
            if (String.IsNullOrEmpty(TradeMileageBox.Text) || TradeMileageBox.Text.ToLower() == "exempt")
            {
                mileage = null;
            }
            else if (int.TryParse(TradeMileageBox.Text, out int milesCheck))
            {
                mileage = milesCheck;
            }
            else
            {
                MessageBox.Show("Car mileage is invalid. Enter a number or exempt/leave empty.", "Trade Mileage Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(TradeValueBox.Text, out decimal value))
            {
                MessageBox.Show("Enter a valid trade value.", "Trade Value Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            car = new Car(year, TradeMakeBox.Text.ToCapital(), TradeModelBox.Text.ToCapital(), TradeColorBox.Text.ToCapital(), TradeVINBox.Text.ToUpper(), value, mileage);
            return true;
        }

        /// <summary>
        /// Checks window information and handles vehicle sales if successful.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            //Check correctness
            if (!CheckPerson(out Person buyer))
                return;
            if (!CheckCar(out Car car))
                return;

            Car trade;
            if (!TradeExpander.IsExpanded)
                trade = null;
            else if (!CheckTrade(out trade))
                return;

            if (!decimal.TryParse(DownBox.Text, out decimal down) || down < 0)
            {
                MessageBox.Show("Enter a valid down payment.");
                return;
            }

            if (!decimal.TryParse(TaxBox.Text, out decimal taxRate) || taxRate <= 0)
            {
                MessageBox.Show("Enter a valid tax rate.");
                return;
            }

            if (Balance < 0)
            {
                MessageBox.Show("Cannot have a negative balance.", "Balance Warning");
                return;
            }

            decimal average = 0;
            if (Balance > 0 && (!decimal.TryParse(AveragePaymentBox.Text, out average) || average <= 0))
            {
                MessageBox.Show("Invalid average payment amount.");
                return;
            }

            if (!decimal.TryParse(BoughtPriceBox.Text, out decimal boughtPrice) || boughtPrice < 0)
            {
                MessageBox.Show("Enter a valid bought price or 0 if unknown.");
                return;
            }

            if (!int.TryParse(WarrantyBox.Text, out int warranty) || warranty < 0)
            {
                MessageBox.Show("Enter a valid warranty amount.");
                return;
            }

            taxRate /= 100.0m;

            SellVehicle(buyer, car, trade, down, warranty, boughtPrice, average, taxRate);

            MessageBox.Show("Papers are being printed.");
        }

        /// <summary>
        /// Handles a vehicle sale.
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        /// <param name="car">The car.</param>
        /// <param name="trade">The trade.</param>
        /// <param name="down">Down.</param>
        /// <param name="warranty">The warranty.</param>
        /// <param name="boughtPrice">The bought price.</param>
        /// <param name="average">The average.</param>
        /// <param name="taxRate">The tax rate.</param>
        private void SellVehicle(Person buyer, Car car, Car trade, decimal down, int warranty, decimal boughtPrice, decimal average, decimal taxRate)
        {
            PaperInfo paperInfo = new PaperInfo(DateTime.Now, buyer, CoBuyerBox.Text.ToCapital(), car, trade, down, TagCheckBox.IsChecked.Value, LienCheckBox.IsChecked.Value, OutOfStateCheckBox.IsChecked.Value, taxRate, warranty, average);
            MSEdit.PrintPapers(paperInfo, Balance != 0);
            Storage.Papers.AddPaperInfo(paperInfo);

            if (Balance != 0)
                AddPaymentCar(buyer, car, down, average);

            MSEdit.AddEndOfYear(paperInfo, boughtPrice);
            AddSale(buyer, car, boughtPrice);

            if (Storage.SalesCarsList.Exists(c => c.VIN == car.VIN))
                Storage.SalesCars.RemoveSalesCar(Storage.SalesCarsList.Find(c => c.VIN == car.VIN));
        }

        /// <summary>
        /// Adds the payment car to storage.
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        /// <param name="car">The car.</param>
        /// <param name="down">Down.</param>
        /// <param name="average">The average.</param>
        private void AddPaymentCar(Person buyer, Car car, decimal down, decimal average)
        {
            int personID;
            Person person = null;

            //Find existing customer info
            bool exists = Storage.PeopleList.Exists(p => p.Name == buyer.Name);
            if (exists)
            {
                person = Storage.PeopleList.Find(p => p.Name == buyer.Name);
                personID = person.PersonID;
                Person newPerson = new Person(personID, buyer.Name, buyer.Phone, buyer.Full_Address, person.Cars);
                if (!person.Equals(newPerson))
                    Storage.People.UpdatePerson(person, newPerson);
            }
            else
                personID = Storage.People.AddPerson(buyer.Name, buyer.Phone, buyer.Full_Address);

            //Get ID's
            int carID = Storage.PaymentCars.AddPaymentCar(personID, car.Year, car.Make, car.Model, car.Mileage, car.VIN, Due, average, DateTime.Now, car.Color);
            int paymentID = Storage.Payments.AddPayment(carID, buyer.Name, car.ToString(), DateTime.Now, down, true);

            //Make Entry
            List<Payment> payments = new List<Payment>() { new Payment(paymentID, carID, DateTime.Now, down, true) };
            PaymentCar paymentCar = new PaymentCar(buyer.Name, car.Year, car.Make, car.Model, car.Mileage, car.VIN, Due, average, car.Color, DateTime.Now, payments, personID, carID);

            //Add to storage
            if (exists)
                Storage.PeopleList[Storage.PeopleList.IndexOf(person)].Cars.Add(paymentCar);
            else
                Storage.PeopleList.Add(new Person(personID, buyer.Name, buyer.Phone, buyer.Full_Address, new List<PaymentCar>() { paymentCar }));

            //Update main window's entries
            if (Owner is MainWindow mainWindow)
                mainWindow.Search_TextChanged(null, null);
        }

        /// <summary>
        /// Adds the sale to storage.
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        /// <param name="car">The car.</param>
        /// <param name="boughtPrice">The bought price.</param>
        private void AddSale(Person buyer, Car car, decimal boughtPrice)
        {
            bool salvage = false;
            DateTime listDate = DateTime.Now;

            if (InCar != null)
            {
                salvage = InCar.Salvage;
                listDate = InCar.ListDate;
            }
            else if (Storage.SalesCarsList.Exists(c => c.VIN == car.VIN))
            {
                SalesCar salesCar = Storage.SalesCarsList.Find(c => c.VIN == car.VIN);
                salvage = salesCar.Salvage;
                listDate = salesCar.ListDate;
            }

            Storage.Sales.AddSales(buyer.Name, car, boughtPrice, listDate, DateTime.Now, salvage);
        }

        /// <summary>
        /// Handles the TextChanged event of the PhoneBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void PhoneBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhoneBox.Text.Length == 10 && double.TryParse(PhoneBox.Text, out double number))
                PhoneBox.Text = String.Format("{0:###-###-####}", number);
        }

        /// <summary>
        /// Handles the PreviewTextInput event of the PhoneBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        private void PhoneBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        /// <summary>
        /// Handles the Checked event of the LienCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void LienCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (Balance == 0 && LienCheckBox.IsChecked == true)
            {
                MessageBox.Show("Can't charge for a lien when there is 0 due.");
                LienCheckBox.IsChecked = false;
            }
            else
                UpdateInfo();
        }

        /// <summary>
        /// Updates the window information.
        /// </summary>
        private void UpdateInfo()
        {
            if (!IsLoaded)
                return;

            decimal.TryParse(PriceBox.Text, out decimal price);
            decimal tradeValue = 0;
            if (TradeExpander.IsExpanded)
                decimal.TryParse(TradeValueBox.Text, out tradeValue);
            decimal difference = price - tradeValue;
            decimal.TryParse(DownBox.Text, out decimal down);
            decimal.TryParse(TaxBox.Text, out decimal taxRate);
            taxRate /= 100.0m;

            decimal taxAmount = OutOfStateCheckBox.IsChecked.Value ? 0 : Math.Round(difference * taxRate, 2);
            decimal tag = TagCheckBox.IsChecked.Value ? Constants.TAG_COST : 0;
            decimal lien = LienCheckBox.IsChecked.Value ? Constants.LIEN_COST : 0;
            decimal subtotal = difference + taxAmount + tag + lien;

            PriceLabel.Content = price.ToString("C");
            TradeValueLabel.Content = tradeValue.ToString("C");
            TradeDifferenceLabel.Content = difference.ToString("C");
            TaxLabel.Content = taxAmount.ToString("C");
            TagLabel.Content = tag.ToString("C");
            LienLabel.Content = lien.ToString("C");
            SubTotalLabel.Content = subtotal.ToString("C");
            Due = subtotal;
            Balance = subtotal - down;
            BalanceLabel.Content = Balance.ToString("C");

            if (Balance == 0)
                AveragePaymentLabel.Visibility = AveragePaymentBox.Visibility = Visibility.Hidden;
            else
                AveragePaymentLabel.Visibility = AveragePaymentBox.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the Checked event of the CheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateInfo();
        }

        /// <summary>
        /// Handles the Changed event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {
            UpdateInfo();
        }

        /// <summary>
        /// Loads information to window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BuyerBox.ItemsSource = null;
            StateBox.ItemsSource = null;
            VINBox.ItemsSource = null;

            BuyerBox.ItemsSource = Storage.PeopleList;
            StateBox.ItemsSource = Constants.States;
            VINBox.ItemsSource = Storage.SalesCarsList;

            if (InCar != null)
            {
                VINBox.Text = InCar.VIN;
                YearBox.Text = InCar.Year.ToString();
                MakeBox.Text = InCar.Make;
                ModelBox.Text = InCar.Model;
                MileageBox.Text = InCar.Mileage == null ? "Exempt" : InCar.Mileage.Value.ToString();
                PriceBox.Text = InCar.Price.ToString("N2");
                ColorBox.Text = InCar.Color;
                BoughtPriceBox.Text = InCar.BoughtPrice.ToString("N2");
            }
        }

        /// <summary>
        /// Checks the sales vehicle's VIN and fills in information if available.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (VINBox.Text.Length < 11 || VINBox.Text.Length > 17)
            {
                MessageBox.Show("Invalid VIN length.", "VIN Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            if (Storage.SalesCarsList.Exists(c => c.VIN == VINBox.Text))
            {
                InCar = Storage.SalesCarsList.Find(c => c.VIN == VINBox.Text);

                VINBox.Text = InCar.VIN;
                MileageBox.Text = InCar.Mileage == null ? "Exempt" : InCar.Mileage.Value.ToString();
                PriceBox.Text = InCar.Price.ToString("N2");
                ColorBox.Text = InCar.Color;
                BoughtPriceBox.Text = InCar.BoughtPrice.ToString("N2");
            }
        }

        /// <summary>
        /// Checks the trade-in's information.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TradeCheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (TradeVINBox.Text.Length < 11 || TradeVINBox.Text.Length > 17)
            {
                MessageBox.Show("Invalid VIN length.", "VIN Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            JSONClass jsonClass = WebManager.DecodeVIN(TradeVINBox.Text);
            Car car = jsonClass.GetBasics();

            if (car == null)
            {
                MessageBox.Show("VIN could not be verified.", "VIN Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (jsonClass.HasErrorCode())
                MessageBox.Show("VIN detected an error code. Double check to see if you entered it correctly.");

            TradeYearBox.Text = car.Year.ToString();
            TradeMakeBox.Text = car.Make;
            TradeModelBox.Text = car.Model;
        }

        /// <summary>
        /// Fills information associated with the Buyer's name.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BuyerButton_Click(object sender, RoutedEventArgs e)
        {
            if (BuyerBox.SelectedItem == null)
                return;
            Person person = BuyerBox.SelectedItem as Person;

            AddressBox.Text = person.Address;
            PhoneBox.Text = person.Phone;
            CityBox.Text = person.City;
            StateBox.Text = person.State;
            ZipBox.Text = person.ZIP;
        }
    }
}