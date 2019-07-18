using System;
using System.Windows;
using System.Windows.Documents;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for PaymentEdit.xaml
    /// </summary>
    public partial class PaymentEdit : Window
    {
        public Payment Payment { get; private set; }
        public string Reason { get; private set; }

        public PaymentEdit(Payment payment)
        {
            InitializeComponent();
            Payment = payment;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DateBox.SelectedDate = Payment.Date;
            AmountTextBox.Text = Payment.Amount.ToString("N2");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string text = new TextRange(ReasonBox.Document.ContentStart, ReasonBox.Document.ContentEnd).Text;
            if (String.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Provide a reason for editing.");
                return;
            }
            if (!DateTime.TryParse(DateBox.Text, out DateTime date))
            {
                MessageBox.Show("Enter a correct date.");
                return;
            }
            if (!Decimal.TryParse(AmountTextBox.Text, out decimal amount))
            {
                MessageBox.Show("Enter a correct payment amount.");
                return;
            }

            Reason = text;
            Payment = new Payment(Payment.PaymentID, Payment.CarID, date, amount, Payment.IsDownPayment);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}