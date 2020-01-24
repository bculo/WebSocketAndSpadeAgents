using System.Collections.Generic;
using System.Linq;

namespace VAS_API.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int TotalBudget => WantedItems?.Sum(i => i.Budget) ?? 0;
        public List<WantedItem> WantedItems { get; set; }
    }
}
