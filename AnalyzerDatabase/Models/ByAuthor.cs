namespace AnalyzerDatabase.Models
{
    public class ByAuthor
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Amount { get; set; }

        public ByAuthor(string name, string surname, int amount)
        {
            Name = name;
            Surname = surname;
            Amount = amount;
        }
    }
}