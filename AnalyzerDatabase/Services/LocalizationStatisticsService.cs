using System;

namespace AnalyzerDatabase.Services
{
    public class LocalizationStatisticsService
    {
        public static string StatisticsName
        {
            get
            {
                return "AnalyzerDatabase";
            }
        }

        public static string AppDataPath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables("%AppData%");
            }
        }

        public static string ProgramDataApplicationDirectory
        {
            get
            {
                return ZetaLongPaths.ZlpPathHelper.Combine(AppDataPath, StatisticsName);
            }
        }

        public static string AnalyzerDatabaseStatisticsPath
        {
            get
            {
                return ZetaLongPaths.ZlpPathHelper.Combine(AppDataPath, StatisticsName,
                    "AnalyzerDatabaseStatistics.config");
            }
        }
    }
}