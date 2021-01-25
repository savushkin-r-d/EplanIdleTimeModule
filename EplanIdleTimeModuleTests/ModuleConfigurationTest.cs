using Microsoft.VisualStudio.TestTools.UnitTesting;
using IdleTimeModule;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace EplanIdleTimeModuleTests
{
    [TestClass]
    public class ModuleConfigurationTest
    {
        IModuleConfiguration moduleConfiguration;

        [TestInitialize]
        public void SetUp()
        {
            moduleConfiguration = new ModuleConfiguration();
        }

        [TestMethod]
        public void TestDefaultCheckIntervalGet()
        {
            TimeSpan defaultCheckInverval = moduleConfiguration.CheckInterval;
            Assert.AreEqual(DefaultCheckInterval, defaultCheckInverval);
        }

        [TestMethod]
        public void TestDefaultMaxChecksCountGet()
        {
            int defaultMaxChecksCount = moduleConfiguration.MaxChecksCount;
            Assert.AreEqual(DefaultMaxChecksCount, defaultMaxChecksCount);
        }

        [DataTestMethod]
        [DataRow("20","10")]
        [DataRow("10", "20")]
        [DataRow("0", "0")]
        [DataRow("100", "50")]
        [DataRow("abcde", "edcba")]
        public void TestReadCheckIntervalAndMaxChecksCountNoCopyFile(
            string expectedMaxChecksCountStr, string expectedCheckIntervalStr)
        {
            SetUpAppConfig(expectedMaxChecksCountStr, expectedCheckIntervalStr);

            // If file already exist, it will work like file copied.
            moduleConfiguration.Read(string.Empty);

            AssertReadCheckIntervalAndMAxChecksCount(expectedMaxChecksCountStr,
                expectedCheckIntervalStr);
        }

        [DataTestMethod]
        [DataRow("20", "10")]
        [DataRow("10", "20")]
        [DataRow("0", "0")]
        [DataRow("100", "50")]
        [DataRow("abcde", "edcba")]
        public void TestReadCheckIntervalAndMaxChecksCountCopyFile(
            string expectedMaxChecksCountStr, string expectedCheckIntervalStr)
        {
            SetUpAppConfig(expectedMaxChecksCountStr, expectedCheckIntervalStr);

            moduleConfiguration.Read(PathToRunningAssembly);

            AssertReadCheckIntervalAndMAxChecksCount(expectedMaxChecksCountStr,
                expectedCheckIntervalStr);
        }

        private void SetUpAppConfig(string maxChecksCount,
            string checkInterval)
        {
            Configuration idleConfig = ConfigurationManager
                .OpenExeConfiguration(PathToIdleAssembly);
            AppSettingsSection appSettings = idleConfig.AppSettings;

            appSettings.Settings[MaxChecksCountSection].Value = maxChecksCount;
            appSettings.Settings[CheckIntervalSection].Value = checkInterval;
            idleConfig.Save();
        }

        private void AssertReadCheckIntervalAndMAxChecksCount(
            string expectedMaxChecksCountStr, string expectedCheckIntervalStr)
        {
            int actualMaxChecksCount = moduleConfiguration.MaxChecksCount;
            TimeSpan actualCheckInverval = moduleConfiguration.CheckInterval;

            bool parsed = int.TryParse(expectedCheckIntervalStr,
                out int expectedCheckInterval);
            if (parsed)
            {
                var expectedTimeSpan = TimeSpan
                    .FromSeconds(expectedCheckInterval);
                Assert.AreEqual(expectedTimeSpan, actualCheckInverval);
            }
            else
            {
                Assert.AreEqual(DefaultCheckInterval, actualCheckInverval);

            }

            parsed = int.TryParse(expectedMaxChecksCountStr,
                out int expectedMaxChecksCount);
            if (parsed)
            {
                Assert.AreEqual(expectedMaxChecksCount, actualMaxChecksCount);
            }
            else
            {
                Assert.AreEqual(DefaultMaxChecksCount, actualMaxChecksCount);
            }
        }

        private string PathToIdleAssembly => Path
            .Combine(Path.GetDirectoryName(PathToRunningAssembly),
            ModuleAssemblyName);

        private string PathToRunningAssembly => Assembly.GetExecutingAssembly()
            .Location;

        private TimeSpan DefaultCheckInterval => TimeSpan.FromSeconds(60);

        const int DefaultMaxChecksCount = 60;

        const string ModuleAssemblyName =
            "EPLAN.EplAddin.IdleTimeModule.dll";

        const string MaxChecksCountSection = "maxChecksCount";

        const string CheckIntervalSection = "checkIntervalSec";
    }
}
