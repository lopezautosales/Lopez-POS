using System;
using System.IO;
using System.Reflection;

namespace Lopez_Auto_Sales
{
    internal static class Logger
    {
        private const string LOG_PATH = "Log.txt";

        internal static void AddPerson(string name, string phone, string full_Address)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]: {1}\tName: {2}\tPhone: {3}\tAddress: {4}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(),
                name, phone, full_Address));
        }

        internal static void UpdatePerson(Person person, Person newPerson)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]: {1}\tOriginal: Name: {2}\tPhone: {3}\tAddress: {4}\tUpdated: Name: {5}\tPhone: {6}\tAddress: {7}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(),
                person.Name, person.Phone, person.Full_Address, newPerson.Name, newPerson.Phone, newPerson.Full_Address));
        }

        internal static void RemovePerson(Person person)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]: {1}\tName: {2}\tPhone: {3}\tAddress: {4}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(),
                person.Name, person.Phone, person.Full_Address));
        }

        internal static void EditSalesCar(SalesCar car, SalesCar newCar)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]: {1}\tOriginal: Name: {2}\tColor: {3}\tPrice: {4}\tUpdated: Name: {5}\tColor: {6}\tPrice: {7}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(),
                car.Name, car.Color, car.Price, newCar.Name, newCar.Color, newCar.Price));
        }

        internal static void RemoveSalesCar(SalesCar car)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]: {1}\tName: {2}\tColor: {3}\tPrice: {4}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(),
                car.Name, car.Color, car.Price));
        }

        internal static void AddSalesCar(SalesCar car)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]: {1}\tName: {2}\tColor: {3}\tPrice: {4}\tMileage: {5}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(),
                car.Name, car.Color, car.Price, car.Mileage));
        }

        internal static void RemovePayment(Payment payment, string reason)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]: {1}\tDate: {2}\tAmount: {3}\tReason: {4}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(),
                payment.Date, payment.Amount, reason));
        }

        internal static void EditPayment(Payment payment, DateTime date, decimal amount, string reason)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]: {1}\tOriginal: Date: {2}\tAmount: {3}\tUpdated: Date: {4}\tAmount: {5}\tReason: {6}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(),
                payment.Date, payment.Amount, date, amount, reason));
        }

        internal static void AddPayment(string person, string car, DateTime date, decimal amount)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]: {1}\tPerson: {2}\tCar: {3}\tAmount: {4}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(),
                person, car, amount));
        }

        internal static void AddPaymentCar(string person, int year, string make, string model, string VIN, decimal due, decimal average, DateTime now, string color)
        {
            string name = year.ToString() + ' ' + make + ' ' + model;
            File.AppendAllText(LOG_PATH, String.Format("[{0}]: {1}\tPerson: {2}\tName: {3}\tColor: {4}\tDue: {5}\tAverage: {6}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(),
                person, name, color, due, average));
        }

        internal static void RemovePaymentCar(Person person, PaymentCar car)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]: {1}\tPerson: {2}\tName: {3}\tColor: {4}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(),
                person.Name, car.Name, car.Color));
        }
    }
}