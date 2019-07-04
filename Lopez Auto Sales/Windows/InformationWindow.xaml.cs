using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;

namespace Lopez_Auto_Sales
{
    /// <summary>
    /// Interaction logic for InformationWindow.xaml
    /// </summary>
    public partial class InformationWindow : Window
    {
        public InformationWindow()
        {
            InitializeComponent();
        }

        private void PapersButton_Click(object sender, RoutedEventArgs e)
        {
            PapersWindow papersWindow = new PapersWindow();
            papersWindow.Show();
            this.Close();
        }

        private void ExpiredContractsButton_Click(object sender, RoutedEventArgs e)
        {
            ExpiredContractsWindow expiredWindow = new ExpiredContractsWindow();
            expiredWindow.Show();
            this.Close();
        }

        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not Implemented Yet: Gathering Data");
        }

        private void ResizeImage(string path, string vin)
        {
            if (File.Exists("Data/Logs/" + DateTime.Today.ToString("MMddyyyy") + "/" + vin + ".jpg"))
            {
                File.Delete(path);
                return;
            }
            Image image = new Bitmap(path);
            Bitmap resize = new Bitmap(image, new System.Drawing.Size(1080, 1080));
            resize.RotateFlip(RotateFlipType.Rotate90FlipNone);
            image.Dispose();
            File.Move(path, "Data/Logs/" + DateTime.Today.ToString("MMddyyyy") + "/" + vin + ".jpg");
            resize.Save("Data/Output/CarImages/" + vin + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private async void JsonButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("Data/Logs/" + DateTime.Today.ToString("MMddyyyy")))
                Directory.CreateDirectory("Data/Logs/" + DateTime.Today.ToString("MMddyyyy"));
            if (!Directory.Exists("Data/Output/CarImages"))
                Directory.CreateDirectory("Data/Output/CarImages");
            List<JsonCar> cars = new List<JsonCar>();
            foreach (SalesCar car in Storage.SalesCars)
            {
                string path = "Data/Input/" + car.VIN + ".jpg";
                if (!File.Exists(path))
                    continue;
                ResizeImage(path, car.VIN);
                JsonCar jsonCar = new JsonCar(car);
                JSONClass json = await VINDecoder.DecodeVINAsync(car.VIN);
                foreach (JSONResult result in json.Results)
                {
                    if (result.Value != null)
                        result.Value = result.Value.ToCapital();
                    switch (result.Variable)
                    {
                        case "Engine Number of Cylinders":
                            if (int.TryParse(result.Value, out int i))
                                jsonCar.NumCylinders = i;
                            else
                                jsonCar.NumCylinders = null;
                            break;

                        case "Drive Type":
                            jsonCar.DriveType = result.Value;
                            break;

                        case "Vehicle Type":
                            jsonCar.VehicleType = result.Value;
                            break;

                        case "Fuel Type - Primary":
                            jsonCar.FuelType = result.Value;
                            break;

                        case "Displacement (L)":
                            jsonCar.Displacement = result.Value;
                            break;

                        case "Doors":
                            if (int.TryParse(result.Value, out int j))
                                jsonCar.NumDoors = j;
                            else
                                jsonCar.NumDoors = null;
                            break;

                        case "Model Year":
                            jsonCar.Year = int.Parse(result.Value);
                            break;

                        case "Model":
                            jsonCar.Model = result.Value;
                            break;

                        case "Make":
                            jsonCar.Make = result.Value;
                            break;

                        case "Series":
                            jsonCar.Series = result.Value;
                            break;
                    }
                }
                cars.Add(jsonCar);
            }
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            File.WriteAllText("Data/Output/Data/cars.json", JsonConvert.SerializeObject(cars, Formatting.Indented, jsonSerializerSettings));
            MessageBox.Show("File Created Successfully");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
            if (!Directory.Exists("Data/Input"))
                Directory.CreateDirectory("Data/Input");
            if (!Directory.Exists("Data/Output"))
                Directory.CreateDirectory("Data/Output");
            if (!Directory.Exists("Data/Logs"))
                Directory.CreateDirectory("Data/Logs");
        }
    }
}