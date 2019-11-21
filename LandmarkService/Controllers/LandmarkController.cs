using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Revel8R.BusinessEntities;
using Revel8R.Landmark.BusinessEntities;
using Revel8R.Landmark.Repositories;

namespace LandmarkService.Controllers
{
    public class LandmarkController : ApiController
    {
        private LandmarkRepository repository;

        // GET: api/Landmark
        public BaseEntityCollection<LandmarkEntity> Get()
        {
            this.repository = new LandmarkRepository();

            BaseEntityCollection<LandmarkEntity> allLandmarks = this.repository.ReadAll();

            return allLandmarks;
        }

        // GET: api/Landmark/?longitude=38.852858&latitude=-77.128800&distance=5
        public LandmarkEntity[] Get(decimal longitude, decimal latitude, int distance)
        {
            this.repository = new LandmarkRepository();

            LandmarkKey key = new LandmarkKey();
            key.Longitude = longitude;
            key.Latitude = latitude;
            key.Distance = distance;

            BaseEntityCollection<LandmarkEntity> allLandmarks =
                this.repository.ReadAll(key);

            LandmarkEntity[] result = null;

            if (allLandmarks != null)
            {
                result = new LandmarkEntity[allLandmarks.Count];

                for (int i = 0; i < allLandmarks.Count; i++)
                {
                    result[i] = allLandmarks.ElementAt(i);
                }
            }
            else
            {
                result = new LandmarkEntity[1];

                result[0] = new LandmarkEntity();
                result[0].Title = "Empty Result";
            }


            return result;
        }

        // GET: api/Landmark/?country=USA&state=Virginia
        public BaseEntityCollection<LandmarkEntity> Get(string country, string state)
        {
            LandmarkKey key = new LandmarkKey();
            key.SearchTerms = new Dictionary<string, string>();
            key.SearchTerms.Add("country", country);
            key.SearchTerms.Add("stateProvince", state);

            this.repository = new LandmarkRepository();

            BaseEntityCollection<LandmarkEntity> landmarks = this.repository.ReadAll(key);

            return landmarks;
        }

        // POST: api/Landmark
        public Result Post([FromBody]LandmarkEntity newLandmark)
        {
            this.repository = new LandmarkRepository();

            Result result = this.repository.Create(newLandmark);

            return result;
        }

        // PUT: api/Landmark/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/Landmark/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
