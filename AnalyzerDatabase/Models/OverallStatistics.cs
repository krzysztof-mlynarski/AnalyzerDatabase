namespace AnalyzerDatabase.Models
{
    public class OverallStatistics
    {
        public string Database { get; set; }
        public int Amount { get; set; }

        public OverallStatistics(string database, int amount)
        {
            Database = database;
            Amount = amount;
        }
    }
}