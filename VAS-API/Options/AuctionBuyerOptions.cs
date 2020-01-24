using System.Collections.Generic;
using System.IO;

namespace VAS_API.Options
{
    public sealed class AuctionBuyerOptions
    {
        public string PyUser { get; set; }
        public string PyPassword { get; set; }
        public string PyBudget { get; set; }
        public string PyItems { get; set; }
        public List<string> FilePathArray { get; set; }
        public string GetFilePath
        {
            get => Path.Combine(FilePathArray.ToArray());
        }
    }
}
