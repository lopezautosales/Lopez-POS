using Lopez_Auto_Sales.Cars;
using Lopez_Auto_Sales.LopezDataDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
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
        static PaidCarTableTableAdapter paidCarAdapter = new PaidCarTableTableAdapter();
        internal static List<Person> People = new List<Person>();
        internal static List<SalesCar> SalesCars = new List<SalesCar>();
        internal static List<PaperInfo> Papers = new List<PaperInfo>();
        public const string CONNECTION = "Data Source = (LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\LopezData.mdf;Integrated Security = True; Connect Timeout = 30; MultipleActiveResultSets=True ";
        const string LOG_PATH = "Log.txt";

        internal static void Init()
        {
            if (File.Exists("Entries.xml"))
            {
                AddEntriesToDatabase("Entries.xml");
                File.Move("Entries.xml", "Old.xml");
            }
            if (File.Exists("Cars.xml"))
            {
                AddCarsToDatabase("Cars.xml");
                File.Move("Cars.xml", "OldCars.xml");
            }

            peopleAdapter.Fill(lopezData.PeopleTable);
            paymentCarAdapter.Fill(lopezData.PaymentCarTable);
            paymentAdapter.Fill(lopezData.PaymentTable);
            salesCarAdapter.Fill(lopezData.SalesCarTable);
            paperInfoAdapter.Fill(lopezData.PaperInfoTable);

            People.Populate();
            SalesCars.Populate();
            Papers.Populate();
        }

        #region POPULATE
        private static void Populate(this List<SalesCar> cars)
        {
            foreach (DataRow car in lopezData.SalesCarTable)
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
            }
        }

        private static void Populate(this List<Person> people)
        {
            foreach (DataRow person in lopezData.PeopleTable)
            {
                string name = (string)person["Name"];
                string phone = (string)person["Phone"];
                string address = (string)person["Address"];
                int personId = (int)person["PersonId"];

                List<PaymentCar> cars = new List<PaymentCar>();

                foreach (DataRow car in lopezData.PaymentCarTable.Select("PersonId=" + personId))
                {
                    int year = (int)car["Year"];
                    string make = (string)car["Make"];
                    string model = (string)car["Model"];
                    string vin = (string)car["VIN"];
                    decimal due = (decimal)car["Due"];
                    decimal expected = (decimal)car["Expected"];
                    string color = (string)car["Color"];
                    DateTime boughtdate = (DateTime)car["BoughtDate"];
                    int carId = (int)car["CarId"];
                    List<Payment> payments = new List<Payment>();

                    foreach (DataRow payment in lopezData.PaymentTable.Select("CarId=" + carId))
                    {
                        DateTime date = (DateTime)payment["Date"];
                        decimal amount = (decimal)payment["Amount"];
                        bool down = (bool)payment["Down"];
                        int paymentId = (int)payment["PaymentId"];

                        payments.Add(new Payment(date, amount, down, carId, paymentId));
                    }

                    cars.Add(new PaymentCar(year, make, model, vin, due, expected, color, boughtdate, payments, personId, carId));
                }
                people.Add(new Person(name, phone, address, cars, personId));
            }
        }

        private static void Populate(this List<PaperInfo> papers)
        {
            foreach (DataRow row in lopezData.PaperInfoTable)
            {
                Car trade = null;
                if (!(row["TradeVIN"] is DBNull))
                {
                    int? trademileage = row["TradeMileage"] is DBNull ? null : (int?)row["TradeMileage"];
                    trade = new Car((int)row["TradeYear"], (string)row["TradeMake"], (string)row["TradeModel"], (string)row["TradeColor"], (string)row["TradeVIN"], (decimal)row["TradeValue"], trademileage);
                }
                int? mileage = row["Mileage"] is DBNull ? null : (int?)row["Mileage"];
                Person person = new Person((string)row["Buyer"], (string)row["Phone"], (string)row["Address"], null, 0);
                Car car = new Car((int)row["Year"], (string)row["Make"], (string)row["Model"], (string)row["Color"], (string)row["VIN"], (decimal)row["SellingPrice"], mileage);
                papers.Add(new PaperInfo((DateTime)row["Date"], person, (string)row["CoBuyer"], car, trade, (decimal)row["Down"], (bool)row["Tag"], (bool)row["Lien"], (bool)row["OutOfState"], (decimal)row["Tax"], (int)row["Warranty"], (decimal)row["AveragePayment"]));
            }
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
        #region CONVERT_OLD_STORAGE
        private static void AddCarsToDatabase(string Path)
        {
            XDocument xdoc = XDocument.Load(Path);

            foreach (XElement car in xdoc.Root.Elements())
            {
                int? mileage = null;
                if (car.Element("Miles").Value != "Exempt")
                    mileage = int.Parse(car.Element("Miles").Value);

                salesCarAdapter.Insert(car.Attribute("VIN").Value, int.Parse(car.Element("Year").Value), car.Element("Make").Value, car.Element("Model").Value, car.Element("Color").Value, mileage, decimal.Parse(car.Element("Price").Value), 0m, DateTime.Now, decimal.Parse(car.Element("Bought").Value), false, false);
            }
        }

        private static void AddEntriesToDatabase(string Path)
        {
            XDocument xdoc = XDocument.Load(Path);

            foreach (XElement person in xdoc.Root.Elements())
            {
                bool InUse = false;
                foreach (XElement entry in person.Element("Entries").Elements())
                {
                    if (entry.Name != "Entry")
                        continue;
                    if (Decimal.Parse(entry.Element("Balance").Value) == 0)
                        continue;
                    InUse = true;
                }
                if (!InUse)
                    continue;
                int personID = (int)peopleAdapter.InsertQuery(person.Attribute("Name").Value, person.Element("Address").Value + "," + person.Element("Address").Attribute("City").Value + "," + person.Element("Address").Attribute("State").Value + "," + person.Element("Address").Attribute("ZIP").Value, person.Attribute("Phone").Value);
                foreach (XElement entry in person.Element("Entries").Elements())
                {
                    if (entry.Name != "Entry")
                        continue;
                    if (Decimal.Parse(entry.Element("Balance").Value) == 0)
                        continue;
                    string make = entry.Attribute("Name").Value.Split(' ')[1];

                    int carID = (int)paymentCarAdapter.InsertQuery(personID, int.Parse(entry.Attribute("Name").Value.Split(' ')[0]), make, entry.Attribute("Name").Value.Substring(entry.Attribute("Name").Value.IndexOf(make) + make.Length + 1),
                        entry.Attribute("VIN").Value, decimal.Parse(entry.Attribute("Due").Value), entry.Attribute("Color").Value, decimal.Parse(entry.Element("Expected").Value), DateTime.Parse(entry.Element("Payments").Element("Down").Attribute("Date").Value));

                    foreach (XElement payment in entry.Element("Payments").Elements())
                    {
                        paymentAdapter.Insert(carID, DateTime.Parse(payment.Attribute("Date").Value), decimal.Parse(payment.Attribute("Amount").Value), payment.Name == "Down");
                    }
                }
            }
        }

        #endregion
        #region PERSON
        internal static int AddPerson(string name, string phone, string full_Address)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]:\t{1}\t{2}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(), name));
            return (int)peopleAdapter.InsertQuery(name, full_Address, phone);
        }
        internal static void UpdatePerson(Person person, Person newPerson)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]:\t{1}\t{2}\t{3}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(), person, newPerson));
            People[People.IndexOf(person)] = newPerson;
            peopleAdapter.Update(newPerson.Name, newPerson.Full_Address, newPerson.Phone, person.PersonID, person.Name, person.Full_Address, person.Phone);
        }
        internal static void RemovePerson(Person person)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]:\t{1}\t{2}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(), person.Name));
            People.Remove(person);
            peopleAdapter.Delete(person.PersonID, person.Name, person.Full_Address, person.Phone);
        }
        #endregion
        #region SALESCARS
        internal static void EditSalesCar(SalesCar car, SalesCar newcar)
        {
            salesCarAdapter.Update(newcar.VIN, newcar.Year, newcar.Make, newcar.Model, newcar.Color, newcar.Mileage, newcar.Price,
                newcar.LowestPrice, newcar.ListDate, newcar.BoughtPrice, newcar.ExtraKey, newcar.Salvage,
                car.VIN, car.Year, car.Make, car.Model, car.Color, car.Mileage, car.Price,
                car.LowestPrice, car.ListDate, car.BoughtPrice, car.ExtraKey, car.Salvage);
            SalesCars[SalesCars.IndexOf(car)] = newcar;
            File.AppendAllText(LOG_PATH, String.Format("[{0}]:\t{1}\t{2}\t{3}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(), car, newcar));
        }

        internal static void RemoveSalesCar(SalesCar car)
        {
            salesCarAdapter.Delete(car.VIN, car.Year, car.Make, car.Model, car.Color, car.Mileage, car.Price, car.LowestPrice, car.ListDate, car.BoughtPrice, car.ExtraKey, car.Salvage);
            SalesCars.Remove(car);
            File.AppendAllText(LOG_PATH, String.Format("[{0}]:\t{1}\t{2}\t{3}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(), car.VIN, car));
        }

        internal static void AddSalesCar(SalesCar car)
        {
            salesCarAdapter.Insert(car.VIN, car.Year, car.Make, car.Model, car.Color, car.Mileage, car.Price, car.LowestPrice, car.ListDate, car.BoughtPrice, car.ExtraKey, car.Salvage);
            SalesCars.Add(car);
            File.AppendAllText(LOG_PATH, String.Format("[{0}]:\t{1}\t{2}\t{3}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(), car.VIN, car));
        }
        #endregion
        #region PAYMENTS
        internal static void RemovePayment(Payment payment, string reason)
        {
            paymentAdapter.Delete(payment.PaymentID, payment.CarID, payment.Date, payment.Amount, payment.Down);
            removedPaymentAdapter.Insert(payment.CarID, payment.Date, payment.Amount, payment.Down, reason);
            File.AppendAllText(LOG_PATH, String.Format("[{0}]:\t{1}\t{2}\t{3}", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(), payment, reason));
        }

        internal static void EditPayment(Payment payment, DateTime date, decimal amount, string reason)
        {
            paymentAdapter.Update(payment.CarID, date, amount, payment.Down, payment.PaymentID, payment.CarID, payment.Date, payment.Amount, payment.Down);
            File.AppendAllText(LOG_PATH, String.Format("[{0}]:\t{1}\t{2}\t{3}", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(), payment, reason));
        }

        internal static int AddPayment(int carId, DateTime date, decimal amount, bool down)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]:\t{1}\t{2}\t{3}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(), carId, amount.ToString("C")));
            return (int)paymentAdapter.InsertQuery(carId, date, amount, down);
        }
        #endregion
        #region PAYMENTCAR
        internal static int AddPaymentCar(int year, string make, string model, string VIN, decimal due, decimal average, DateTime now, string color, int personID)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]:\t{1}\t{2}\t{3}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(), VIN, personID));
            return (int)paymentCarAdapter.InsertQuery(personID, year, make, model, VIN, due, color, average, now);
        }

        internal static void RemovePaymentCar(Person person, PaymentCar car)
        {
            File.AppendAllText(LOG_PATH, String.Format("[{0}]:\t{1}\t{2}\t{3}\r\n", DateTime.Now, MethodBase.GetCurrentMethod().Name.ToUpper(), person, car));
            People[People.IndexOf(person)].Cars.Remove(car);
            foreach(Payment p in car.Payments)
            {
                paymentAdapter.Delete(p.PaymentID, car.CarID, p.Date, p.Amount, p.Down);
            }
            paymentCarAdapter.Delete(car.CarID, car.PersonID, car.Year, car.Make, car.Model, car.VIN, car.Due, car.Color, car.Expected, car.BoughtDate);
            paidCarAdapter.Insert(car.PersonID, car.Year, car.Make, car.Model, car.VIN, car.Due, car.Color, car.Expected, car.BoughtDate);
        }
        #endregion
    }
}
