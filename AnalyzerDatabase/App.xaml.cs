using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AnalyzerDatabase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private CultureInfo cultureOverride = new CultureInfo("pl-PL");
        public App()
        {
            if (Debugger.IsAttached == true && cultureOverride != null)
            {
                Thread.CurrentThread.CurrentUICulture = cultureOverride;
                Thread.CurrentThread.CurrentCulture = cultureOverride;
            }
        }
    }
}
