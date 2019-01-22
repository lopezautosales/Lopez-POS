using Lopez_Auto_Sales.Cars;
using System;
using System.Windows;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for EntrytWindow.xaml
    /// </summary>
    public partial class EntryWindow : Window
    {
        public PaymentCar Car { get; private set; }
        public Person Person { get; private set; }
        public EntryWindow(Person owner, PaymentCar paymentCar)
        {
            InitializeComponent();
            Car = paymentCar;
            Person = owner;
        }

        private void UpdateInfo()
        {
            dueLabel.Text = "Total Due: " + Car.Due.ToString("C");
            balanceLabel.Text = "Balance: " + Car.GetBalance().ToString("C");
            expirationLabel.Text = "Contract Expiration Date: " + Car.ContractExpirationDate().ToString("MM/dd/yyyy");
            daysLabel.Text = "Days From Last Payment: " + Car.DaysSinceLastPayment();
            lateLabel.Text = "Late Due: " + Car.LateDue().ToString("C");

            if (Car.GetBalance() == 0)
                PaymentButton.Content = "Close Entry";
            else
                PaymentButton.Content = "+ Add Payment";
        }

        private void EntryWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PaymentsGrid.ItemsSource = null;
            PaymentsGrid.ItemsSource = Car.Payments;
            UpdateInfo();
        }

        private void PaymentButton_Click(object sender, RoutedEventArgs e)
        {
            if(Car.GetBalance() == 0)
            {
                Storage.RemovePaymentCar(Person, Car);
                if (Person.Cars.Count == 0)
                    Storage.RemovePerson(Person);
                (Owner as MainWindow).Search_TextChanged(null, null);
                Close();
                MessageBox.Show("Account Closed.");
                return;
            }

            if (!decimal.TryParse(PaymentTextBox.Text, out decimal paymentAmount) || paymentAmount == 0)
            {
                MessageBox.Show("Enter a valid payment amount.", "Payment Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if(Car.GetBalance() < paymentAmount)
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

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if(PaymentsGrid.SelectedItem == null)
            {
                MessageBox.Show("Select a payment to edit.", "Edit Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Payment payment = PaymentsGrid.SelectedItem as Payment;
            PaymentEdit paymentEdit = new PaymentEdit(payment)
            {
                Owner= this,Topmost = true
            };
            if (paymentEdit.ShowDialog() == true)
            {
                if (paymentEdit.Payment.Amount == 0)
                {
                    if (payment.Down)
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

        private void ReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            MSEdit.PrintReceipt(Person, Car);
        }

        private void PapersButton_Click(object sender, RoutedEventArgs e)
        {
            ExtraPapers extraPapers = new ExtraPapers
            {
                Owner = this,Topmost = true
            };
            if (extraPapers.ShowDialog() == true)
            {
                if(!Storage.Papers.Exists(paper=> paper.Buyer.Name == Person.Name && paper.Car.VIN == Car.VIN))
                {
                    MessageBox.Show("Could not find paper info.", "Paper Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                PaperInfo paperInfo = Storage.Papers.Find(paper => paper.Buyer.Name == Person.Name && paper.Car.VIN == Car.VIN);

                if (extraPapers.papers.Contains("Contract"))
                {
                    MSEdit.PrintContract(paperInfo);
                }
                if (extraPapers.papers.Contains("Warranty"))
                {
                    MSEdit.PrintWarranty(paperInfo);
                }
                if (extraPapers.papers.Contains("Transfer Agreement"))
                {
                    MSEdit.PrintTransferAgreement(paperInfo);
                }
                if (extraPapers.papers.Contains("Legal"))
                {
                    MSEdit.PrintLegal(paperInfo);
                }
                if (extraPapers.papers.Contains("Lien Release"))
                {
                    MSEdit.PrintLien(paperInfo);
                }
            }
        }
    }
}
