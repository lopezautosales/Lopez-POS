using Lopez_Auto_Sales.Cars;
using Lopez_Auto_Sales.LopezDataDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lopez_Auto_Sales
{
    static class Storage
    {
        internal static LopezDataDataSet lopezData = new LopezDataDataSet();
        static PeopleTableTableAdapter peopleAdapter = new PeopleTableTableAdapter();
        static PaymentCarTableTableAdapter paymentCarAdapter = new PaymentCarTableTableAdapter();
        static SalesCarTableTableAdapter salesCarAdapter = new SalesCarTableTableAdapter();
        static PaymentTableTableAdapter paymentAdapter = new PaymentTableTableAdapter();
        static PaperInfoTableTableAdapter paperInfoAdapter = new PaperInfoTableTableAdapter();
        static RemovedPaymentTableTableAdapter removedPaymentAdapter = new RemovedPaymentTableTableAdapter();
        static SalesTableTableAdapter salesAdapter = new SalesTableTableAdapter();
        internal static List<Person> People = new List<Person>();
        internal static List<SalesCar> SalesCars = new List<SalesCar>();
        internal static List<PaperInfo> Papers = new List<PaperInfo>();
        public const string CONNECTION = "Data Source = (LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\LopezData.mdf;Integrated Security = True; Connect Timeout = 30; MultipleActiveResultSets=True ";
        private static bool isPapersLoaded = false;

        internal static void Init()
        {
            peopleAdapter.Fill(lopezData.PeopleTable);
            paymentCarAdapter.Fill(lopezData.PaymentCarTable);
            paymentAdapter.Fill(lopezData.PaymentTable);
            salesCarAdapter.Fill(lopezData.SalesCarTable);

            People.Populate();
            SalesCars.Populate();
        }

        internal static void LoadPapers()
        {
            if (!isPapersLoaded)
            {
                paperInfoAdapter.Fill(lopezData.PaperInfoTable);
                Papers.Populate();
                isPapersLoaded = true;
            }
        }

        #region POPULATE
        private static void Populate(this List<SalesCar> cars)
        {
            Parallel.ForEach(lopezData.SalesCarTable, (DataRow car) =>
            {
                int year = (int)car["Year"];
                string make = (string)car["Make"];
                string model = (string)car["Model"];
                string vin = (string)car["VIN"];
                int? mileage = car["Mileage"] is DBNull ? null : (int?)car["Mileage"];
                decimal price = (decimal)car["Price"];
                decimal lowestPrice = (decimal)car["LowestPrice"];
                DateTime date = (DateTime)car["ListDate"];
                string color = (string)car["Color"];
                decimal boughtPrice = (decimal)car["BoughtPrice"];
                bool salvage = (bool)car["Salvage"];
                bool extraKey = (bool)car["ExtraKey"];

                cars.Add(new SalesCar(year, make, model, vin, mileage, price, lowestPrice, date, color, boughtPrice, salvage, extraKey));
            });
        }

        private static void Populate(this List<Person> people)
        {
            Parallel.ForEach(lopezData.PeopleTable, (DataRow person) =>
            {
                string name = (string)person["Name"];
                string phone = (string)person["Phone"];
                string address = (string)person["Address"];
                int personId = (int)person["PersonId"];

                List<PaymentCar> cars = new List<PaymentCar>();

                Parallel.ForEach(lopezData.PaymentCarTable.Select("PersonId=" + personId), (DataRow car) =>
                {
                    int year = (int)car["Year"];
                    string make = (string)car["Make"];
                    string model = (string)car["Model"];
                    string vin = (string)car["VIN"];
                    decimal due = (decimal)car["Due"];
                    decimal expected = (decimal)car["Expected"];
                    int? mileage = car["Mileage"] is DBNull ? null : (int?)car["Mileage"];
                    string color = (string)car["Color"];
                    DateTime boughtdate = (DateTime)car["BoughtDate"];
                    int carID = (int)car["CarId"];
                    List<Payment> payments = new List<Payment>();

                    foreach (DataRow payment in lopezData.PaymentTable.Select("CarId=" + carID))
                    {
                        DateTime date = (DateTime)payment["Date"];
                        decimal amount = (decimal)payment["Amount"];
                        bool down = (bool)payment["Down"];
                        int paymentId = (int)payment["PaymentId"];

                        payments.Add(new Payment(paymentId, carID, date, amount, down));
                    }

                    cars.Add(new PaymentCar(name, year, make, model, mileage, vin, due, expected, color, boughtdate, payments, personId, carID));
                });
                people.Add(new Person(personId, name, phone, address, cars));
            });
        }

        private static void Populate(this List<PaperInfo> papers)
        {
            Parallel.ForEach(lopezData.PaperInfoTable, (DataRow row) =>
            {
                Car trade = null;
                if (!(row["TradeVIN"] is DBNull))
                {
                    int? trademileage = row["TradeMileage"] is DBNull ? null : (int?)row["TradeMileage"];
                    trade = new Car((int)row["TradeYear"], (string)row["TradeMake"], (string)row["TradeModel"], (string)row["TradeColor"], (string)row["TradeVIN"], (decimal)row["TradeValue"], trademileage);
                }
                int? mileage = row["Mileage"] is DBNull ? null : (int?)row["Mileage"];
                Person person = new Person(0, (string)row["Buyer"], (string)row["Phone"], (string)row["Address"], null);
                Car car = new Car((int)row["Year"], (string)row["Make"], (string)row["Model"], (string)row["Color"], (string)row["VIN"], (decimal)row["SellingPrice"], mileage);
                papers.Add(new PaperInfo((DateTime)row["Date"], person, (string)row["CoBuyer"], car, trade, (decimal)row["Down"], (bool)row["Tag"], (bool)row["Lien"], (bool)row["OutOfState"], (decimal)row["Tax"], (int)row["Warranty"], (decimal)row["AveragePayment"]));
            });
        }
        #endregion
        #region PAPERINFO
        internal static void AddPaperInfo(PaperInfo paperInfo)
        {
            if (paperInfo.Trade == null)
            {
                paperInfoAdapter.Insert(paperInfo.Date, paperInfo.Buyer.Name, paperInfo.CoBuyer, paperInfo.Buyer.Full_Address, paperInfo.Buyer.Phone, paperInfo.Car.VIN, paperInfo.Car.Year,
                    paperInfo.Car.Make, paperInfo.Car.Model, paperInfo.Car.Color, paperInfo.Car.Mileage, null, null, null, null, null, null, null, paperInfo.Car.Value, paperInfo.Down,
                    paperInfo.Tag, paperInfo.Lien, paperInfo.OutOfState, paperInfo.Tax, paperInfo.Warranty, paperInfo.AveragePayment);
            }
            else
            {
                paperInfoAdapter.Insert(paperInfo.Date, paperInfo.Buyer.Name, paperInfo.CoBuyer, paperInfo.Buyer.Full_Address, paperInfo.Buyer.Phone, paperInfo.Car.VIN, paperInfo.Car.Year,
                    paperInfo.Car.Make, paperInfo.Car.Model, paperInfo.Car.Color, paperInfo.Car.Mileage, paperInfo.Trade.VIN, paperInfo.Trade.Year,
                    paperInfo.Trade.Make, paperInfo.Trade.Model, paperInfo.Trade.Color, paperInfo.Trade.Mileage, paperInfo.Trade.Value, paperInfo.Car.Value, paperInfo.Down,
                    paperInfo.Tag, paperInfo.Lien, paperInfo.OutOfState, paperInfo.Tax, paperInfo.Warranty, paperInfo.AveragePayment);
            }
        }
        #endregion
        #region PERSON
        internal static int AddPerson(string name, string phone, string full_Address)
        {
            Logger.AddPerson(name, phone, full_Address);
            return peopleAdapter.Insert(name, full_Address, phone);
        }
        internal static void UpdatePerson(Person person, Person newPerson)
        {
            Logger.UpdatePerson(person, newPerson);
            People[People.IndexOf(person)] = newPerson;
            peopleAdapter.Update(newPerson.Name, newPerson.Full_Address, newPerson.Phone, person.PersonID, person.Name, person.Full_Address, person.Phone);
        }
        internal static void RemovePerson(Person person)
        {
            Logger.RemovePerson(person);
            People.Remove(person);
            peopleAdapter.Delete(person.PersonID, person.Name, person.Full_Address, person.Phone);
        }
        #endregion
        #region SALESCARS
        internal static void EditSalesCar(SalesCar car, SalesCar newCar)
        {
            Logger.EditSalesCar(car, newCar);
            salesCarAdapter.Update(newCar.VIN, newCar.Year, newCar.Make, newCar.Model, newCar.Color, newCar.Mileage, newCar.Price,
                newCar.LowestPrice, newCar.ListDate, newCar.BoughtPrice, newCar.ExtraKey, newCar.Salvage,
                car.VIN, car.Year, car.Make, car.Model, car.Color, car.Mileage, car.Price,
                car.LowestPrice, car.ListDate, car.BoughtPrice, car.ExtraKey, car.Salvage);
            SalesCars[SalesCars.IndexOf(car)] = newCar;
        }

        internal static void RemoveSalesCar(SalesCar car)
        {
            Logger.RemoveSalesCar(car);
            salesCarAdapter.Delete(car.VIN, car.Year, car.Make, car.Model, car.Color, car.Mileage, car.Price, car.LowestPrice, car.ListDate, car.BoughtPrice, car.ExtraKey, car.Salvage);
            SalesCars.Remove(car);
        }

        internal static void AddSalesCar(SalesCar car)
        {
            Logger.AddSalesCar(car);
            salesCarAdapter.Insert(car.VIN, car.Year, car.Make, car.Model, car.Color, car.Mileage, car.Price, car.LowestPrice, car.ListDate, car.BoughtPrice, car.ExtraKey, car.Salvage);
            SalesCars.Add(car);
        }
        #endregion
        #region PAYMENTS
        internal static void RemovePayment(Payment payment, string reason)
        {
            Logger.RemovePayment(payment, reason);
            paymentAdapter.Delete(payment.PaymentID, payment.CarID, payment.Date, payment.Amount, payment.Down);
            removedPaymentAdapter.Insert(payment.CarID, payment.Date, payment.Amount, payment.Down, reason);
        }

        internal static void EditPayment(Payment payment, DateTime date, decimal amount, string reason)
        {
            Logger.EditPayment(payment, date, amount, reason);
            paymentAdapter.Update(payment.CarID, date, amount, payment.Down, payment.PaymentID, payment.CarID, payment.Date, payment.Amount, payment.Down);
        }

        internal static int AddPayment(int carID, string person, string car, DateTime date, decimal amount, bool down)
        {
            Logger.AddPayment(person, car, date, amount);
            return paymentAdapter.Insert(carID, date, amount, down);
        }
        #endregion
        #region PAYMENTCAR
        internal static int AddPaymentCar(int personID, string person, int year, string make, string model, int? mileage, string VIN, decimal due, decimal average, DateTime now, string color)
        {
            Logger.AddPaymentCar(person, year, make, model, VIN, due, average, now, color);
            return paymentCarAdapter.Insert(personID, year, make, model, VIN, due, color, average, now, mileage);
        }

        internal static void RemovePaymentCar(Person person, PaymentCar car)
        {
            Logger.RemovePaymentCar(person, car);
            People[People.IndexOf(person)].Cars.Remove(car);
            foreach(Payment p in car.Payments)
            {
                paymentAdapter.Delete(p.PaymentID, car.CarID, p.Date, p.Amount, p.Down);
            }
            paymentCarAdapter.Delete(car.CarID, car.PersonID, car.Year, car.Make, car.Model, car.VIN, car.Due, car.Color, car.Expected, car.BoughtDate, car.Mileage);
        }
        #endregion
        #region Sales
        internal static void AddSales(string name, string vin, int year, string make, string model, string color, int? mileage, decimal sellingPrice, decimal boughtPrice, DateTime listDate, DateTime boughtDate, bool salvage)
        {
            salesAdapter.Insert(name, vin, year, make, model, color, mileage, sellingPrice, boughtPrice, listDate, boughtDate, salvage);
        }
        #endregion  
    }
}
