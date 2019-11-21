using System.ServiceModel;
using Revel8R.BusinessEntities;
using Revel8R.Landmark.BusinessEntities;

namespace LandmarkServiceLibrary
{
    [ServiceContract]
    public interface ILandmarkService
    {
        [OperationContract]
        BaseEntityCollection<LandmarkEntity> GetLandmarksInStateCountry(string stateProvince);
    }
}