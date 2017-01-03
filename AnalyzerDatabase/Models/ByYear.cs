namespace AnalyzerDatabase.Models
{
    public class ByYear
    {
        #region Variables
        public string Year { get; set; }
        public int Amount { get; set; }
        #endregion

        #region Constructors
        public ByYear(string year, int amount)
        {        
            Year = year;
            Amount = amount;
        }
        #endregion
    }
}