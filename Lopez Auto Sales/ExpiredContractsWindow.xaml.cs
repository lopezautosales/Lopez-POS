using Lopez_Auto_Sales.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

            foreach(Person person in Storage.People)
            {
                foreach(PaymentCar car in person.Cars)
                {
                    if(car.ContractExpirationDate() < DateTime.Today) //needs to be added to view
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
