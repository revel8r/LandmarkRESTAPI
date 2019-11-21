using Microsoft.SqlServer.Types;
using System;
using System.Data;
using System.Data.Spatial;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Revel8R.BusinessEntities;
using Revel8R.Landmark.DataAccess;

namespace Revel8R.Landmark.DataSource
{
    public class LandmarksDataAccess : BaseDataObject
    {
        private enum SelectOption
        {
            none,
            byCountryStateProvince,
            byLongitudeLatitude
        }

        private SelectOption SelectBy { get; set; }

        public LandmarksDataAccess()
            : base("Landmarks")
        {
        }

        protected override void AddTableColumns()
        {
            DataColumn landmarkId = new DataColumn("LandmarkId", typeof(Int64));
            landmarkId.DefaultValue = 0;
            this.Table.Columns.Add(landmarkId);

            DataColumn markerNumber = new DataColumn("MarkerNumber", typeof(string));
            markerNumber.DefaultValue = string.Empty;
            this.Table.Columns.Add(markerNumber);

            DataColumn title = new DataColumn("Title", typeof(string));
            title.DefaultValue = string.Empty;
            this.Table.Columns.Add(title);

            DataColumn description = new DataColumn("Description", typeof(string));
            description.DefaultValue = string.Empty;
            this.Table.Columns.Add(description);

            DataColumn country = new DataColumn("Country", typeof(string));
            description.DefaultValue = string.Empty;
            this.Table.Columns.Add(country);

            DataColumn stateProvince = new DataColumn("StateProvince", typeof(string));
            description.DefaultValue = string.Empty;
            this.Table.Columns.Add(stateProvince);

            DataColumn gpsCoord = new DataColumn("GPSCoord", typeof(string));
            gpsCoord.DefaultValue = string.Empty;
            this.Table.Columns.Add(gpsCoord);

            DataColumn createdBy = new DataColumn("CreatedBy", typeof(long));
            createdBy.DefaultValue = 0;
            this.Table.Columns.Add(createdBy);

            DataColumn createdDate = new DataColumn("CreatedDate", typeof(DateTime));
            createdDate.DefaultValue = DateTime.MinValue;
            this.Table.Columns.Add(createdDate);
        }

        public DataRowCollection FindRows(string country, string stateProvince)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(country))
                {
                    throw new ArgumentNullException("country");
                }

                if (string.IsNullOrWhiteSpace(stateProvince))
                {
                    throw new ArgumentNullException("stateProvince");
                }

                SqlCommand command = DataContext.Instance.Command;
                this.SelectBy = SelectOption.byCountryStateProvince;
                command.CommandText = this.SelectCommandText;
                command.Parameters.Clear();
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("country", SqlDbType.VarChar));
                command.Parameters["country"].Value = country;
                command.Parameters.Add(new SqlParameter("stateProvince", SqlDbType.VarChar));
                command.Parameters["stateProvince"].Value = stateProvince;

                this.Adapter.SelectCommand = command;
                this.Adapter.Fill(this.Table);

                return this.Table.Rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRowCollection FindRows(decimal longitude, decimal latitude, int distance)
        {
            try
            {
                SqlCommand command = DataContext.Instance.Command;
                this.SelectBy = SelectOption.byLongitudeLatitude;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = this.SelectCommandText;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("longitude", SqlDbType.Decimal));
                command.Parameters["longitude"].Value = longitude;
                command.Parameters.Add(new SqlParameter("latitude", SqlDbType.Decimal));
                command.Parameters["latitude"].Value = latitude;
                command.Parameters.Add(new SqlParameter("distance", SqlDbType.VarChar));
                command.Parameters["distance"].Value = distance;

                this.Adapter.SelectCommand = command;
                this.Adapter.Fill(this.Table);
                this.Adapter.SelectCommand.Connection.Close();

                return this.Table.Rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRowCollection FindRows()
        {
            SqlCommand command = DataContext.Instance.Command;
            this.SelectBy = SelectOption.none;
            command.CommandText = this.SelectCommandText;

            this.Adapter.SelectCommand = command;
            this.Adapter.Fill(this.Table);

            return this.Table.Rows;
        }

        public Result Insert(
            string markerNumber,
            string title,
            string description,
            string country,
            string stateProvince,
            decimal longitude,
            decimal latitude,
            int createdBy)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    throw new ArgumentNullException("title");
                }

                if (string.IsNullOrWhiteSpace(description))
                {
                    throw new ArgumentNullException("description");
                }

                if (string.IsNullOrWhiteSpace(country))
                {
                    throw new ArgumentNullException("country");
                }

                if (string.IsNullOrWhiteSpace(stateProvince))
                {
                    throw new ArgumentNullException("stateProvince");
                }
                
                if (longitude == 0)
                {
                    throw new ArgumentNullException("longitude");
                }

                if (latitude == 0)
                {
                    throw new ArgumentNullException("latitude");
                }

                if (createdBy == 0)
                {
                    throw new ArgumentNullException("createdBy");
                }

                SqlCommand command = DataContext.Instance.Command;
                command.CommandText = this.InsertCommandText;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("markerNumber", SqlDbType.VarChar));
                command.Parameters["markerNumber"].Value = markerNumber;
                command.Parameters.Add(new SqlParameter("title", SqlDbType.VarChar));
                command.Parameters["title"].Value = title;
                command.Parameters.Add(new SqlParameter("description", SqlDbType.VarChar));
                command.Parameters["description"].Value = description;
                command.Parameters.Add(new SqlParameter("country", SqlDbType.VarChar));
                command.Parameters["country"].Value = country;
                command.Parameters.Add(new SqlParameter("stateProvince", SqlDbType.VarChar));
                command.Parameters["stateProvince"].Value = stateProvince;

                string gpsCoords = PointFromLongitudeLatitude(longitude, latitude);
                command.Parameters["gpsCoords"].Value = gpsCoords;
                command.Parameters.Add(new SqlParameter("createdBy", SqlDbType.VarChar));
                command.Parameters["createdBy"].Value = createdBy;

                this.Adapter.InsertCommand = command;
                this.Adapter.InsertCommand.ExecuteNonQuery();

                return new Result(true);
            }
            catch (Exception ex)
            {
                Result result = new Result(false);
                result.Message = ex.Message;

                return result;
            }
        }

        protected override string SelectCommandText
        {
            get
            {
                if (this.SelectBy == SelectOption.byCountryStateProvince)
                {
                    return "select LandmarkId, MarkerNumber, Title, Description, Country, StateProvince, GPSCoord.ToString() as GPSCoord, CreatedBy, CreatedDate from Landmarks where Country = @country and StateProvince = @stateProvince;";
                }
                else if (this.SelectBy == SelectOption.byLongitudeLatitude)
                {
                    // call stored procedure;
                    return "dbo.getLocalLandmarks";
                }
                else
                {
                    return "select LandmarkId, MarkerNumber, Title, Description, Country, StateProvince, GPSCoord.ToString() as GPSCoord, CreatedBy, CreatedDate from Landmarks;";
                }
            }
        }

        protected override string InsertCommandText
        {
            get
            {
                return "insert into Landmarks values (@markerNumber, @title, @description, @country, @stateprovince, @gpsCoords, @createdby, GETUTCDATE());";
            }
        }

        private string PointFromLongitudeLatitude(decimal longitude, decimal latitude)
        {
            string point = "geography::POINT(" + longitude + ", " + latitude + ")";

            return point;
        }
    }
}
