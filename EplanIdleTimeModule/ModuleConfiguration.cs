using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace IdleTimeModule
{
    public interface IModuleConfiguration
    {
        /// <summary>
        /// Прочитать конфигурацию модуля
        /// </summary>
        /// <param name="pathToCopyConfigFile">Путь куда скопировать configFile
        /// </param>
        void Read(string pathToCopyConfigFile);

        /// <summary>
        /// Интервал проверки простоя
        /// </summary>
        TimeSpan CheckInterval { get; }

        /// <summary>
        /// Максимальное число проверок до вывода окна
        /// </summary>
        int MaxChecksCount { get; }
    }

    public class ModuleConfiguration : IModuleConfiguration
    {
        public void Read(string configFilePath = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(configFilePath))
                {
                    CopyAppConfig(configFilePath);
                }

                Configuration configuration;
                configuration = ConfigurationManager
                    .OpenExeConfiguration(RunningAssemblyLocation);

                AppSettingsSection appSettings = configuration.AppSettings;
                ReadProperties(appSettings);
            }
            catch
            {
                string errMessage = "Не найден App.config. Используются " +
                    "стандартные значения";
                MessageBox.Show(errMessage, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Копировать конфигурационный файл в папку с dll.
        /// </summary>
        /// <param name="filePath"></param>
        private void CopyAppConfig(string filePath)
        {
            const string fileName = "EPLAN.EplAddin.IdleTimeModule.dll.config";
            string dir = Path.GetDirectoryName(filePath);
            string pathToFile = Path.Combine(dir, fileName);
            if (File.Exists(pathToFile))
            {
                string currentAssemblyDir = Path
                    .GetDirectoryName(RunningAssemblyLocation);
                string currentAssemblyPath = Path
                    .Combine(currentAssemblyDir, fileName);
                bool owerwtire = true;
                if (pathToFile != currentAssemblyPath)
                {
                    File.Copy(pathToFile, currentAssemblyPath, owerwtire);
                }
            }
        }

        /// <summary>
        /// Прочитать свойства из AppSettings секции конфигурации
        /// </summary>
        /// <param name="appSettings">Ссылка на секцию AppSettings</param>
        private void ReadProperties(AppSettingsSection appSettings)
        {
            var maxChecksCountKey = appSettings.Settings[maxChecksCountSetting];
            if (maxChecksCountKey != null &&
                !string.IsNullOrEmpty(maxChecksCountKey.Value))
            {
                bool parsed = int.TryParse(maxChecksCountKey.Value,
                    out int parsedMaxChecksCount);
                if (parsed)
                {
                    MaxChecksCount = parsedMaxChecksCount;
                }
            }

            var checkIntervalKey = appSettings.Settings[checkIntervalSetting];
            if (checkIntervalKey != null &&
                !string.IsNullOrEmpty(checkIntervalKey.Value))
            {
                bool parsed = int.TryParse(checkIntervalKey.Value,
                    out int parsedCheckInterval);
                if (parsed)
                {
                    CheckInterval = TimeSpan.FromSeconds(parsedCheckInterval);
                }
            }
        }

        public int MaxChecksCount { get; set; } = 60;

        public TimeSpan CheckInterval { get; set; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// Путь к запущенной dll
        /// </summary>
        private string RunningAssemblyLocation =>
            Assembly.GetExecutingAssembly().Location;

        /// <summary>
        /// Название ключа с максимальным числом проверок до вывода окна
        /// </summary>
        private const string maxChecksCountSetting = "maxChecksCount";

        /// <summary>
        /// Название ключа с интервалом проверки простоя
        /// </summary>
        private const string checkIntervalSetting = "checkIntervalSec";
    }
}
