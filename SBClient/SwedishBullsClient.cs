using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SBClient
{
    public class SwedishBullsClient
    {
        private IBrowsingContext context;

        public SwedishBullsClient()
        {
            var config = Configuration.Default
                .WithDefaultLoader(new LoaderOptions { IsResourceLoadingEnabled = true })
                .WithDefaultCookies()
                .WithJs();

            context = BrowsingContext.New(config);
        }

        public async Task Login(string username, string password)
        {

            //probably doesnt work, cant really test
            var queryDocument = await context.OpenAsync("https://www.swedishbulls.com/Signin.aspx?lang=en");
            ((IHtmlInputElement)queryDocument.GetElementById("MainContent_uEmail")).Value = username;
            ((IHtmlInputElement)queryDocument.GetElementById("MainContent_uPassword")).Value = password;
            ((IHtmlElement)queryDocument.GetElementById("MainContent_btnSubmit")).DoClick();

            //There is no such user
            var s = Stopwatch.StartNew();
            while (s.Elapsed.TotalSeconds < 20)
            {
                var message = context.Current.Document.GetElementById("MainContent_msg");
                Console.WriteLine(message?.TextContent);
                await Task.Delay(1000);
            }
        }

        public async Task<SBSnapshot> GetSnapshot(string ticker)
        {
            var doc = await context.OpenAsync("https://www.swedishbulls.com/SignalPage.aspx?lang=en&Ticker=" + ticker);


            var intraday = doc.GetElementById("MainContent_ASPxRoundPanel2_WebChartControl1_IMG")?.Attributes["src"].Value;
            if (intraday != null)
            {
                var download = context.GetService<IDocumentLoader>().FetchAsync(new DocumentRequest(new Url("https://www.swedishbulls.com" + intraday)));
                var res = await download.Task;
                MemoryStream ms = new MemoryStream();
                res.Content.CopyTo(ms);
                var bytes = ms.ToArray();
                File.WriteAllBytes("test.jpg", bytes);
            }


            var signalHistory = doc.GetElementById("MainContent_signalpagehistory_PatternHistory24_DXMainTable").GetElementsByTagName("tbody").First().Children.Skip(1).Select(tr =>
            {
                var td = tr.GetElementsByTagName("td").ToList();
                return new SBSignalHistoryItem
                {
                    Date = DateTime.TryParseExact(td[0].TextContent, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out var d) ? d : DateTime.MinValue,
                    Price = decimal.TryParse(td[1].TextContent, out var p) ? p : 0,
                    Signal = td[2].TextContent,
                };
            }).ToList();

            return new SBSnapshot
            {
                SignalUpdate = doc.GetElementById("MainContent_signalpagedailycommentarytext")?.TextContent,
                MarketOutlook = doc.GetElementById("MainContent_signalpagemaincommentarytext")?.TextContent,
                PatternDescription = doc.GetElementById("MainContent_signalpageinfotextpattern")?.TextContent,
                LastSignal = doc.GetElementById("MainContent_LastSignal")?.TextContent,
                LastPattern = doc.GetElementById("MainContent_LastPattern")?.TextContent,
                LastClose = decimal.TryParse(doc.GetElementById("MainContent_LastClose")?.TextContent, out var lc) ? lc : 0,
                Change = decimal.TryParse(doc.GetElementById("MainContent_Change")?.TextContent, out var c) ? c : 0,
                PercentChange = decimal.TryParse(doc.GetElementById("MainContent_ChangePercent")?.TextContent, out var pc) ? pc : 0,
                SignalHistory = signalHistory
            };
        }
    }
}
