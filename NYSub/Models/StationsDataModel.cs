using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Web;

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
        private bool _dataSourceUpdated;
        private DateTime _lastUpdate;
        private FileInfo _localFile = new FileInfo(HttpRuntime.AppDomainAppPath + @"\Content\Downloads\stations.zip");
        private string _dataURL = "http://web.mta.info/developers/data/nyct/subway/google_transit.zip";
        public DateTime FileDate;
        private bool _fileExists;

        public StationsDataModel()
        {
            FileExists();
            GetFileDate();
            _lastUpdate = _localFile.CreationTime;
            _dataSourceUpdated = (_lastUpdate < FileDate);
            if (Stations == null || _dataSourceUpdated) GetStations();
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
            _lastUpdate = DateTime.Now;
            GetFile();
        }

        private void GetFileDate()
        {
                if (!_fileExists) FileDate = _lastUpdate;
                HttpWebRequest File = (HttpWebRequest)WebRequest.Create(_dataURL);
                HttpWebResponse FileResponse = (HttpWebResponse)(File.GetResponse());
                FileDate = FileResponse.LastModified;
        }

        private void GetFile()
        {
            var exctractfolder = _localFile.DirectoryName.ToString() + @"\exctract\";
            if (_dataSourceUpdated) DownloadFile(_localFile.FullName);
            var zip = new FastZip();
            zip.ExtractZip(_localFile.FullName, exctractfolder, "");

            string line;

            StreamReader file =
               new StreamReader(exctractfolder + "stops.txt");
            file.ReadLine(); //skip header
            _stations = new List<Station>();
            while ((line = file.ReadLine()) != null)
            {
                string[] entries = line.Split(',');
                var station  = GetStation(entries);
                if (!_stations.Contains(station))
                    _stations.Add(GetStation(entries));
            }
            file.Close();
        }

        private void DownloadFile( string filename)
        {
            if (_fileExists)
            {
                using (var client = new WebClient())
                {
                        client.DownloadFileAsync(new Uri(_dataURL), filename);
                }
            }
            if (!File.Exists(filename)) throw new Exception("NO avaliable data");
        }

        private void FileExists()
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(_dataURL);
            request.Method = "HEAD";
            try
            {
                response = (HttpWebResponse)(request.GetResponse());
                _fileExists = true;
            }
            catch (WebException)
            {
                _fileExists = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
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