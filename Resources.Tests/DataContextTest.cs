using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resources.Common.Abstractions;
using Resources.DAL;
using System.Threading.Tasks;

namespace Resources.Tests
{
    [TestClass]
    public class DataContextTest
    {
        [TestMethod]
        public async Task Test()
        {
            IResourcesDataContext dataContext = new ResourcesDataContext();
            await dataContext.InitialDataContext();

            Assert.IsNotNull(dataContext.Records);
            Assert.IsTrue(dataContext.Records.Count > 0);
        }
    }
}
