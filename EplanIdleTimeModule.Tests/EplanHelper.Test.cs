using NUnit.Framework;
using IdleTimeModule.EplanAPIHelper;
using Moq;

namespace IdleTimeModuleTests.EplanApiHelperTests
{
    public class EplanHelperTest : EplanHelper
    {

        [SetUp]
        public void Setup()
        {
            var eplanHelperMock = new Mock<IEplanHelper>();
        }

        [Test]
        public void TestGetCurrentProject()
        {
            
            //eplanHelperMock.Setup(x => x.GetCurrentProject()).
        }
    }
}