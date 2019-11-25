using Searchfight.Service;
using System;
using System.Collections.Generic;

namespace Searchfight
{
    public class Program
    {
        static List<ISearch> engines;

        public static void Main(string[] args)
        {
            ConfigureSearchEngines();

            string totalWinnerKeyword = "";
            long totalWinner = 0;
            for (int i = 0; i < args.Length; i++)
            {
                string keyword = args[i];
                long total = 0;
                Console.Write(string.Format("{0}: ", keyword));

                // For each search engine configured
                foreach (ISearch engine in engines)
                {
                    //Console.WriteLine("Realizando la búsqueda en {0} para el valor: {1}", engine.ToString(), keyword);
                    long resultCount = engine.Search(keyword);
                    Console.Write(string.Format("{0}: {1} ", engine.ToString(), resultCount));

                    if (resultCount > engine.Winner)
                    {
                        engine.Winner = resultCount;
                        engine.WinnerKeyword = keyword;
                    }

                    total += resultCount;
                }

                if (total > totalWinner)
                {
                    totalWinner = total;
                    totalWinnerKeyword = keyword;
                }
                Console.WriteLine();
            }

            // Imprimir ganadores por Search engine
            foreach (ISearch engine in engines)
            {
                Console.WriteLine("{0} winner: {1}", engine.ToString(), engine.WinnerKeyword);
            }

            Console.WriteLine("Total winner: " + totalWinnerKeyword);
            Console.ReadKey();
        }

        private static void ConfigureSearchEngines()
        {
            engines = new List<ISearch>();
            string googleKey = "AIzaSyCZbpwdosXn-BcjgVF8wA7o3AtlFuqVxXw";
            string contextSearchId = "013643910918161912808:v6sbbaksguk";
            engines.Add(new GoogleSearch(googleKey, contextSearchId));

            string bingKey = "e2074e3c6eaf455b974546e6f8ad5d0e";
            engines.Add(new BingSearch(bingKey));
        }
    }
}
