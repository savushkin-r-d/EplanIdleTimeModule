using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DowntimeModule
{
    public static class ModuleConfiguration
    {
        static ModuleConfiguration()
        {
            MaxChecksCount = 60;
            CheckInterval = new TimeSpan(0, 0, 60);
        }

        /// <summary>
        /// Прочитать конфигурацию модуля
        /// </summary>
        /// <param name="assemblyPath">Путь к dll, опционально</param>
        public static void Read(string assemblyPath = "")
        {
            try
            {
                Configuration configuration;
                if (string.IsNullOrEmpty(assemblyPath))
                {
                    configuration = ConfigurationManager
                        .OpenExeConfiguration(RunningAssemblyLocation);
                }
                else
                {
                    configuration = ConfigurationManager
                        .OpenExeConfiguration(assemblyPath);
                }

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
        /// Прочитать свойства из AppSettings секции конфигурации
        /// </summary>
        /// <param name="appSettings">Ссылка на секцию AppSettings</param>
        private static void ReadProperties(AppSettingsSection appSettings)
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

        /// <summary>
        /// Максимальное число проверок до вывода окна
        /// </summary>
        public static int MaxChecksCount { get; set; } 

        /// <summary>
        /// Интервал проверки простоя
        /// </summary>
        public static TimeSpan CheckInterval { get; set; }

        /// <summary>
        /// Путь к запущенной dll
        /// </summary>
        public static string RunningAssemblyLocation =>
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
