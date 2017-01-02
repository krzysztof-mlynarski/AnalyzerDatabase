namespace AnalyzerDatabase.Models
{
    public class OverallStatistics2
    {
        #region Variables
        public string Name { get; set; }
        public int Amount { get; set; }
        #endregion

        #region Constructors
        public OverallStatistics2(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }
        #endregion
    }
}
