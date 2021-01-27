using Microsoft.VisualStudio.TestTools.UnitTesting;
using IdleTimeModule;
using IdleTimeModule.EplanAPIHelper;
using Moq;

namespace EplanIdleTimeModuleTests
{
    [TestClass]
    public class IdleModuleTest
    {
        // IIdleModule - non testable method Start(), because we have
        // multi-threading there.

        [TestInitialize]
        public void SetUp()
        {
            eplanHelperMock = new Mock<IEplanHelper>();
            moduleConfigurationMock = new Mock<IModuleConfiguration>();
            
            runningProcessMock = new Mock<IRunningProcess>();
            runningProcessMock.Setup(x => x.Kill()).Verifiable();
            runningProcessMock.Setup(x => x.Close()).Verifiable();

            eplanProjectMock = new Mock<IProject>();
        }

        [TestMethod]
        public void CloseApp_MockProjectMainWindow_NoInvokeEvent()
        {
            eplanHelperMock.Setup(x => x.GetCurrentProject())
                .Returns(eplanProjectMock.Object);
            idleTimeModule =
                new IdleTimeModule.IdleTimeModule(eplanHelperMock.Object,
                moduleConfigurationMock.Object, runningProcessMock.Object);
            bool isInvokedEvent = false;
            idleTimeModule.BeforeClosingProject +=
                ((bool silentMode) => isInvokedEvent = true);
            runningProcessMock.Setup(x => x.CloseMainWindow()).Returns(true);

            idleTimeModule.CloseApplication();

            eplanHelperMock.Verify(x => x.GetCurrentProject(), Times.Never());
            runningProcessMock.Verify(x => x.CloseMainWindow(), Times.Once());
            runningProcessMock.Verify(x => x.Kill(), Times.Never());
            runningProcessMock.Verify(x => x.Close(), Times.Once());
            Assert.AreEqual(false, isInvokedEvent);
        }

        [TestMethod]
        public void CloseApp_MockProjectNoMainWindow_InvokeEvent()
        {
            eplanHelperMock.Setup(x => x.GetCurrentProject())
                .Returns(eplanProjectMock.Object);
            idleTimeModule = 
                new IdleTimeModule.IdleTimeModule(eplanHelperMock.Object,
                moduleConfigurationMock.Object, runningProcessMock.Object);
            bool isInvokedEvent = false;
            idleTimeModule.BeforeClosingProject +=
                ((bool silentMode) => isInvokedEvent = true);
            runningProcessMock.Setup(x => x.CloseMainWindow()).Returns(false);

            idleTimeModule.CloseApplication();

            eplanHelperMock.Verify(x => x.GetCurrentProject(), Times.Once());
            runningProcessMock.Verify(x => x.CloseMainWindow(), Times.Once());
            runningProcessMock.Verify(x => x.Kill(), Times.Once());
            runningProcessMock.Verify(x => x.Close(), Times.Never());
            Assert.AreEqual(true, isInvokedEvent);
        }

        [TestMethod]
        public void CloseApp_NullProjectMainWindow_NoInvokeEvent()
        {
            eplanHelperMock.Setup(x => x.GetCurrentProject())
                .Returns((IProject)null);
            idleTimeModule =
                new IdleTimeModule.IdleTimeModule(eplanHelperMock.Object,
                moduleConfigurationMock.Object, runningProcessMock.Object);
            bool isInvokedEvent = false;
            idleTimeModule.BeforeClosingProject +=
                ((bool silentMode) => isInvokedEvent = true);
            runningProcessMock.Setup(x => x.CloseMainWindow()).Returns(true);

            idleTimeModule.CloseApplication();

            eplanHelperMock.Verify(x => x.GetCurrentProject(), Times.Never());
            runningProcessMock.Verify(x => x.CloseMainWindow(), Times.Once());
            runningProcessMock.Verify(x => x.Close(), Times.Once());
            runningProcessMock.Verify(x => x.Kill(), Times.Never());
            Assert.AreEqual(false, isInvokedEvent);
        }

        [TestMethod]
        public void CloseApp_NullProjectNoMainWindow_NoInvokeEvent()
        {
            eplanHelperMock.Setup(x => x.GetCurrentProject())
                .Returns((IProject)null);
            idleTimeModule =
                new IdleTimeModule.IdleTimeModule(eplanHelperMock.Object,
                moduleConfigurationMock.Object, runningProcessMock.Object);
            bool isInvokedEvent = false;
            idleTimeModule.BeforeClosingProject +=
                ((bool silentMode) => isInvokedEvent = true);
            runningProcessMock.Setup(x => x.CloseMainWindow()).Returns(false);

            idleTimeModule.CloseApplication();

            eplanHelperMock.Verify(x => x.GetCurrentProject(), Times.Once());
            runningProcessMock.Verify(x => x.CloseMainWindow(), Times.Once());
            runningProcessMock.Verify(x => x.Kill(), Times.Once());
            runningProcessMock.Verify(x => x.Close(), Times.Never());
            Assert.AreEqual(false, isInvokedEvent);
        }

        IIdleTimeModule idleTimeModule;

        Mock<IEplanHelper> eplanHelperMock;
        Mock<IModuleConfiguration> moduleConfigurationMock;
        Mock<IRunningProcess> runningProcessMock;
        Mock<IProject> eplanProjectMock;
    }
}
