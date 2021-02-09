using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FamilyTree.Database
{
    class Crud
    {


        public string ConnectionString { get; set; } = @"Data Source = .\SQLExpress; Integrated Security = true; database = {0}";
        public string DatabaseName { get; set; }

        /// <summary>
        /// Creates a database with the provided string as Database name
        /// </summary>
        /// <param name="databaseName">Provide the name for the database name</param>
        /// <returns>Returns true if database was created successfully, or else returns false </returns>
        public bool CreateDatabase(string databaseName)
        {
            try
            {
                var sql = $"CREATE DATABASE {databaseName}";
                ExecuteSQL(sql);
                return true;
            }
            catch
            {
                return false;
            }
        }









        /// <summary>
        /// Executes SQL query and return number of rows affected as an INT.
        /// </summary>
        /// <param name="sql">Takes an SQL query as a parameter</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSQL(string sql, params (string, string)[] parameters)
        {
            var connectionString = string.Format(ConnectionString, DatabaseName);
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(sql, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }












        public DataTable GetDataTable(string sql, params (string, string)[] parameters)
        {
            var dataTable = new DataTable();
            var connectionString = string.Format(ConnectionString, DatabaseName);
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(sql, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                    }

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }






        /// <summary>
        /// Class that checks the existance of database and tables.
        /// </summary>



        public bool DoesDataBaseExist()
        {
            var crud = new Crud();

            var list = new List<string>();
            var sql = "SELECT name FROM sys.databases";

            var dt = crud.GetDataTable(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(row["name"].ToString());
            }

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.Contains("FamilyTree"))
                    {
                        return true;
                    }

                }
                return false;
            }
            else
            {
                return false;
            }
        }

    }
}
