namespace AnalyzerDatabase.Models
{
    public class OverallStatistics
    {
        #region Variables
        public string Database { get; set; }
        public int Amount { get; set; }
        #endregion

        #region Constructors
        public OverallStatistics(string database, int amount)
        {
            Database = database;
            Amount = amount;
        }
        #endregion
    }
}