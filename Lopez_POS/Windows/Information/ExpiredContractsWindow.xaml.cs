using Lopez_POS.Static;
using System;
using System.Windows;

namespace Lopez_POS
{
    /// <summary>
    /// Interaction logic for ExpiredContractsWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class ExpiredContractsWindow : Window
    {
        /// <summary>
        /// Gets the total due.
        /// </summary>
        /// <value>
        /// The total due.
        /// </value>
        public decimal TotalDue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpiredContractsWindow"/> class.
        /// </summary>
        public ExpiredContractsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Loaded event of the DataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            TotalDue = 0;

            foreach (Person person in Storage.PeopleList)
            {
                foreach (PaymentCar car in person.Cars)
                {
                    if (car.GetContractExpirationDate() < DateTime.Today) //needs to be added to view
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