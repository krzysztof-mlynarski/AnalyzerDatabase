namespace AnalyzerDatabase.Interfaces
{
    public interface IDeserializeJsonService
    {
        T GetObjectFromJson<T>(string jsonToDeserialize);
    }
}