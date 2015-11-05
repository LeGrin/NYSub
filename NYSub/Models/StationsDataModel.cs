using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Threading;

namespace NYSub.Models
{
    public class StationsDataModel
    {
        public List<Station> Stations
        {
            get { return _stations; } 
            set { _stations = value; } 
        }

        private List<Station> _stations;
        private FileInfo _localFile = new FileInfo((HttpRuntime.AppDomainAppId == null ? @"C:\Temp" : HttpRuntime.AppDomainAppPath) + @"\Content\Downloads\stations.zip");


        public StationsDataModel()
        {
            if (Stations == null) GetStations();
        }


        public Dictionary<string, float[]> LookupStations()
        {
            if (Stations == null) throw new Exception("no data found");
            var uniquStations = Stations.GroupBy(test => test.StationName)
                   .Select(grp => grp.First().StationName)
                   .ToList();
            var result = new Dictionary<string, float[]>();
            foreach (var station in uniquStations)
            {
                result[station] = new float[2] { Stations.Where(x => x.StationName == station).Select(x => x.Latitude).FirstOrDefault(),
                                                 Stations.Where(x => x.StationName == station).Select(x => x.Longitude).FirstOrDefault(),
                };
            }
            return result;
        }

        private void GetStations()
        {
            if (!_localFile.Exists)
            {
                JobScheduler.ExecuteNow();
                while (!_localFile.Exists)
                {
                    _localFile.Refresh();
                    Thread.Sleep(50);
                }
            }
            var exctractfolder = _localFile.DirectoryName.ToString() + @"\exctract\";
            string line;
            StreamReader file =
               new StreamReader(exctractfolder + "stops.txt");
            file.ReadLine(); //skip header
            _stations = new List<Station>();
            while ((line = file.ReadLine()) != null)
            {
                string[] entries = line.Split(',');
                var station = GetStation(entries);
                if (!_stations.Contains(station))
                    _stations.Add(GetStation(entries));
            }
            file.Close();
        }
        private Station GetStation(string[] entries)
        {
            var station = new Station()
            {
                StationId = entries[0],
                StationName = entries[2],
                Latitude = float.Parse(entries[4]),
                Longitude = float.Parse(entries[5]),
                ParentStation = entries[9]
            };
            return station;
        }
    }

    
}