using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Revel8R.Landmark.DataAccess
{
    public sealed class DataContext
    {
        private static volatile DataContext instance;

        private static object syncRoot = new object();

        private SqlConnection connection;

        private SqlCommand command;

        private DataContext()
        {
            this.connection = new SqlConnection();
            this.connection.ConnectionString = "Data Source=REVEL8R-PC2\\SQLEXPRESS;Initial Catalog=Landmark;Integrated Security=True;Type System Version=SQL Server 2012;";
            this.command = new SqlCommand();
            this.command.Connection = this.connection;
        }

        public static DataContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new DataContext();
                        }
                    }
                }

                return instance;
            }
        }

        public SqlConnection Connection
        {
            get
            {
                return this.connection;
            }
        }

        public SqlCommand Command
        {
            get
            {
                return this.command;
            }
        }
    }
}
