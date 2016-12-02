using System;

namespace AnalyzerDatabase.Services
{
    public class LocalizationSettingsService
    {
        public static string SettingsName
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
                return ZetaLongPaths.ZlpPathHelper.Combine(AppDataPath, SettingsName);
            }
        }

        public static string AnalyzerDatabaseConfigPath
        {
            get
            {
                return ZetaLongPaths.ZlpPathHelper.Combine(AppDataPath, SettingsName,
                    "AnalyzerDatabaseConfig.config");
            }
        }

        public static string DefaultSavingLocalizationPublicationsPath
        {
            get
            {
                return
                    ZetaLongPaths.ZlpPathHelper.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), SettingsName,
                        "Publications");
            }
        }
    }
}