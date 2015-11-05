using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYSub.Tests
{
    [TestClass]
    public class StationModelTest
    {

        [TestMethod]
        public void GetAndDecompress()
        {
            var datamodel = new Models.StationsDataModel();
            var count = datamodel.Stations.Count();
            Debug.Write(count);
            Assert.IsTrue(count > 1);
        }

        [TestMethod]
        public void GetLookup()
        {
            var datamodel = new Models.StationsDataModel();
            Assert.IsNotNull(datamodel.LookupStations().FirstOrDefault());
            Debug.Write(datamodel.LookupStations().FirstOrDefault());
        }
    }
}
