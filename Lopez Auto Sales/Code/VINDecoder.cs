using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Lopez_Auto_Sales
{
    internal static class VINDecoder
    {
        private const string URL = "https://vpic.nhtsa.dot.gov/api/vehicles/DecodeVin/<vin>?format=json";

        /// <summary>Decodes the vin asynchronous.</summary>
        /// <param name="VIN">The vin.</param>
        /// <returns>JSONClass type of content</returns>
        public static async Task<JSONClass> DecodeVINAsync(string VIN)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(URL.Replace("<vin>", VIN))
                };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var tmp = client.GetAsync(URL.Replace("<vin>", VIN)).Result;
                if (tmp.IsSuccessStatusCode)
                {
                    return Deserialize<JSONClass>(await tmp.Content.ReadAsStringAsync());
                }
            }
            catch { }
            return null;
        }

        /// <summary>Deserializes the specified json.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns>Type specified</returns>
        public static T Deserialize<T>(string json)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            T obj = (T)ser.ReadObject(stream);
            return obj;
        }
    }

    [DataContract]
    internal class JSONClass
    {
        [DataMember]
        internal int Count { get; set; }

        [DataMember]
        internal string Message { get; set; }

        [DataMember]
        internal string SearchCriteria { get; set; }

        [DataMember]
        internal JSONResult[] Results { get; set; }
    }

    [DataContract]
    internal class JSONResult
    {
        [DataMember]
        internal string Value { get; set; }

        [DataMember]
        internal string ValueId { get; set; }

        [DataMember]
        internal string Variable { get; set; }

        [DataMember]
        internal string VariableId { get; set; }
    }
}