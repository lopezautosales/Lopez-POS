using System;
using System.Globalization;
using System.IO;
using System.Windows;
using excel = Microsoft.Office.Interop.Excel;
using word = Microsoft.Office.Interop.Word;

namespace Lopez_Auto_Sales
{
    internal static class MSEdit
    {
        public struct DealerInfo
        {
            public const string DealerName = "Lopez Auto Sales";
            public const string Dealer = "Gabriel Lopez";
            public const int DealerNumber = 2518;
        }

        internal static void PrintContract(PaperInfo paperInfo)
        {
            string docPath = "Documents\\Contract.docx";
            if (!File.Exists(docPath))
            {
                MessageBox.Show(docPath + " file does not exist.");
                return;
            }

            word.Application application = new word.Application();
            word.Document document = application.Documents.Open(Path.Combine(Environment.CurrentDirectory, docPath));
            document.Activate();

            decimal difference = paperInfo.Car.Value - (paperInfo.Trade == null ? 0 : paperInfo.Trade.Value);
            decimal tax = paperInfo.OutOfState ? 0 : difference * paperInfo.Tax;
            decimal lien = paperInfo.Lien ? Constants.LIEN_COST : 0;
            decimal tag = paperInfo.Tag ? Constants.TAG_COST : 0;
            decimal total = difference + tax + lien + tag;
            decimal due = total - paperInfo.Down;

            foreach (word.Shape shape in document.Shapes)
            {
                if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoTextBox)
                    switch (shape.TextFrame.ContainingRange.Text.Replace("\r", ""))
                    {
                        #region Date

                        case "[NextDate]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Date.AddMonths(1).ToString("MM/dd/yyyy");
                            break;

                        case "[Date]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Date.ToString("MM/dd/yyyy");
                            break;

                        case "[Day]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Date.Day.ToString();
                            break;

                        case "[Month2]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Date.ToString("MMM", CultureInfo.InvariantCulture);
                            break;

                        case "[Yr]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Date.Year.ToString();
                            break;

                        #endregion Date

                        #region Person

                        case "[Buyer]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.Name;
                            break;

                        case "[Co-Buyer]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.CoBuyer;
                            break;

                        case "[Address]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.Address;
                            break;

                        case "[City-State-Zip]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.City + '\t' + paperInfo.Buyer.State + '\t' + paperInfo.Buyer.ZIP;
                            break;

                        case "[Phone]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.Phone;
                            break;

                        #endregion Person

                        #region Car

                        case "[Year]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Year.ToString();
                            break;

                        case "[Make]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Make;
                            break;

                        case "[Model]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Model;
                            break;

                        case "[Color]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Color;
                            break;

                        case "[Mileage]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Mileage == null ? "Exempt" : paperInfo.Car.Mileage.Value.ToString();
                            break;

                        case "[VIN]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.VIN;
                            break;

                        #endregion Car

                        #region Trade-in

                        case "[Year2]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Trade == null ? String.Empty : paperInfo.Trade.Year.ToString();
                            break;

                        case "[Make2]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Trade == null ? String.Empty : paperInfo.Trade.Make;
                            break;

                        case "[Model2]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Trade == null ? String.Empty : paperInfo.Trade.Model;
                            break;

                        case "[Color2]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Trade == null ? String.Empty : paperInfo.Trade.Color;
                            break;

                        case "[Mileage2]":
                            if (paperInfo.Trade == null)
                                shape.TextFrame.ContainingRange.Text = String.Empty;
                            else
                                shape.TextFrame.ContainingRange.Text = paperInfo.Trade.Mileage == null ? "Exempt" : paperInfo.Trade.Mileage.ToString();
                            break;

                        case "[VIN2]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Trade == null ? String.Empty : paperInfo.Trade.VIN;
                            break;

                        #endregion Trade-in

                        #region Money

                        case "[Price]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Value.ToString("N2");
                            break;

                        case "[Trade]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Trade == null ? String.Empty : paperInfo.Trade.Value.ToString("N2");
                            break;

                        case "[Difference]":
                            shape.TextFrame.ContainingRange.Text = difference.ToString("N2");
                            break;

                        case "[Tax]":
                            shape.TextFrame.ContainingRange.Text = tax == 0 ? String.Empty : tax.ToString("N2");
                            break;

                        case "[Tag]":
                            shape.TextFrame.ContainingRange.Text = tag == 0 ? String.Empty : tag.ToString("N2");
                            break;

                        case "[Lien]":
                            shape.TextFrame.ContainingRange.Text = lien == 0 ? String.Empty : lien.ToString("N2");
                            break;

                        case "[Total]":
                            shape.TextFrame.ContainingRange.Text = total.ToString("N2");
                            break;

                        case "[Down]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Down.ToString("N2");
                            break;

                        case "[Due]":
                            shape.TextFrame.ContainingRange.Text = due.ToString("N2");
                            break;

                        #endregion Money

                        #region Disclosure

                        case "[P_Amount]":
                            shape.TextFrame.ContainingRange.Text = due == 0 ? String.Empty : Math.Ceiling(total / paperInfo.AveragePayment).ToString();
                            break;

                        case "[Payment]":
                            shape.TextFrame.ContainingRange.Text = due == 0 ? String.Empty : paperInfo.AveragePayment.ToString("N2");
                            break;

                        case "[Due_Date]":
                            shape.TextFrame.ContainingRange.Text = due == 0 ? String.Empty : "Between the 25th and 30th of every month";
                            break;

                        #endregion Disclosure

                        default:
                            break;
                    }
            }
            document.PrintOut(true, ManualDuplexPrint: false);
            document.Close(false);
            application.Quit();
        }

