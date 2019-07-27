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
        /// Logs the sale.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="car">The car.</param>
        internal static void LogSale(string person, Car car)
        {
            SalesLogger.Info("{@sale}", new { person, Car = car.Name, car.Color, Price = car.Value, car.Mileage, car.VIN });
        }

        /// <summary>
        /// Logs the add payment.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="car">The car.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="down">if set to <c>true</c> [down].</param>
        internal static void LogAddPayment(string person, string car, decimal amount, bool down)
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
        internal static void LogRemovePayment(string person, string car, Payment payment, string reason)
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
        internal static void LogEditPayment(string person, string car, Payment payment, DateTime date, decimal amount, string reason)
        {
            PaymentsLogger.Info("Edit Payment: {payment} {payment2} {info}", new { payment.Date, payment.Amount }, new { date, amount }, new { person, car, reason});
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        internal static void LogError(Exception exception)
        {
            ErrorsLogger.Error(exception);
        }

        /// <summary>
        /// Logs the inventory.
        /// </summary>
        /// <param name="isAdding">if set to <c>true</c> [is adding].</param>
        /// <param name="car">The car.</param>
        internal static void LogInventory(bool isAdding, Car car)
        {
            if (isAdding)
                InventoryLogger.Info("Added Vehicle: {car}", new { Car = car.Name, Price = car.Value, car.Mileage, car.Color });
            else
                InventoryLogger.Info("Removed Vehicle: {car}", new { Car = car.Name, Price = car.Value, car.Mileage, car.Color });
        }
    }
}