namespace AnalyzerDatabase.Models
{
    public class ByYear
    {
        public string Year { get; set; }
        public int Amount { get; set; }

        public ByYear(string year, int amount)
        {
            this.Year = year;
            this.Amount = amount;
        }
    }
}