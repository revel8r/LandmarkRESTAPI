using System;
using System.Data;
using System.Data.SqlClient;

namespace Revel8R.Landmark.DataAccess
{
    public abstract class BaseDataObject
    {
        private SqlDataAdapter adapter = null;

        public BaseDataObject(string tableName)
        {
            this.TableName = tableName;

            this.Table = new DataTable(this.TableName);
        }

        protected DataTable Table { get; set; }

        protected SqlDataAdapter Adapter
        {
            get
            {
                if (this.adapter == null)
                {
                    this.adapter = new SqlDataAdapter(this.SelectCommandText, DataContext.Instance.Connection);
                }

                return this.adapter;
            }
        }

        protected virtual string SelectCommandText { get; }

        protected virtual string InsertCommandText { get; }

        protected virtual string UpdateCommandText { get; }

        private string TableName { get; set; }

        protected virtual void AddTableColumns()
        {
            throw new NotImplementedException();
        }
    }
}
