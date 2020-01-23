using System.Collections.Generic;
using System.IO;

namespace VAS_API.Options
{
    public sealed class AuctionOrganizerOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PyUser { get; set; }
        public string PyPassword { get; set; }
        public List<string> FilePathArray { get; set; }

        public string GetFilePath
        {
            get => Path.Combine(FilePathArray.ToArray());
        }
    }
}
