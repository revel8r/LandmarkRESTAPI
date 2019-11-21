using System;
using System.Collections.Generic;
using System.ServiceModel.Web;
using Revel8R.BusinessEntities;
using Revel8R.Landmark.BusinessEntities;
using Revel8R.Landmark.Repositories;

namespace LandmarkServiceLibrary
{
    public class LandmarkService : ILandmarkService
    {
        private LandmarkRepository repository = null;

        public LandmarkService()
        {
            this.repository = new LandmarkRepository();
        }

        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "data/{stateProvince}")]
        public BaseEntityCollection<LandmarkEntity> GetLandmarksInStateCountry(string stateProvince)
        {
            LandmarkKey key = new LandmarkKey();
            key.SearchTerms = new Dictionary<string, string>();
            key.SearchTerms.Add("USA", stateProvince);

            return this.repository.ReadAll(key);
        }
    }
}