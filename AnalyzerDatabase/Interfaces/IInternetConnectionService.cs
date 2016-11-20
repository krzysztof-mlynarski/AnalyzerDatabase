namespace AnalyzerDatabase.Interfaces
{
    public interface IInternetConnectionService
    {
        bool CheckConnectedToInternet();
        bool CheckConnectedToInternetVpn();
    }
}