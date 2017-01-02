namespace AnalyzerDatabase.Models
{
    public class ByAuthor
    {
        public string Name { get; set; }
        public int Amount { get; set; }

        public ByAuthor(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}