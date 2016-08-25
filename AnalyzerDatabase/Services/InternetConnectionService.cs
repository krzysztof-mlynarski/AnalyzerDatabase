using System.Net.NetworkInformation;
using System.Threading.Tasks;
using AnalyzerDatabase.Interfaces;

namespace AnalyzerDatabase.Services
{
    public class InternetConnectionService : IInternetConnectionService
    {
        //public bool CheckConnection()
        //{
        //    ConnectionProfile internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

        //    if (internetConnectionProfile == null)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public async Task<bool> IsNetworkAvailable()
        {
            if (!(await Task.Run(() => NetworkInterface.GetIsNetworkAvailable())))
                return false;
            else
                return true;
        }

        //public bool IsInternetAccess()
        //{
        //    if (!(NetworkInformation.GetInternetConnectionProfile().GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess))
        //        return false;
        //    else
        //        return true;
        //}
    }
}