using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace NYSub.Tests
{
    [TestClass]
    public class StationModelTest
    {

        [TestMethod]
        public void TestGetFileAndDecompress()
        {
            var datamodel = new Models.StationsDataModel();
            var count = datamodel.Stations.Count();
            Debug.Write(count);
            Assert.IsTrue(count > 1);
        }

        [TestMethod]
        public void TestGetLookup()
        {
            var datamodel = new Models.StationsDataModel();
            Assert.IsNotNull(datamodel.LookupStations().FirstOrDefault());
            Debug.Write(datamodel.LookupStations().FirstOrDefault());
        }
    }
}
