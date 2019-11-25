using System;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;

namespace Searchfight.Service
{
    public class BingSearch : ISearch
    {
        public long SearchResult { get ; set; }
        public string AccessKey { get; set; }
        public long Winner { get; set; }
        public string WinnerKeyword { get; set; }

        const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/search";

        public BingSearch(string accessKey)
        {
            this.AccessKey = accessKey;
        }

        /// <summary>
        /// Se implementa la búsqueda a traves del servicio de Bing, es necesario tener un key
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public long Search(string keyword)
        {
            if (AccessKey.Length == 32)
            {
                var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(keyword);

                WebRequest request = HttpWebRequest.Create(uriQuery);
                request.Headers["Ocp-Apim-Subscription-Key"] = AccessKey;
                HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
                string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //Console.WriteLine("\nJSON Response:\n");
                //Console.WriteLine(json);
                ProcessSearchResult(json);
            }
            else
            {
                Console.WriteLine("El AccessKey no es correcto");
            }
            return SearchResult;
        }

        /// <summary>
        /// Método que procesa el resultado del search engine
        /// </summary>
        public void ProcessSearchResult(string json)
        {
            var jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), new System.Xml.XmlDictionaryReaderQuotas());
            var root = XElement.Load(jsonReader);
            try
            {
                string resultVal = root.Element("webPages").Element("totalEstimatedMatches").Value;
                //Console.WriteLine("Valor de campo: webPages/totalEstimatedMatches = " + resultVal);
                SearchResult = long.Parse(resultVal);
            }
            catch (Exception e)
            {
                Console.WriteLine("Hubo un error al obtener el total de búsquedas");
                Console.WriteLine("Excepcion: " + e.Message);
            }
        }

        public override string ToString()
        {
            return "Bing Search";
        }
    }
}
