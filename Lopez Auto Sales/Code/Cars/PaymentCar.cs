using Lopez_Auto_Sales.Static;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// The Payment Car Class
    /// </summary>
    /// <seealso cref="Lopez_Auto_Sales.Car" />
    public class PaymentCar : Car
    {
        /// <summary>
        /// Gets the person identifier.
        /// </summary>
        /// <value>
        /// The person identifier.
        /// </value>
        public int PersonID { get; private set; }

        /// <summary>
        /// Gets the car identifier.
        /// </summary>
        /// <value>
        /// The car identifier.
        /// </value>
        public int CarID { get; private set; }

        /// <summary>
        /// Gets the due.
        /// </summary>
        /// <value>
        /// The due.
        /// </value>
        public decimal Due { get; private set; }

        /// <summary>
        /// Gets the expected.
        /// </summary>
        /// <value>
        /// The expected.
        /// </value>
        public decimal Expected { get; private set; }

        /// <summary>
        /// Gets the bought date.
        /// </summary>
        /// <value>
        /// The bought date.
        /// </value>
        public DateTime BoughtDate { get; private set; }

        /// <summary>
        /// The payments
        /// </summary>
        internal List<Payment> Payments = new List<Payment>();

        /// <summary>
        /// Gets down.
        /// </summary>
        /// <value>
        /// Down.
        /// </value>
        internal Payment Down { get { return Payments.Find(p => p.IsDownPayment == true); } }

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <value>
        /// The person.
        /// </value>
        public string Person { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentCar"/> class.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="year">The year.</param>
        /// <param name="make">The make.</param>
        /// <param name="model">The model.</param>
        /// <param name="mileage">The mileage.</param>
        /// <param name="vin">The vin.</param>
        /// <param name="due">The due.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="color">The color.</param>
        /// <param name="boughtDate">The bought date.</param>
        /// <param name="payments">The payments.</param>
        /// <param name="personID">The person identifier.</param>
        /// <param name="carID">The car identifier.</param>
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

        /// <summary>
        /// Adds the payment.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="down">if set to <c>true</c> [down].</param>
        public void AddPayment(DateTime date, decimal amount, bool down)
        {
            int paymentID = Storage.AddPayment(CarID, Person, this.Name, date, amount, false);
            Payments.Add(new Payment(paymentID, CarID, date, amount, down));
        }

        /// <summary>
        /// Edits the payment.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="newPayment">The new payment.</param>
        /// <param name="reason">The reason.</param>
        public void EditPayment(Payment payment, Payment newPayment, string reason)
        {
            Storage.EditPayment(Person, this.Name, payment, newPayment.Date, newPayment.Amount, reason);
            Payments[Payments.IndexOf(payment)] = newPayment;
        }

        /// <summary>
        /// Removes the payment.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="reason">The reason.</param>
        public void RemovePayment(Payment payment, string reason)
        {
            Storage.RemovePayment(Person, this.Name, payment, reason);
            Payments.Remove(payment);
        }

        /// <summary>
        /// Gets the total payments amount.
        /// </summary>
        /// <returns>The total payments amount.</returns>
        public decimal GetTotalPayments(bool withDown = true)
        {
            decimal total = 0;
            Payments.ForEach((Payment payment) =>
            {
                total += payment.Amount;
            });
            if (!withDown)
                total -= Down.Amount;
            return total;
        }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        public decimal Balance
        {
            get { return Due - GetTotalPayments(); }
        }

        /// <summary>
        /// Gets the amount of months needed to pay the vehicle off.
        /// </summary>
        /// <returns>The months need to pay vehicle.</returns>
        public int GetMonthsToPay()
        {
            return (int)Math.Ceiling((Due - Down.Amount) / Expected);
        }

        /// <summary>
        /// Gets the contract expiration date.
        /// </summary>
        /// <returns>The contract expiration date.</returns>
        public DateTime GetContractExpirationDate()
        {
            return Down.Date.AddMonths(GetMonthsToPay());
        }

        /// <summary>
        /// Gets the days since last payment.
        /// </summary>
        /// <returns>The amount of days since the last payment.</returns>
        public int GetDaysSinceLastPayment()
        {
            return (DateTime.Today - Payments.Last().Date).Days;
        }

        /// <summary>
        /// Gets the due to date amount.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The due to date amount.</returns>
        public decimal GetDueToDate(DateTime date)
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

        /// <summary>
        /// Gets the late due amount.
        /// </summary>
        /// <returns>The late due amount</returns>
        public decimal GetLateDue()
        {
            decimal lateDue = GetDueToDate(DateTime.Today) - GetTotalPayments(false);
            if (lateDue < 0)
                return 0;
            return lateDue;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}