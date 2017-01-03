using System;
using System.Diagnostics;
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
            bool isConnected;
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
            using (Process myProcess = new Process())
            {
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.RedirectStandardInput = true;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.FileName = "cmd.exe";
                myProcess.Start();
                myProcess.StandardInput.WriteLine("ipconfig");
                myProcess.StandardInput.WriteLine("exit");
                myProcess.WaitForExit();

                string content = myProcess.StandardOutput.ReadToEnd();
                if (content.Contains("0.0.0.0"))
                {
                    return true;
                }

                return false;
            }
        }

        #endregion
    }
}