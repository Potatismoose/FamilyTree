using FamilyTree.Person;
using System;
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

                    foreach (SqlParameter parameter in cmd.Parameters)
                    {
                        if (parameter.Value.ToString() == "" || parameter.Value.ToString() == "0001-01-01 00:00:00")
                        {
                            parameter.Value = DBNull.Value;
                        }
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
                    System.Diagnostics.Debug.WriteLine(sql);
                    foreach (var parameter in parameters)
                    {
                        System.Diagnostics.Debug.WriteLine(parameter.Item1 + " : " + parameter.Item2);
                        cmd.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                    }

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                        if (DatabaseName == "FamilyTree")
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {
                                System.Diagnostics.Debug.WriteLine(row["firstName"].ToString());
                            }

                        }
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


        /*******************************************************************
                                 SEARCHPERSON() - PUBLIC
         *******************************************************************/

        public Relative SearchPerson(int id)
        {
            DataTable dt = new DataTable();
            var sql = $"SELECT * FROM Persons WHERE id = @id";
            dt = GetDataTable(sql, ("@id", id.ToString()));

            if (dt.Rows.Count > 0)
            {
                return GetPerson(dt.Rows[0]);
            }
            else
                return null;



        }



        /*******************************************************************
                                 SEARCHPERSONLIKE() - PUBLIC
         *******************************************************************/

        //public Relative SearchPersonLike()
        //{
        //    DataTable dt = new DataTable();
        //    var sql = $"SELECT * FROM Persons WHERE id = @id";
        //    dt = GetDataTable(sql, ("@id", id.ToString()));


        //    return GetPerson(dt.Rows[0]);


        //}

        /*******************************************************************
                                 LISTPERSON() - PUBLIC


        NOT IMPLEMENTED YET
         *******************************************************************/
        // TODO Implementera denna metod
        public DataTable ListPerson()
        {

            return null;
        }





        /*******************************************************************
                                 GETALLPERSONS() - PUBLIC
         *******************************************************************/

        /// <summary>
        /// Method for searching database for 0 up to 2 keywords (firstname or lastname)
        /// </summary>
        /// <param name="searchString">Optional to include search parameter. If no parameter is included, the search will return everyone in database.</param>
        /// <returns>Returns the datatable with 0 or more rows depending on searchresult</returns>
        public DataTable GetAllPersons(string searchString = null)
        {
            string sql = "SELECT * FROM Persons ";
            var fName = default(string);
            var lName = default(string);
            if (searchString != null)
            {

                var split = searchString.Split(' ');

                if (split.Length > 1)
                {
                    fName = split[0];
                    lName = split[1];
                    sql += "WHERE firstName LIKE @fName OR lastName LIKE @lName ORDER BY firstName";
                    return GetDataTable(sql, ("@fName", $"%{fName}%"), ("@lName", $"%{lName}%"));
                }
                else
                {
                    fName = split[0];
                    sql += "WHERE firstName LIKE @fName ORDER BY firstName";
                    return GetDataTable(sql, ("@fName", $"%{fName}%"));
                }
            }
            sql += "ORDER BY firstName";
            return GetDataTable(sql);

        }



        /*******************************************************************
                                 GETPERSON() - PUBLIC
         *******************************************************************/
        /// <summary>
        /// Creates one person from the Relative class. 
        /// </summary>
        /// <param name="row">Takes an DataRow as inparameter</param>
        /// <returns>Returns a person object of the Relative class</returns>
        public Relative GetPerson(DataRow row)
        {
            var person = new Relative()
            {
                Id = (int)row["id"],
                FirstName = row["firstName"].ToString(),
                LastName = row["lastName"].ToString(),
                MotherId = (int)row["motherId"],
                FatherId = (int)row["fatherId"]

            };

            if (!(row["birthDate"] is DBNull))
            {
                person.BirthDate = (DateTime)row["birthDate"];
            }
            if (!(row["birthPlace"] is DBNull))
            {
                person.BirthPlace = row["birthPlace"].ToString();
            }
            if (!(row["deathDate"] is DBNull))
            {
                person.DeathDate = (DateTime)row["deathDate"];
            }
            if (!(row["deathPlace"] is DBNull))
            {
                person.DeathPlace = row["deathPlace"].ToString();
            }

            return person;
        }




    }
}
