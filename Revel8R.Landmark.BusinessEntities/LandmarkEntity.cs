using System;
using Revel8R.BusinessEntities;

namespace Revel8R.Landmark.BusinessEntities
{
    public class LandmarkEntity : BaseEntity
    {
        public long LandmarkId { get; set; }

        public string MarkerNumber { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string StateProvince { get; set; }

        public string Country { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal GetDegreesFromPoint(string pointString, bool isLongitude)
        {
            int posSpace = pointString.LastIndexOf(" ");
            int posPoint = "POINT (".Length;

            if (isLongitude)
            {
                string longString = pointString.Substring(posSpace, pointString.Length - posSpace - 1);
                decimal longitude = decimal.Parse(longString);

                return longitude;
            }
            else
            {
                //  Getting Latitude
                string latString = pointString.Substring(posPoint, posSpace - posPoint);
                decimal latitude = decimal.Parse(latString);

                return latitude;
            }
        }
    }
}
