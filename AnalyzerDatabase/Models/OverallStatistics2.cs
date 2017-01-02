using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDatabase.Models
{
    public class OverallStatistics2
    {
        public string Name { get; set; }
        public int Amount { get; set; }

        public OverallStatistics2(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
