using System;
using System.Globalization;
using AnalyzerDatabase.Models;

namespace AnalyzerDatabase.Services
{
    public class SettingsService
    {
        #region Variables
        private CultureInfo _culture;
        private AnalyzerDatabaseSettings _analyzerDatabaseSettings;
        #endregion

        #region Singleton
        private static SettingsService _instance;

        public static SettingsService Instance
        {
            get
            {
                return _instance ?? (_instance = new SettingsService());
            }
        }

        private SettingsService()
        {
        }
        #endregion

        #region Public methods
        public AnalyzerDatabaseSettings Settings
        {
            get
            {
                return _analyzerDatabaseSettings ?? (_analyzerDatabaseSettings = LoadSettings());
            }
        }

        public void Save()
        {
            XmlSerialize<AnalyzerDatabaseSettings>.Serialize(Settings, LocalizationSettingsService.AnalyzerDatabaseConfigPath);
        }
        #endregion

        #region Private methods
        private AnalyzerDatabaseSettings LoadSettings()
        {
            try
            {
                AnalyzerDatabaseSettings settings;
                if (ZetaLongPaths.ZlpIOHelper.FileExists(LocalizationSettingsService.AnalyzerDatabaseConfigPath))
                {
                    settings = XmlSerialize<AnalyzerDatabaseSettings>.Deserialize(
                        LocalizationSettingsService.AnalyzerDatabaseConfigPath);
                }
                else
                {
                    settings = EmptySettings();
                }

                return settings;
            }
            catch (Exception)
            {
                return EmptySettings();
            }
        }

        private AnalyzerDatabaseSettings EmptySettings()
        {
            AnalyzerDatabaseSettings settings = new AnalyzerDatabaseSettings();
            XmlSerialize<AnalyzerDatabaseSettings>.InitEmptyProperties(settings);

            settings.ScienceDirectAndScopusApiKey = "";
            settings.SpringerApiKey = "";
            settings.StartOnLogin = true;
            settings.SavingPublicationPath = LocalizationSettingsService.DefaultSavingLocalizationPublicationsPath;

            if (!ZetaLongPaths.ZlpIOHelper.DirectoryExists(LocalizationSettingsService.ProgramDataApplicationDirectory))
            {
                ZetaLongPaths.ZlpIOHelper.CreateDirectory(LocalizationSettingsService.ProgramDataApplicationDirectory);
            }

            XmlSerialize<AnalyzerDatabaseSettings>.Serialize(settings, LocalizationSettingsService.AnalyzerDatabaseConfigPath);

            return settings;
        }
        #endregion
    }
}