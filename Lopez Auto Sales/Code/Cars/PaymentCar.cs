using System;
using System.Collections.Generic;
using System.Linq;

namespace Lopez_Auto_Sales
{
    public class PaymentCar : Car
    {
        public int PersonID { get; private set; }
        public int CarID { get; private set; }
        public decimal Due { get; private set; }
        public decimal Expected { get; private set; }
        public DateTime BoughtDate { get; private set; }
        internal List<Payment> Payments = new List<Payment>();
        internal Payment Down { get { return Payments.Find(p => p.IsDownPayment == true); } }
        public string Person { get; private set; }

        public PaymentCar(string person, int year, string make, string model, int? mileage, string vin, decimal due, decimal expected, string color, DateTime boughtDate, List<Payment> payments, int personID, int carID) : base(year, make, model, color, vin, 0, mileage)
        {
            Person = person;
            Year = year;
            Make = make;
            Model = model;
            VIN = vin;
            Payments = payments;
            Mileage = mileage;
            Due = due;
            Color = color;
            BoughtDate = boughtDate;
            Expected = expected;
            PersonID = personID;
            CarID = carID;
        }

        public void AddPayment(DateTime date, decimal amount, bool down)
        {
            int paymentID = Storage.AddPayment(CarID, Person, this.ToString(), date, amount, false);
            Payments.Add(new Payment(paymentID, CarID, date, amount, down));
        }

        public void EditPayment(Payment payment, Payment newPayment, string reason)
        {
            Storage.EditPayment(payment, newPayment.Date, newPayment.Amount, reason);
            Payments[Payments.IndexOf(payment)] = newPayment;
        }

        public void RemovePayment(Payment payment, string reason)
        {
            Storage.RemovePayment(payment, reason);
            Payments.Remove(payment);
        }

        public decimal TotalPayments()
        {
            decimal total = 0;
            Payments.ForEach((Payment payment) =>
            {
                total += payment.Amount;
            });
            return total;
        }

        public decimal Balance
        {
            get { return Due - TotalPayments(); }
        }

        public int MonthsToPay()
        {
            return (int)Math.Ceiling((Due - Down.Amount) / Expected);
        }

        public DateTime ContractExpirationDate()
        {
            return Down.Date.AddMonths(MonthsToPay());
        }

        public int DaysSinceLastPayment()
        {
            return (DateTime.Today - Payments.Last().Date).Days;
        }

        public decimal DueToDate(DateTime date)
        {
            DateTime firstDate = Down.Date;
            decimal due = 0;

            while (date > firstDate)
            {
                due += Expected;
                firstDate = firstDate.Date.AddMonths(1);
            }

            if (due > Due)
                return Due;
            return due;
        }

        public decimal LateDue()
        {
            decimal lateDue = DueToDate(DateTime.Today) - TotalPayments();
            if (lateDue < 0)
                return 0;
            return lateDue;
        }

        public override string ToString()
        {
            return Year.ToString() + ' ' + Make + ' ' + Model;
        }
    }
}