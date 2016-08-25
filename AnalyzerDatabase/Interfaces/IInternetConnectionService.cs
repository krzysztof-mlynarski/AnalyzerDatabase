using System.Threading.Tasks;

namespace AnalyzerDatabase.Interfaces
{
    public interface IInternetConnectionService
    {
        //bool CheckConnection();
        Task<bool> IsNetworkAvailable();
        //bool IsInternetAccess();
    }
}