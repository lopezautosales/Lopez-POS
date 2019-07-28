using Lopez_Auto_Sales.Static;
using System;
using System.Windows;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for EntrytWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class EntryWindow : Window
    {
        /// <summary>
        /// Gets the car.
        /// </summary>
        /// <value>
        /// The car.
        /// </value>
        public PaymentCar Car { get; private set; }
        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <value>
        /// The person.
        /// </value>
        public Person Person { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryWindow"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="paymentCar">The payment car.</param>
        public EntryWindow(Person owner, PaymentCar paymentCar)
        {
            InitializeComponent();
            Car = paymentCar;
            Person = owner;
        }

        /// <summary>
        /// Updates the information.
        /// </summary>
        private void UpdateInfo()
        {
            dueLabel.Text = "Total Due: " + Car.Due.ToString("C");
            balanceLabel.Text = "Balance: " + Car.Balance.ToString("C");
            expirationLabel.Text = "Contract Expiration Date: " + Car.GetContractExpirationDate().ToString("MM/dd/yyyy");
            daysLabel.Text = "Days From Last Payment: " + Car.GetDaysSinceLastPayment();
            lateLabel.Text = "Late Due: " + Car.GetLateDue().ToString("C");

            if (Car.Balance == 0)
                PaymentButton.Content = "Close Entry";
            else
                PaymentButton.Content = "+ Add Payment";
        }

        /// <summary>
        /// Handles the Loaded event of the EntryWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void EntryWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PaymentsGrid.ItemsSource = null;
            PaymentsGrid.ItemsSource = Car.Payments;
            UpdateInfo();
        }

        /// <summary>
        /// Handles the Click event of the PaymentButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PaymentButton_Click(object sender, RoutedEventArgs e)
        {
            if (Car.Balance == 0)
            {
                Storage.PaymentCars.RemovePaymentCar(Person, Car);
                if (Person.Cars.Count == 0)
                    Storage.People.RemovePerson(Person);
                if (Owner is MainWindow mainWindow)
                    mainWindow.Search_TextChanged(null, null);
                Close();
                MessageBox.Show("Account Closed.");
                return;
            }

            if (!decimal.TryParse(PaymentTextBox.Text, out decimal paymentAmount) || paymentAmount == 0)
            {
                MessageBox.Show("Enter a valid payment amount.", "Payment Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Car.Balance < paymentAmount)
            {
                MessageBox.Show("Cannot pay more than the car's balance.", "Payment Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Car.AddPayment(DateTime.Now, paymentAmount, false);
            UpdateInfo();

            PaymentsGrid.ItemsSource = null;
            PaymentsGrid.ItemsSource = Car.Payments;

            MSEdit.PrintReceipt(Person, Car);
            PaymentTextBox.Text = String.Empty;
        }

        /// <summary>
        /// Handles the Click event of the EditButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentsGrid.SelectedItem == null)
            {
                MessageBox.Show("Select a payment to edit.", "Edit Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Payment payment = PaymentsGrid.SelectedItem as Payment;
            PaymentEdit paymentEdit = new PaymentEdit(payment)
            {
                Owner = this,
                Topmost = true
            };
            if (paymentEdit.ShowDialog() == true)
            {
                if (paymentEdit.Payment.Amount == 0)
                {
                    if (payment.IsDownPayment)
                    {
                        MessageBox.Show("Cannot delete the down payment.", "Edit Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else
                        Car.RemovePayment(payment, paymentEdit.Reason);
                }
                else
                    Car.EditPayment(payment, paymentEdit.Payment, paymentEdit.Reason);

                PaymentsGrid.ItemsSource = null;
                PaymentsGrid.ItemsSource = Car.Payments;

                UpdateInfo();
            }
        }

        /// <summary>
        /// Handles the Click event of the ReceiptButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            MSEdit.PrintReceipt(Person, Car);
        }

        /// <summary>
        /// Handles the Click event of the PapersButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PapersButton_Click(object sender, RoutedEventArgs e)
        {
            ExtraPapers extraPapers = new ExtraPapers(Person.Name, Car.VIN);
            extraPapers.Show();
        }
    }
}