using System;
using System.Web;
using Quartz;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Net;
using System.Diagnostics;

namespace NYSub.Models
{
    public class Scheduler : IJob
    {
        private bool _remoteFileExists;
        private bool _localFileExists;

        private DateTime _lastUpdate;
        private DateTime FileDate;
        private bool _dataSourceUpdated;
        private string _dataURL = "http://web.mta.info/developers/data/nyct/subway/google_transit.zip";
        private FileInfo _localFile = new FileInfo((HttpRuntime.AppDomainAppId == null ? @"C:\Temp" : HttpRuntime.AppDomainAppPath) + @"\Content\Downloads\stations.zip");

        public void Execute(IJobExecutionContext context)
        {
            FilesExists();
            GetFileDate();
            _lastUpdate = _localFile.CreationTime;
            Debug.WriteLine("Local file date is:" + _lastUpdate);
            _dataSourceUpdated = (_lastUpdate < FileDate);
            if (_dataSourceUpdated) GetFile();
        }

        private void GetFile()
        {
            var exctractfolder = _localFile.DirectoryName.ToString() + @"\exctract\";
            DownloadFile(_localFile.FullName);
            var zip = new FastZip();
            zip.ExtractZip(_localFile.FullName, exctractfolder, "");
        }

        private void DownloadFile(string filename)
        {
            Debug.WriteLine("Downloading remote file");
            if (_remoteFileExists)
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(_dataURL, filename);
                }
            }
            if (!File.Exists(filename)) Debug.WriteLine("Failed to save file.");
        }

        private void FilesExists()
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(_dataURL);
            request.Method = "HEAD";
            _localFileExists = _localFile.Exists;
            try
            {
                response = (HttpWebResponse)(request.GetResponse());
                _remoteFileExists = true;
            }
            catch (WebException)
            {
                _remoteFileExists = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        private void GetFileDate()
        {
            if (!_remoteFileExists) FileDate = _lastUpdate;
            HttpWebRequest File = (HttpWebRequest)WebRequest.Create(_dataURL);
            HttpWebResponse FileResponse = (HttpWebResponse)(File.GetResponse());
            FileDate = FileResponse.LastModified;
        }
    }


}