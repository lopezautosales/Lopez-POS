﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for SellCarWindow.xaml
    /// </summary>
    public partial class SellCarWindow : Window
    {
        private decimal Due { get; set; }
        private decimal Balance { get; set; }
        private SalesCar InCar { get; set; }

        public SellCarWindow(SalesCar car = null)
        {
            InitializeComponent();
            InCar = car;
        }

        private bool CheckPerson(out Person person)
        {
            person = null;
            if (String.IsNullOrEmpty(BuyerBox.Text) || String.IsNullOrEmpty(AddressBox.Text) || String.IsNullOrEmpty(CityBox.Text) || String.IsNullOrEmpty(StateBox.Text) || String.IsNullOrEmpty(ZipBox.Text))
            {
                MessageBox.Show("Some buyer info is blank.", "Info Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!String.IsNullOrEmpty(PhoneBox.Text) && PhoneBox.Text.Length != 13)
            {
                MessageBox.Show("Enter a 10 digit phone number or leave blank.", "Phone Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            person = new Person(0, BuyerBox.Text.ToCapital(), PhoneBox.Text, AddressBox.Text.ToCapital(), CityBox.Text.ToCapital(), StateBox.Text.ToCapital(), ZipBox.Text, null);
            return true;
        }

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

            if (!decimal.TryParse(DownBox.Text, out decimal down))
            {
                MessageBox.Show("Enter a valid down payment.", "Down Payment Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TaxBox.Text, out decimal taxRate))
            {
                MessageBox.Show("Enter a valid tax rate.", "Tax Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Balance < 0)
            {
                MessageBox.Show("Cannot have a negative balance.", "Balance Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal average = 300;
            if (Balance > 0 && (!decimal.TryParse(AveragePaymentBox.Text, out average) || average == 0))
            {
                MessageBox.Show("Invalid average payment amount.", "Average Payment Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(BoughtPriceBox.Text, out decimal boughtPrice))
            {
                MessageBox.Show("Enter a valid bought price or 0 if unknown.", "Bought Price Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(WarrantyBox.Text, out int warranty))
            {
                MessageBox.Show("Enter a valid warranty.", "Warranty Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            taxRate /= 100.0m;

            //print papers
            PaperInfo paperInfo = new PaperInfo(DateTime.Now, buyer, CoBuyerBox.Text.ToCapital(), car, trade, down, TagCheckBox.IsChecked.Value, LienCheckBox.IsChecked.Value, OutOfStateCheckBox.IsChecked.Value, taxRate, warranty, average);
            MSEdit.PrintPapers(paperInfo, Balance != 0);
            Storage.AddPaperInfo(paperInfo);

            if (Balance != 0)
            {
                int personID;
                Person person = null;

                //Find existing customer info
                bool exists = Storage.People.Exists(p => p.Name == buyer.Name);
                if (exists)
                {
                    person = Storage.People.Find(p => p.Name == buyer.Name);
                    personID = person.PersonID;
                }
                else
                    personID = Storage.AddPerson(buyer.Name, buyer.Phone, buyer.Full_Address);

                //Get ID's
                int carID = Storage.AddPaymentCar(personID, buyer.Name, car.Year, car.Make, car.Model, car.Mileage, car.VIN, Due, average, DateTime.Now, car.Color);
                int paymentID = Storage.AddPayment(carID, buyer.Name, car.ToString(), DateTime.Now, down, true);

                //Make Entry
                List<Payment> payments = new List<Payment>() { new Payment(paymentID, carID, DateTime.Now, down, true) };
                PaymentCar paymentCar = new PaymentCar(buyer.Name, car.Year, car.Make, car.Model, car.Mileage, car.VIN, Due, average, car.Color, DateTime.Now, payments, personID, carID);

                //Add to storage
                if (exists)
                    Storage.People[Storage.People.IndexOf(person)].Cars.Add(paymentCar);
                else
                    Storage.People.Add(new Person(personID, buyer.Name, buyer.Phone, buyer.Full_Address, new List<PaymentCar>() { paymentCar }));

                //Update main window's entries
                if (Owner is MainWindow mainWindow)
                    mainWindow.Search_TextChanged(null, null);
            }

            MSEdit.AddEndOfYear(paperInfo, boughtPrice);
            RemoveSalesCar(car);
            AddSale(buyer, car, boughtPrice);
        }

        private void RemoveSalesCar(Car car)
        {
            if (Storage.SalesCars.Exists(c => c.VIN == car.VIN))
            {
                Storage.RemoveSalesCar(Storage.SalesCars.Find(c => c.VIN == car.VIN));
            }
        }

        private void AddSale(Person buyer, Car car, decimal boughtPrice)
        {
            bool salvage = false;
            DateTime listDate = DateTime.Now;

            if (InCar != null)
            {
                salvage = InCar.Salvage;
                listDate = InCar.ListDate;
            }

            Storage.AddSales(buyer.Name, car.VIN, car.Year, car.Make, car.Model, car.Color, car.Mileage, car.Value, boughtPrice, listDate, DateTime.Now, salvage);
        }

        private void PhoneBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhoneBox.Text.Length == 10 && double.TryParse(PhoneBox.Text, out double number))
                PhoneBox.Text = String.Format("{0:(###)###-####}", number);
        }

        private void PhoneBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void LienCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (Balance == 0 && LienCheckBox.IsChecked == true)
            {
                MessageBox.Show("Can't charge for a lien when there is 0 due.", "Paper Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                LienCheckBox.IsChecked = false;
            }
            else
                UpdateInfo();
        }

        private void UpdateInfo()
        {
            if (!IsLoaded)
                return;

            decimal.TryParse(PriceBox.Text, out decimal price);
            decimal.TryParse(TradeValueBox.Text, out decimal value);
            decimal difference = price - value;
            decimal.TryParse(DownBox.Text, out decimal down);
            decimal.TryParse(TaxBox.Text, out decimal taxRate);
            taxRate /= 100.0m;

            decimal tradeValue = 0;
            if (TradeExpander.IsExpanded)
                decimal.TryParse(TradeValueBox.Text, out tradeValue);

            decimal taxAmount = OutOfStateCheckBox.IsChecked.Value ? 0 : Math.Round(difference * taxRate, 2);
            decimal tag = TagCheckBox.IsChecked.Value ? Constants.TAG_COST : 0;
            decimal lien = LienCheckBox.IsChecked.Value ? Constants.LIEN_COST : 0;
            decimal subtotal = difference + taxAmount + tag + lien;

            PriceLabel.Content = price.ToString("C");
            TradeValueLabel.Content = value.ToString("C");
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateInfo();
        }

        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {
            UpdateInfo();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BuyerBox.ItemsSource = null;
            StateBox.ItemsSource = null;
            VINBox.ItemsSource = null;

            BuyerBox.ItemsSource = Storage.People;
            StateBox.ItemsSource = Constants.States;
            VINBox.ItemsSource = Storage.SalesCars;

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

        private async void CheckButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (VINBox.Text.Length < 11 || VINBox.Text.Length > 17)
            {
                MessageBox.Show("Invalid VIN length.", "VIN Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Storage.SalesCars.Exists(c => c.VIN == VINBox.Text))
            {
                InCar = Storage.SalesCars.Find(c => c.VIN == VINBox.Text);

                VINBox.Text = InCar.VIN;
                YearBox.Text = InCar.Year.ToString();
                MakeBox.Text = InCar.Make;
                ModelBox.Text = InCar.Model;
                MileageBox.Text = InCar.Mileage == null ? "Exempt" : InCar.Mileage.Value.ToString();
                PriceBox.Text = InCar.Price.ToString("N2");
                ColorBox.Text = InCar.Color;
                BoughtPriceBox.Text = InCar.BoughtPrice.ToString("N2");
            }
            else
            {
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

        private async void TradeCheckButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (TradeVINBox.Text.Length < 11 || TradeVINBox.Text.Length > 17)
            {
                MessageBox.Show("Invalid VIN length.", "VIN Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            JSONClass jsonClass = await VINDecoder.DecodeVINAsync(TradeVINBox.Text);
            try
            {
                TradeYearBox.Text = jsonClass.Results[8].Value.ToCapital();
                TradeMakeBox.Text = jsonClass.Results[5].Value.ToCapital();
                TradeModelBox.Text = jsonClass.Results[7].Value.ToCapital();
            }
            catch
            {
                MessageBox.Show("VIN could not be verified.", "VIN Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

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