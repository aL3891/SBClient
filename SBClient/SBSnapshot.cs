using System.Collections.Generic;

namespace SBClient
{
    public class SBSnapshot
    {
        public string SignalUpdate { get; set; }
        public string MarketOutlook { get; set; }
        public string PatternDescription { get; set; }
        public string LastSignal { get; set; }
        public string LastPattern { get; set; }
        public decimal LastClose { get; set; }
        public decimal Change { get; set; }
        public decimal PercentChange { get; set; }
        public List<SBSignalHistoryItem> SignalHistory { get; set; }
    }
}
