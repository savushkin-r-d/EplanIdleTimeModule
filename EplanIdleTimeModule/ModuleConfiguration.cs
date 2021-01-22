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
        /// <param name="configFilePath">Путь к app.config, опционально</param>
        void Read(string configFilePath);

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
                File.Copy(pathToFile, currentAssemblyPath, owerwtire);
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
                MaxChecksCount = int.Parse(maxChecksCountKey.Value);
            }

            var checkIntervalKey = appSettings.Settings[checkIntervalSetting];
            if (checkIntervalKey != null &&
                !string.IsNullOrEmpty(checkIntervalKey.Value))
            {
                CheckInterval =
                    new TimeSpan(0, 0, int.Parse(checkIntervalKey.Value));
            }
        }

        public int MaxChecksCount { get; set; } = 60;

        public TimeSpan CheckInterval { get; set; } = new TimeSpan(0, 0, 60);

        /// <summary>
        /// Путь к запущенной dll
        /// </summary>
        public string RunningAssemblyLocation =>
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
