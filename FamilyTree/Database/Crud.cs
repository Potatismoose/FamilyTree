using System.Data;
using System.Data.SqlClient;

namespace FamilyTree.Database
{
    class Crud
    {


        public string ConnectionString { get; set; } = @"Data Source = .\SQLExpress; Integrated Security = true; database = {0}";
        public string DatabaseName { get; set; }



        /*******************************************************************
                            CONSTRUCTOR FOR CRUD CLASS
         *******************************************************************/
        /// <summary>
        /// Constructor for Crud class
        /// </summary>
        /// <param name="databaseName">Sets the active database to the parameter passed in to the constructor.</param>
        public Crud(string databaseName)
        {
            DatabaseName = databaseName;
        }


        /*******************************************************************
                                 CREATEDATABASE()
         *******************************************************************/
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


        /*******************************************************************
                                 EXECUTESQL()
         *******************************************************************/
        /// <summary>
        /// Executes SQL query and return number of rows affected as an INT.
        /// </summary>
        /// <param name="sql">Takes an SQL query as a inparameter</param>
        /// <param name="parameters"></param>
        /// <returns>Returns number of rows affected</returns>
        /// 

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

        /*******************************************************************
                                 GETDATATABLE()
         *******************************************************************/
        /// <summary>
        /// Gets information from the database and stores it in a datatable
        /// </summary>
        /// <param name="sql">The SQL string that will be queried to the database</param>
        /// <param name="parameters"></param>
        /// <returns>Returns a datatable with the query result</returns>
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

        /*******************************************************************
                                 DOESDATABASEEXIST()
         *******************************************************************/
        /// <summary>
        /// Class that checks the existance of database and tables.   
        /// </summary>
        /// <param name="crud">Takes an object of the crud class so the right database will be checked</param>
        /// <returns>Returns true if it exists, false if it´s not existing.</returns>
        public bool DoesDataBaseExist(Crud crud)
        {

            var sql = "SELECT name FROM sys.databases";
            var dt = crud.GetDataTable(sql);

            //Kontrollerar om databasen finns
            foreach (DataRow row in dt.Rows)
            {
                //om databasen finns
                if (row["name"].ToString().Contains("FamilyTree"))
                {
                    return true;
                }
            }

            return false;
        }


        /*******************************************************************
                                 CREATETABLE()
         *******************************************************************/
        /// <summary>
        /// Creates the nessecary table for the program to work
        /// </summary>
        /// <param name="table">Creates the table with this name</param>
        /// <param name="fields">Defines what fields that should be included when created.</param>
        public int CreateTable(string table, string fields)
        {
            var sql = $"CREATE TABLE {table} ({fields})";
            return ExecuteSQL(sql);
        }





    }
}
