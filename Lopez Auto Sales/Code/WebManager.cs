using Lopez_Auto_Sales.JSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

namespace Lopez_Auto_Sales
{
    internal static class WebManager
    {
        private const string DECODE_URL = "https://vpic.nhtsa.dot.gov/api/vehicles/DecodeVin/{0}?format=json";
        private const int exifOrientationID = 0x112;
        private const int IMG_SIZE = 1080;
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        internal struct Paths
        {
            public const string ROOT = "Data/";
            public const string INPUT = "Data/Input/";
            public const string OUTPUT = "Data/Output/";
            public const string IMAGES = "Data/Output/CarImages/";
            public const string JSON = "Data/Output/CarData/";
            public const string LOGS = "Data/Logs/";
            public const string JSON_FILE = "cars.json";
            public const string IMAGE_TYPE = ".jpg";
            public static readonly string[] Directories = { ROOT, INPUT, OUTPUT, IMAGES, JSON, LOGS };
        }

        internal static void Init()
        {
            foreach (string directory in Paths.Directories)
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
        }

        private static void ExifRotate(this Image img)
        {
            if (!img.PropertyIdList.Contains(exifOrientationID))
                return;

            PropertyItem prop = img.GetPropertyItem(exifOrientationID);
            int val = BitConverter.ToUInt16(prop.Value, 0);
            RotateFlipType rot = RotateFlipType.RotateNoneFlipNone;

            if (val == 3 || val == 4)
                rot = RotateFlipType.Rotate180FlipNone;
            else if (val == 5 || val == 6)
                rot = RotateFlipType.Rotate90FlipNone;
            else if (val == 7 || val == 8)
                rot = RotateFlipType.Rotate270FlipNone;
            if (val == 2 || val == 4 || val == 5 || val == 7)
                rot |= RotateFlipType.RotateNoneFlipX;
            if (rot != RotateFlipType.RotateNoneFlipNone)
                img.RotateFlip(rot);
        }

        private static void ProcessImage(string path, string vin)
        {
            Image image = new Bitmap(path);
            image.ExifRotate();
            Bitmap resize = new Bitmap(image, new Size(IMG_SIZE, IMG_SIZE));
            image.Dispose();
            resize.Save(Paths.IMAGES + vin + Paths.IMAGE_TYPE, ImageFormat.Jpeg);
        }

        internal static JSONCar GetInfo(SalesCar salesCar)
        {
            if (File.Exists(Paths.JSON + Paths.JSON_FILE))
            {
                string json = File.ReadAllText(Paths.JSON + Paths.JSON_FILE);
                JSONFile jsonFile = JsonConvert.DeserializeObject<JSONFile>(json, SerializerSettings);
                if (jsonFile != null)
                {
                    List<JSONCar> cars = new List<JSONCar>(jsonFile.Cars);

                    if (cars.Any(match => match.VIN == salesCar.VIN))
                        return cars.Find(match => match.VIN == salesCar.VIN);
                }
            }
            JSONCar jsonCar = new JSONCar(salesCar);
            jsonCar.ProcessAPI();
            return jsonCar;
        }

        internal static JSONClass DecodeVIN(string VIN)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(DECODE_URL, VIN));

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return JsonConvert.DeserializeObject<JSONClass>(reader.ReadToEnd());
            }
        }

        private static void ProcessAPI(this JSONCar jsonCar)
        {
            JSONClass json = DecodeVIN(jsonCar.VIN);
            foreach (JSONResult result in json.Results)
            {
                if (string.IsNullOrWhiteSpace(result.Value))
                    continue;
                switch (result.Variable)
                {
                    case "Error Text":
                    case "Error Code":
                    case "Model Year":
                    case "Model":
                    case "Make":
                        //Already provided by JSONCar do nothing
                        break;

                    default:
                        jsonCar.Extras.Add(new JSONExtra(result.Variable, result.Value));
                        break;
                }
            }
        }

        private static void DeleteImages()
        {
            foreach (string file in Directory.GetFiles(Paths.IMAGES))
                File.Delete(file);
        }

        public static void CheckForUpdates()
        {
            DeleteImages();
            List<JSONCar> cars = new List<JSONCar>();

            if (File.Exists(Paths.JSON + Paths.JSON_FILE))
            {
                JSONFile jsonFile = JsonConvert.DeserializeObject<JSONFile>(File.ReadAllText(Paths.JSON + Paths.JSON_FILE), SerializerSettings);

                //set car info according to previously created file
                if (jsonFile != null)
                {
                    cars = new List<JSONCar>(jsonFile.Cars);
                    //check for any vehicles that have been removed
                    for (int i = cars.Count - 1; i >= 0; i--)
                    {
                        JSONCar car = cars[i];
                        if (!Storage.SalesCars.Any(match => match.VIN == car.VIN))
                            cars.Remove(car);
                        else
                        {
                            SalesCar salesCar = Storage.SalesCars.Find(match => match.VIN == car.VIN);
                            car.Update(salesCar);
                        }
                    }
                }
            }

            foreach (SalesCar car in Storage.SalesCars)
            {
                string path = Paths.INPUT + car.VIN + Paths.IMAGE_TYPE;
                if (!File.Exists(path))
                    continue;
                //has already been checked
                if (cars.Any(match => match.VIN == car.VIN))
                    continue;

                ProcessImage(path, car.VIN);
                JSONCar jsonCar = new JSONCar(car);
                jsonCar.ProcessAPI();
                cars.Add(jsonCar);
            }
            JSONFile output = new JSONFile(cars);
            File.WriteAllText(Paths.JSON + Paths.JSON_FILE, JsonConvert.SerializeObject(output, Formatting.Indented, SerializerSettings));
        }
    }
}