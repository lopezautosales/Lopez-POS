using NLog;
using System;

namespace Lopez_Auto_Sales.Static
{
    /// <summary>
    /// Class for handling any logging actions.
    /// </summary>
    internal static class Logger
    {
        /// <summary>
        /// The sales logger
        /// </summary>
        private static readonly NLog.Logger SalesLogger = LogManager.GetLogger("salesLogger");

        /// <summary>
        /// The payments logger
        /// </summary>
        private static readonly NLog.Logger PaymentsLogger = LogManager.GetLogger("paymentsLogger");

        /// <summary>
        /// The errors logger
        /// </summary>
        private static readonly NLog.Logger ErrorsLogger = LogManager.GetLogger("errorsLogger");

        /// <summary>
        /// The inventory logger
        /// </summary>
        private static readonly NLog.Logger InventoryLogger = LogManager.GetLogger("inventoryLogger");

        /// <summary>
        /// The people logger
        /// </summary>
        private static readonly NLog.Logger PeopleLogger = LogManager.GetLogger("peopleLogger");

        /// <summary>
        /// Logging for sales
        /// </summary>
        internal static class Sales
        {
            /// <summary>
            /// Logs the sale.
            /// </summary>
            /// <param name="person">The person.</param>
            /// <param name="car">The car.</param>
            internal static void AddSale(string person, Car car)
            {
                SalesLogger.Info("{@sale}", new { person, Car = car.Name, car.Color, Price = car.Value, car.Mileage, car.VIN });
            }
        }

        /// <summary>
        /// Logging for payments
        /// </summary>
        internal static class Payments
        {
            /// <summary>
            /// Logs the add payment.
            /// </summary>
            /// <param name="person">The person.</param>
            /// <param name="car">The car.</param>
            /// <param name="amount">The amount.</param>
            /// <param name="down">if set to <c>true</c> [down].</param>
            internal static void AddPayment(string person, string car, decimal amount, bool down)
            {
                PaymentsLogger.Info("Add Payment: {payment} {info}", new { amount, down }, new { person, car });
            }

            /// <summary>
            /// Logs the remove payment.
            /// </summary>
            /// <param name="person">The person.</param>
            /// <param name="car">The car.</param>
            /// <param name="payment">The payment.</param>
            /// <param name="reason">The reason.</param>
            internal static void RemovePayment(string person, string car, Payment payment, string reason)
            {
                PaymentsLogger.Info("Remove Payment: {payment} {info}", new { payment.Date, payment.Amount }, new { person, car, reason });
            }

            /// <summary>
            /// Logs the edit payment.
            /// </summary>
            /// <param name="person">The person.</param>
            /// <param name="car">The car.</param>
            /// <param name="payment">The payment.</param>
            /// <param name="date">The date.</param>
            /// <param name="amount">The amount.</param>
            /// <param name="reason">The reason.</param>
            internal static void EditPayment(string person, string car, Payment payment, DateTime date, decimal amount, string reason)
            {
                PaymentsLogger.Info("Edit Payment: {payment} {payment2} {info}", new { payment.Date, payment.Amount }, new { date, amount }, new { person, car, reason });
            }
        }

        /// <summary>
        /// Logging for errors
        /// </summary>
        internal static class Errors
        {
            /// <summary>
            /// Logs the error.
            /// </summary>
            /// <param name="exception">The exception.</param>
            internal static void Error(Exception exception)
            {
                ErrorsLogger.Error(exception);
            }
        }

        /// <summary>
        /// Logging for inventory
        /// </summary>
        internal static class Inventory
        {
            /// <summary>
            /// Adds the inventory.
            /// </summary>
            /// <param name="isAdding">if set to <c>true</c> [is adding].</param>
            /// <param name="car">The car.</param>
            internal static void AddInventory(Car car)
            {
                InventoryLogger.Info("Added Vehicle: {car}", new { Car = car.Name, Price = car.Value, car.Mileage, car.Color });
            }

            /// <summary>
            /// Removes the inventory.
            /// </summary>
            /// <param name="car">The car.</param>
            internal static void RemoveInventory(Car car)
            {
                InventoryLogger.Info("Removed Vehicle: {car}", new { Car = car.Name, Price = car.Value, car.Mileage, car.Color });
            }
        }


        /// <summary>
        /// Logging for people
        /// </summary>
        internal static class People
        {
            /// <summary>
            /// Logs the add person.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="phone">The phone.</param>
            /// <param name="address">The address.</param>
            internal static void AddPerson(string name, string phone, string address)
            {
                PeopleLogger.Info("Add Person: {person}", new { name, phone, address });
            }

            /// <summary>
            /// Logs the update person.
            /// </summary>
            /// <param name="person">The person.</param>
            /// <param name="newPerson">The new person.</param>
            internal static void UpdatePerson(Person person, Person newPerson)
            {
                PeopleLogger.Info("Update Person: {person} {person2}", new { person.Name, person.Phone, Address = person.Full_Address }, new { newPerson.Name, newPerson.Phone, Address = newPerson.Full_Address });
            }
        }
    }
}