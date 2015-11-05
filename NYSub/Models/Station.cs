using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NYSub.Models
{
    public class Station
    {
        private float _lon;
        private float _lat;
        private string _id;
        private string _Name;
        private string _parentStationId;

        public float Latitude
        {
            get; set;
        }

        public float Longitude
        {
            get; set;
        }

        public string StationId
        {
            get; set;
        }

        public string StationName
        {
            get; set;
        }

        public string ParentStation
        {
            get; set;
        }
    }
}