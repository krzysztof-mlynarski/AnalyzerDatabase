using System;
using System.Runtime.InteropServices;
using AnalyzerDatabase.Interfaces;

namespace AnalyzerDatabase.Services
{

    public class InternetConnectionService : IInternetConnectionService
    {
        #region Public methods
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);
        public bool CheckConnectedToInternet()
        {
            bool isConnected = false;
            try
            {
                int desc;
                isConnected = InternetGetConnectedState(out desc, 0);
            }
            catch (Exception)
            {
                isConnected = false;
            }
            return isConnected;
        }

        public bool CheckConnectedToInternetVpn()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}