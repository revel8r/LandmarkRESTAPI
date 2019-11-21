using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revel8R.BusinessEntities;

namespace Revel8R.Landmark.BusinessEntities
{
    public class LandmarkKey : BaseKey
    {
        public LandmarkKey()
        {
        }

        public LandmarkKey(decimal latitude, decimal longitude, int distance)
        {
            this.Longitude = longitude;

            this.Latitude = latitude;

            this.Distance = distance;
        }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int Distance { get; set; }
    }
}
