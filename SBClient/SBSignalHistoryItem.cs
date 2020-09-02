using AngleSharp.Html.Dom;
using System;
using System.IO;

namespace SBClient
{
    public class SBSignalHistoryItem
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string Signal { get; set; }
    }
}
