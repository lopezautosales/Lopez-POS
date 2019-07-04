using System;

namespace Lopez_Auto_Sales
{
    public class Payment
    {
        public int CarID { get; private set; }
        public int PaymentID { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
        public bool IsDownPayment { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="Payment"/> class.</summary>
        /// <param name="paymentID">The payment identifier.</param>
        /// <param name="carID">The car identifier.</param>
        /// <param name="date">The date.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="isDownPayment">if set to <c>true</c> [down].</param>
        public Payment(int paymentID, int carID, DateTime date, decimal amount, bool isDownPayment)
        {
            Date = date;
            Amount = amount;
            IsDownPayment = isDownPayment;
            CarID = carID;
            PaymentID = paymentID;
        }

        public override string ToString()
        {
            return Date.ToString("MM/dd/yyyy") + "\t\t" + Amount.ToString("C");
        }
    }
}