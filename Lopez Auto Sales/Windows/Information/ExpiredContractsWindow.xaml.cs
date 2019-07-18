using System;
using System.Windows;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for ExpiredContractsWindow.xaml
    /// </summary>
    public partial class ExpiredContractsWindow : Window
    {
        public decimal TotalDue { get; private set; }

        public ExpiredContractsWindow()
        {
            InitializeComponent();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            TotalDue = 0;

            foreach (Person person in Storage.People)
            {
                foreach (PaymentCar car in person.Cars)
                {
                    if (car.ContractExpirationDate() < DateTime.Today) //needs to be added to view
                    {
                        EntriesGrid.Items.Add(car);
                        TotalDue += car.Balance;
                    }
                }
            }

            totalBlock.Text = "Total Due: " + TotalDue.ToString("C");
        }
    }
}