namespace AnalyzerDatabase.Models
{
    public class ByAuthor
    {
        #region Variables
        public string Name { get; set; }
        public int Amount { get; set; }
        #endregion

        #region Constructors
        public ByAuthor(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }
        #endregion
    }
}