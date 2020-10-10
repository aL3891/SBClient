using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using PuppeteerSharp;
using PuppeteerSharp.Media;
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
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                //Headless = false
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://www.swedishbulls.com/SignalPage.aspx?lang=en&Ticker=8TRA.ST");

            //await page.EvaluateExpressionAsync("document.getElementById('MainContent_uEmail').value = " + "'gurka@gmail.com'");
            //await page.EvaluateExpressionAsync("document.getElementById('MainContent_uPassword').value = " + "'passwörd'");
            //await page.EvaluateExpressionAsync("document.getElementById('MainContent_btnSubmit').click()");
            //await page.SetViewportAsync(new ViewPortOptions
            //{
            //    Width = 1024,
            //    Height = 780
            //});

           
            var element =  await page.QuerySelectorAsync("#MainContent_ASPxRoundPanel2_WebChartControl1_IMG");
            //var element = await page.EvaluateExpressionAsync<ElementHandle>("document.getElementById('MainContent_ASPxRoundPanel2_WebChartControl1_IMG')");
           await  element.ScreenshotAsync("gurka.png");

            //await page.ScreenshotAsync("test.png", new ScreenshotOptions
            //{
            //    Clip = new Clip()
            //    {
            //        X = 10,
            //        Y = 10,
            //        Height = 100,
            //        Width = 100
            //    }
            //});


           var res=  await page.EvaluateExpressionAsync<string>("document.getElementById('MainContent_signalpagedailycommentarytext').textContent");

            element = await page.QuerySelectorAsync("#MainContent_signalpagedailycommentarytext");
            var apaaoa = (await element.GetPropertyAsync("textContent")).JsonValueAsync() ;
            //var res = await element.EvaluateFunctionAsync<string>("textContent");

            await browser.CloseAsync();


            //var client = new SwedishBullsClient();
            ////var s = await client.GetSnapshot("8TRA.ST");
            ////Console.WriteLine(s.SignalUpdate);
            ////Console.WriteLine(s.MarketOutlook);

            //await client.Login("test@gmail.com", "test");

        }
    }

}
