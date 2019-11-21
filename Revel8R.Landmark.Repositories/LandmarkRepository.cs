using System;
using System.Data;
using Revel8R.BusinessEntities;
using Revel8R.Landmark.BusinessEntities;
using Revel8R.Landmark.DataSource;
using Revel8R.Repositories;

namespace Revel8R.Landmark.Repositories
{
    public class LandmarkRepository : IRepository<LandmarkEntity, LandmarkKey>
    {
        public Result Create(LandmarkEntity entity)
        {
            LandmarksDataAccess dataAccess = new LandmarksDataAccess();

            Result result = 
                dataAccess.Insert(
                    entity.MarkerNumber,
                    entity.Title, 
                    entity.Description, 
                    entity.Country, 
                    entity.StateProvince, 
                    entity.Longitude, 
                    entity.Latitude, 
                    3);

            return result;
        }

        public Result Delete(LandmarkEntity entity)
        {
            throw new NotImplementedException();
        }

        public LandmarkEntity Read(LandmarkKey key)
        {
            throw new NotImplementedException();
        }

        public BaseEntityCollection<LandmarkEntity> ReadAll(LandmarkKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            else if (
                key.SearchTerms == null ||
                key.SearchTerms.Count < 2 ||
                !key.SearchTerms.ContainsKey("country") ||
                !key.SearchTerms.ContainsKey("stateProvince"))
            {
                return this.ReadByGPS(key);
            }
            else
            {
                return ReadByCountryState(key);
            }
        }

        private BaseEntityCollection<LandmarkEntity> ReadByGPS(LandmarkKey key)
        {
            LandmarksDataAccess dataAccess = new LandmarksDataAccess();

            DataRowCollection rows = dataAccess.FindRows(key.Longitude, key.Latitude, key.Distance);

            BaseEntityCollection<LandmarkEntity> result = this.CreateEntities(rows);

            return result;
        }

        private BaseEntityCollection<LandmarkEntity> ReadByCountryState(LandmarkKey key)
        {
            LandmarksDataAccess dataAccess = new LandmarksDataAccess();

            DataRowCollection rows = dataAccess.FindRows(key.SearchTerms["country"], key.SearchTerms["stateProvince"]);

            BaseEntityCollection<LandmarkEntity> result = this.CreateEntities(rows);

            return result;
        }

        public BaseEntityCollection<LandmarkEntity> ReadAll()
        {
            LandmarksDataAccess dataAccess = new LandmarksDataAccess();

            DataRowCollection rows = dataAccess.FindRows();

            BaseEntityCollection<LandmarkEntity> result = this.CreateEntities(rows);

            return result;
        }

        public Result Update(LandmarkEntity entity, LandmarkKey key)
        {
            throw new NotImplementedException();
        }

        private BaseEntityCollection<LandmarkEntity> CreateEntities(DataRowCollection rows)
        {
            BaseEntityCollection<LandmarkEntity> result = null;

            if (rows != null && rows.Count > 0)
            {
                result = new BaseEntityCollection<LandmarkEntity>();

                foreach (DataRow row in rows)
                {
                    LandmarkEntity entity = new LandmarkEntity();

                    entity.LandmarkId = row.Field<long>("LandmarkId");
                    entity.MarkerNumber = row.Field<string>("MarkerNumber");
                    entity.Title = row.Field<string>("Title");
                    entity.Description = row.Field<string>("Description");
                    entity.StateProvince = row.Field<string>("StateProvince");
                    entity.Country = row.Field<string>("Country");

                    entity.Longitude = entity.GetDegreesFromPoint(row.Field<string>("GPSCoord"), true);
                    entity.Latitude = entity.GetDegreesFromPoint(row.Field<string>("GPSCoord"), false);

                    entity.CreatedBy = row.Field<long>("CreatedBy");
                    entity.CreatedDate = row.Field<DateTime>("CreatedDate");

                    result.Add(entity);
                }
            }

            return result;
        }
    }
}
