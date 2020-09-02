using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using SBClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new SwedishBullsClient();
            var s = await client.GetSnapshot("8TRA.ST");

            await client.Login("test@gmail.com", "test");

            Console.WriteLine(s.SignalUpdate);
            Console.WriteLine(s.MarketOutlook);
        }
    }

}
