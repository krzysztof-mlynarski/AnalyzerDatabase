namespace AnalyzerDatabase.Models
{
    public class ByMagazine
    {
        public string Name { get; set; }
        public int Amount { get; set; }

        public ByMagazine(string name, int amount)
        {
            this.Name = name;
            this.Amount = amount;
        }
    }
}