namespace AnalyzerDatabase.Models
{
    public class ByMagazine
    {
        #region Variables
        public string Name { get; set; }
        public int Amount { get; set; }
        #endregion

        #region Constructors
        public ByMagazine(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }
        #endregion
    }
}