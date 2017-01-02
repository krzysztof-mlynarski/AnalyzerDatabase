using System;
using System.Globalization;
using AnalyzerDatabase.Models;

namespace AnalyzerDatabase.Services
{
    public class SettingsService
    {
        private CultureInfo _culture;
        private AnalyzerDatabaseSettings _analyzerDatabaseSettings;

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

        public CultureInfo Culture
        {
            get
            {
                return _culture ?? (_culture = new CultureInfo(Settings.CurrentLanguage));
            }
            set
            {
                _culture = value;
            }
        }

        public AnalyzerDatabaseSettings Settings
        {
            get
            {
                return _analyzerDatabaseSettings ?? (_analyzerDatabaseSettings = LoadSettings());
            }
        }

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

            //settings.CurrentLanguage = "pl-PL";
            settings.CurrentStyle = "green.xaml";
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

        public void Save()
        {
            XmlSerialize<AnalyzerDatabaseSettings>.Serialize(Settings, LocalizationSettingsService.AnalyzerDatabaseConfigPath);
        }
    }
}