        internal static void PrintWarranty(PaperInfo paperInfo)
        {
            PrintWarranty(paperInfo.Car, paperInfo.Warranty);
        }

        internal static void PrintWarranty(Car car, int warranty)
        {
            string docPath = "Documents\\Warranty.docx";
            if (!File.Exists(docPath))
            {
                MessageBox.Show(docPath + " file does not exist.");
                return;
            }

            word.Application application = new word.Application();
            word.Document document = application.Documents.Open(Path.Combine(Environment.CurrentDirectory, docPath));
            document.Activate();
            foreach (word.Shape shape in document.Shapes)
            {
                if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoTextBox)
                    switch (shape.TextFrame.ContainingRange.Text.Replace("\r", ""))
                    {
                        case "[Year]":
                            shape.TextFrame.ContainingRange.Text = car.Year.ToString();
                            break;

                        case "[Make]":
                            shape.TextFrame.ContainingRange.Text = car.Make;
                            break;

                        case "[Model]":
                            shape.TextFrame.ContainingRange.Text = car.Model;
                            break;

                        case "[VIN]":
                            shape.TextFrame.ContainingRange.Text = car.VIN;
                            break;

                        case "[Percent]":
                            shape.TextFrame.ContainingRange.Text = warranty.ToString();
                            break;
                    }
            }
            document.PrintOut(true, Pages: "1");
            document.Close(false);
            application.Quit();
        }

        internal static void PrintTransferAgreement(PaperInfo paperInfo)
        {
            string docPath = "Documents\\Agreement.docx";
            if (!File.Exists(docPath))
            {
                MessageBox.Show(docPath + " file does not exist.");
                return;
            }

            word.Application application = new word.Application();
            word.Document document = application.Documents.Open(Path.Combine(Environment.CurrentDirectory, docPath));
            document.Activate();

            foreach (word.Shape shape in document.Shapes)
            {
                if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoTextBox)
                    switch (shape.TextFrame.ContainingRange.Text.Replace("\r", ""))
                    {
                        case "[Date]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Date.ToString("MM/dd/yyyy");
                            break;

                        #region Person

                        case "[Buyer]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.Name;
                            break;

                        case "[City-State]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.City + ", " + paperInfo.Buyer.State;
                            break;

                        #endregion Person

                        #region Car

                        case "[Year]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Year.ToString();
                            break;

                        case "[Make]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Make;
                            break;

                        case "[Model]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Model;
                            break;

                        case "[VIN]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.VIN;
                            break;

                        #endregion Car

                        #region Seller/Buyer

                        case "[Dealer]":
                            shape.TextFrame.ContainingRange.Text = DealerInfo.DealerName;
                            break;

                        case "[Seller]":
                            shape.TextFrame.ContainingRange.Text = DealerInfo.Dealer;
                            break;

                        #endregion Seller/Buyer

                        default:
                            break;
                    }
            }
            document.PrintOut(true, ManualDuplexPrint: false);
            document.Close(false);
            application.Quit();
        }

        internal static void PrintLegal(PaperInfo paperInfo)
        {
            string docPath = "Documents\\Legal.docx";
            if (!File.Exists(docPath))
            {
                MessageBox.Show(docPath + " file does not exist.");
                return;
            }

            word.Application application = new word.Application();
            word.Document document = application.Documents.Open(Path.Combine(Environment.CurrentDirectory, docPath));
            document.Activate();

            decimal difference = paperInfo.Car.Value - (paperInfo.Trade == null ? 0 : paperInfo.Trade.Value);
            decimal tax = paperInfo.OutOfState ? 0 : difference * paperInfo.Tax;
            decimal lien = paperInfo.Lien ? Constants.LIEN_COST : 0;
            decimal tag = paperInfo.Tag ? Constants.TAG_COST : 0;
            decimal total = difference + tax + lien + tag;
            decimal due = total - paperInfo.Down;

            foreach (word.Shape shape in document.Shapes)
            {
                if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoTextBox)
                    switch (shape.TextFrame.ContainingRange.Text.Replace("\r", ""))
                    {
                        #region Date

                        case "[Date]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Date.ToString("MM/dd/yyyy");
                            break;

                        case "[Month2]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Date.ToString("MMM", CultureInfo.InvariantCulture);
                            break;

                        case "[Yr]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Date.Year.ToString();
                            break;

                        #endregion Date

                        #region Person

                        case "[Buyer]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.Name;
                            break;

                        case "[Co-Buyer]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.CoBuyer;
                            break;

                        case "[Address]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.Address;
                            break;

                        case "[City]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.City;
                            break;

                        case "[ZIP]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.ZIP;
                            break;

                        #endregion Person

                        #region Car

                        case "[Car]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.ToString();
                            break;

                        case "[Mileage]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Mileage == null ? "Exempt" : paperInfo.Car.Mileage.Value.ToString();
                            break;

                        case "[VIN]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.VIN;
                            break;

                        #endregion Car

                        #region Money

                        case "[Due]":
                            shape.TextFrame.ContainingRange.Text = due.ToString("N2");
                            break;

                        #endregion Money

                        #region Disclosure

                        case "[Payment]":
                            shape.TextFrame.ContainingRange.Text = due == 0 ? String.Empty : paperInfo.AveragePayment.ToString("N2");
                            break;

                        #endregion Disclosure

                        default:
                            break;
                    }
            }
            document.PrintOut(true, ManualDuplexPrint: true);
            document.Close(false);
            application.Quit();
        }

        internal static void PrintLien(PaperInfo paperInfo)
        {
            string docPath = "Documents\\Lien Release.docx";
            if (!File.Exists(docPath))
            {
                MessageBox.Show(docPath + " file does not exist.");
                return;
            }

            word.Application application = new word.Application();
            word.Document document = application.Documents.Open(Path.Combine(Environment.CurrentDirectory, docPath));
            document.Activate();
            foreach (word.Shape shape in document.Shapes)
            {
                if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoTextBox)
                    switch (shape.TextFrame.ContainingRange.Text.Replace("\r", ""))
                    {
                        case "[Owner]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.Name;
                            break;

                        case "[CoOwner]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.CoBuyer;
                            break;

                        case "[Address]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.Address;
                            break;

                        case "[City]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.City;
                            break;

                        case "[State]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.State;
                            break;

                        case "[Zip]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Buyer.ZIP;
                            break;

                        case "[Year]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Year.ToString();
                            break;

                        case "[Make]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.Make;
                            break;

                        case "[VIN]":
                            shape.TextFrame.ContainingRange.Text = paperInfo.Car.VIN;
                            break;
                    }
            }
            document.PrintOut(true, ManualDuplexPrint: false);
            document.Close(false);
            application.Quit();
        }

        internal static void PrintReceipt(Person person, PaymentCar car, int index = 0, decimal due = 0)
        {
            string docPath = "Documents\\Statement_Empty.xlsx";
            if (!File.Exists(docPath))
            {
                MessageBox.Show(docPath + " file does not exist.");
                return;
            }

            excel.Application Excel = new excel.Application();
            excel.Workbook wb = Excel.Workbooks.Open(Path.Combine(Environment.CurrentDirectory, docPath));
            excel.Worksheet ws = wb.Sheets[1];

            if (due == 0)
                due = car.Due;

            ws.Cells[10, "B"] = "Initial Due: " + car.Due.ToString("C");
            ws.Cells[7, "B"] = person.Name;
            ws.Cells[8, "B"] = car.ToString();
            ws.Cells[3, "G"] = DateTime.Today.ToString("MM/dd/yyyy");

            for (int i = 0; i <= 22; i++)
            {
                if (index >= car.Payments.Count)
                    break;

                Payment p = car.Payments[index];
                if (p.IsDownPayment)
                    ws.Cells[12 + i, "A"] = "Down Payment";
                ws.Cells[12 + i, "B"] = p.Date.ToString("MM/dd/yyyy");
                ws.Cells[12 + i, "C"] = p.Amount.ToString("C");
                ws.Cells[12 + i, "D"] = (due -= p.Amount).ToString("C");
                index++;
            }
            ws.Cells[36, "G"] = car.Balance.ToString("C");
            wb.PrintOutEx();
            wb.Close(false);
            Excel.Quit();

            if (index < car.Payments.Count)
                PrintReceipt(person, car, index, due);
        }

        internal static void PrintPapers(PaperInfo paperInfo, bool legal = false)
        {
            PrintContract(paperInfo);
            PrintTransferAgreement(paperInfo);
            PrintWarranty(paperInfo);
            if (legal)
                PrintLegal(paperInfo);
            if (paperInfo.Lien)
                PrintLien(paperInfo);
        }

        internal static void AddEndOfYear(PaperInfo paperInfo, decimal boughtPrice)
        {
            string docPath = "Documents\\EndOfYear.xlsx";
            if (!File.Exists(docPath))
            {
                MessageBox.Show(docPath + " file does not exist.");
                return;
            }

            excel.Application application = new excel.Application();
            excel.Workbook wb = application.Workbooks.Open(Path.Combine(Environment.CurrentDirectory, docPath));
            excel.Worksheet ws = wb.Sheets[1];

            int i = 1;
            while (ws.Cells[i, "A"].Value != null)
                i++;

            ws.Cells[i, "A"] = DateTime.Now.ToString("MM/dd/yyyy");
            ws.Cells[i, "B"] = paperInfo.Buyer.Name;
            ws.Cells[i, "C"] = paperInfo.Car.ToString();
            ws.Cells[i, "D"] = paperInfo.Car.VIN;
            ws.Cells[i, "E"] = boughtPrice;
            ws.Cells[i, "F"] = paperInfo.Car.Value;

            if (paperInfo.Trade != null)
            {
                ws.Cells[1 + i, "H"] = paperInfo.Trade.ToString();
                ws.Cells[1 + i, "I"] = paperInfo.Trade.Value;
                ws.Cells[1 + i, "J"] = paperInfo.Trade.VIN;
            }
            wb.Save();
            wb.Close();
            application.Quit();
        }
    }
}