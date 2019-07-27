using System;
using System.Windows;
using System.Windows.Documents;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for PaymentEdit.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class PaymentEdit : Window
    {
        /// <summary>
        /// Gets the payment.
        /// </summary>
        /// <value>
        /// The payment.
        /// </value>
        public Payment Payment { get; private set; }
        /// <summary>
        /// Gets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public string Reason { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentEdit"/> class.
        /// </summary>
        /// <param name="payment">The payment.</param>
        public PaymentEdit(Payment payment)
        {
            InitializeComponent();
            Payment = payment;
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DateBox.SelectedDate = Payment.Date;
            AmountTextBox.Text = Payment.Amount.ToString("N2");
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}