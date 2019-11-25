using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;

namespace Searchfight.Service
{
    public class GoogleSearch : ISearch
    {
        public long SearchResult { get; set; }
        public string AccessKey { get; set; }
        public string CXId { get; set; }
        public long Winner { get; set; }
        public string WinnerKeyword { get; set; }

        const string uriBase = "https://www.googleapis.com/customsearch/v1";

        public GoogleSearch(string accessKey, string customSearchEngineId)
        {
            this.AccessKey = accessKey;
            this.CXId = customSearchEngineId;
        }

        public void ProcessSearchResult(string json)
        {
            var jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), new System.Xml.XmlDictionaryReaderQuotas());
            XElement root = XElement.Load(jsonReader);
            try
            {
                string resultVal = root.Element("searchInformation").Element("totalResults").Value;
                //Console.WriteLine("Valor de campo: searchInformation/totalResults = " + resultVal);
                SearchResult = long.Parse(resultVal);
            }
            catch (Exception e)
            {
                Console.WriteLine("Hubo un error al obtener el total de búsquedas");
                Console.WriteLine("Excepcion: " + e.Message);
            }
        }

        public long Search(string keyword)
        {
            if (!string.IsNullOrEmpty(AccessKey) && !string.IsNullOrEmpty(CXId))
            {
                var uriQuery = uriBase + "?key=" + Uri.EscapeDataString(AccessKey) + "&cx=" + Uri.EscapeDataString(CXId) + "&q=" + Uri.EscapeDataString(keyword);

                WebRequest request = HttpWebRequest.Create(uriQuery);
                HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
                string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //Console.WriteLine("\nJSON Response:\n");
                //Console.WriteLine(json);
                ProcessSearchResult(json);
            }
            else
            {
                Console.WriteLine("El AccessKey o Custom Search Engine Id no son correctos");
            }
            return SearchResult;
        }

        public override string ToString()
        {
            return "Google";
        }
    }
}
