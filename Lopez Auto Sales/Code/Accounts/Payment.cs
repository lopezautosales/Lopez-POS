using System;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// The payment class.
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Gets the car identifier.
        /// </summary>
        /// <value>
        /// The car identifier.
        /// </value>
        public int CarID { get; private set; }

        /// <summary>
        /// Gets the payment identifier.
        /// </summary>
        /// <value>
        /// The payment identifier.
        /// </value>
        public int PaymentID { get; private set; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is down payment.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is down payment; otherwise, <c>false</c>.
        /// </value>
        public bool IsDownPayment { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Payment" /> class.
        /// </summary>
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

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Date.ToString("MM/dd/yyyy") + "\t\t" + Amount.ToString("C");
        }
    }
}