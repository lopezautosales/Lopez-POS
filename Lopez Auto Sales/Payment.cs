using System;

namespace Lopez_Auto_Sales
{
    public class Payment
    {
        public int CarID { get; private set; }
        public int PaymentID { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
        public bool Down { get; private set; }

        public Payment(int paymentID, int carID, DateTime date, decimal amount, bool down)
        {
            Date = date;
            Amount = amount;
            Down = down;
            CarID = carID;
            PaymentID = paymentID;
        }

        public override string ToString()
        {
            return Date.ToString("MM/dd/yyyy") + "\t\t"+ Amount.ToString("C");
        }
    }
}