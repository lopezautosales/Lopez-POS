using Lopez_POS.JSON;
using Lopez_POS.Static;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

namespace Lopez_POS.Web
{
    /// <summary>
    /// Handles website operations.
    /// </summary>
    internal static class WebManager
    {
        /// <summary>
        /// The URL for decoding VIN's.
        /// </summary>
        private const string DECODE_URL = "https://vpic.nhtsa.dot.gov/api/vehicles/DecodeVin/{0}?format=json";

        /// <summary>
        /// The exif orientation identifier
        /// </summary>
        private const int exifOrientationID = 0x112;

        /// <summary>
        /// The img size
        /// </summary>
        private const int IMG_SIZE = 1080;

        /// <summary>
        /// The serializer settings
        /// </summary>
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        /// <summary>
        /// A struct containing path information.
        /// </summary>
        internal struct Paths
        {
            /// <summary>
            /// The root data folder.
            /// </summary>
            public const string ROOT = "Data/";

            /// <summary>
            /// The input folder.
            /// </summary>
            public const string INPUT = "Data/Input/";

            /// <summary>
            /// The output folder.
            /// </summary>
            public const string OUTPUT = "Data/Output/";

            /// <summary>
            /// The output images folder.
            /// </summary>
            public const string IMAGES = "Data/Output/CarImages/";

            /// <summary>
            /// The json data folder.
            /// </summary>
            public const string JSON = "Data/Output/CarData/";

            /// <summary>
            /// The car image logs folder.
            /// </summary>
            public const string LOGS = "Data/Logs/";

            /// <summary>
            /// The json file name
            /// </summary>
            public const string JSON_FILE = "cars.json";

            /// <summary>
            /// The image type
            /// </summary>
            public const string IMAGE_TYPE = ".jpg";

            /// <summary>
            /// The directories
            /// </summary>
            public static readonly string[] Directories = { ROOT, INPUT, OUTPUT, IMAGES, JSON, LOGS };
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Init()
        {
            foreach (string directory in Paths.Directories)
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
        }

        /// <summary>
        /// Rotates the provided image according to exif data.
        /// </summary>
        /// <param name="img">The img.</param>
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

        /// <summary>
        /// Processes the image. Resizes and rotates.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="vin">The vin.</param>
        private static void ProcessImage(string path, string vin)
        {
            Image image = new Bitmap(path);
            image.ExifRotate();
            Bitmap resize = new Bitmap(image, new Size(IMG_SIZE, IMG_SIZE));
            image.Dispose();
            resize.Save(Paths.IMAGES + vin + Paths.IMAGE_TYPE, ImageFormat.Jpeg);
        }

        /// <summary>
        /// Gets the json information.
        /// </summary>
        /// <param name="salesCar">The sales car.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the car basics from json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        internal static Car GetBasics(this JSONClass json)
        {
            try
            {
                int year = int.Parse(json.Results.First<JSONResult>(j => j.Variable == "Model Year").Value);
                string make = json.Results.First<JSONResult>(j => j.Variable == "Make").Value.ToCapital();
                string model = json.Results.First<JSONResult>(j => j.Variable == "Model").Value.ToCapital();

                return new Car(year, make, model);
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Determines whether [has error code].
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>
        ///   <c>true</c> if [has error code] [the specified json]; otherwise, <c>false</c>.
        /// </returns>
        internal static bool HasErrorCode(this JSONClass json)
        {
            try
            {
                int code = int.Parse(json.Results.First<JSONResult>(j => j.Variable == "Error Code").Value);

                if (code == 0)
                    return false;
            }
            catch { }
            return true;
        }

        /// <summary>
        /// Decodes the vin.
        /// </summary>
        /// <param name="VIN">The vin.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Processes the API from jsonCar to jsonClass.
        /// </summary>
        /// <param name="jsonCar">The json car.</param>
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

        /// <summary>
        /// Deletes the images in output folder.
        /// </summary>
        private static void DeleteImages()
        {
            foreach (string file in Directory.GetFiles(Paths.IMAGES))
                File.Delete(file);
        }

        /// <summary>
        /// Checks for updates to website files. Utilizes the gitmanager class.
        /// </summary>
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
                        if (Storage.SalesCarsList.Any(match => match.VIN == car.VIN))
                        {
                            SalesCar salesCar = Storage.SalesCarsList.Find(match => match.VIN == car.VIN);
                            cars[i].Update(salesCar);
                        }
                        else
                        {
                            cars.Remove(car);

                            if (File.Exists(Paths.INPUT + car.VIN + Paths.IMAGE_TYPE))
                                File.Move(Paths.INPUT + car.VIN + Paths.IMAGE_TYPE, Paths.LOGS + car.VIN + Paths.IMAGE_TYPE);
                        }
                    }
                }
            }

            foreach (SalesCar car in Storage.SalesCarsList)
            {
                string path = Paths.INPUT + car.VIN + Paths.IMAGE_TYPE;
                if (!File.Exists(path))
                    continue;

                ProcessImage(path, car.VIN);

                //has already been checked
                if (cars.Any(match => match.VIN == car.VIN))
                    continue;

                JSONCar jsonCar = new JSONCar(car);
                if (car.ByOwner)
                    jsonCar.Extras.Add(new JSONExtra("Note", "For Sale by Owner"));
                jsonCar.ProcessAPI();
                cars.Add(jsonCar);
            }
            JSONFile output = new JSONFile(cars);
            File.WriteAllText(Paths.JSON + Paths.JSON_FILE, JsonConvert.SerializeObject(output, Formatting.Indented, SerializerSettings));
            GitManager gitManager = new GitManager();
            gitManager.TryPushChanges();
        }
    }
}