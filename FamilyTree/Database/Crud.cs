using FamilyTree.Person;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FamilyTree.Database
{
    class Crud
    {

        /// <summary>
        /// Constructor for Crud class
        /// </summary>
        /// <param name="databaseName">Sets the active database to the parameter passed in to the constructor.</param>
        public Crud(string databaseName)
        {
            /*******************************************************************
                            CONSTRUCTOR FOR CRUD CLASS
            *******************************************************************/
            DatabaseName = databaseName;
        }

        public string ConnectionString { get; set; } = @"Data Source = .\SQLExpress; Integrated Security = true; database = {0}";
        public string DatabaseName { get; set; }


        /// <summary>
        /// Method for adding a person to the database.
        /// </summary>
        /// <param name="person">Takes an instance of the relative class as an in parameter</param>
        /// <returns>Returns the number of rows affected</returns>
        public int AddPerson(Relative person)
        {
            /*******************************************************************
                            ADDPERSON() - PUBLIC
            *******************************************************************/
            var sql = $"INSERT INTO Persons " +
                      $"(firstName,lastName,birthDate,deathDate,motherId,fatherId, birthPlace, deathPlace) " +
                      $"VALUES(@fName, @lName, @birthDate, @deathDate, @motherId, @fatherId, @birthPlace, @deathPlace)";


            return ExecuteSQL(sql,
                ("@fName", $"{person.FirstName}"),
                ("@lName", $"{person.LastName}"),
                ("@birthDate", $"{person.BirthDate}"),
                ("@deathDate", $"{person.DeathDate}"),
                ("@motherId", $"{person.MotherId}"),
                ("@fatherId", $"{person.FatherId}"),
                ("@birthPlace", $"{person.BirthPlace}"),
                ("@deathPlace", $"{person.DeathPlace}"));
        }

        /// <summary>
        /// Creates a database with the provided string as Database name
        /// </summary>
        /// <param name="databaseName">Provide the name for the database name</param>
        /// <returns>Returns true if database was created successfully, or else returns false </returns>
        public bool CreateDatabase(string databaseName)
        {
            /*******************************************************************
                                 CREATEDATABASE()
            *******************************************************************/
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
        /// Creates one person from the Relative class. 
        /// </summary>
        /// <param name="row">Takes an DataRow as inparameter.</param>
        /// <returns>Returns a person object of the Relative class.</returns>
        public Relative CreatePersonFromQuery(DataRow row)
        {
            /*******************************************************************
                            CREATEPERSONFROMQUERY() - PUBLIC
            *******************************************************************/
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

        /// <summary>
        /// Creates the nessecary table for the program to work
        /// </summary>
        /// <param name="table">Creates the table with this name</param>
        /// <param name="fields">Defines what fields that should be included when created.</param>
        /// <returns>Returns number of rows affected in the database</returns>
        public int CreateTable(string table, string fields)
        {
            /*******************************************************************
                                 CREATETABLE()
            *******************************************************************/
            var sql = $"CREATE TABLE {table} ({fields})";
            return ExecuteSQL(sql);
        }

        /// <summary>
        /// Class that checks the existance of database and tables.   
        /// </summary>
        /// <param name="crud">Takes an object of the crud class so the right database will be checked</param>
        /// <returns>Returns true if it exists, false if it´s not existing.</returns>
        public bool DoesDataBaseExist(Crud crud)
        {
            /*******************************************************************
                                 DOESDATABASEEXIST()
            *******************************************************************/
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

        /// <summary>
        /// Method for updating a person in the database.
        /// </summary>
        /// <param name="person">Takes a Person (relative) object as argument.</param>
        /// <returns>Returns number of rows affected in the database.</returns>
        public int EditPerson(Relative person)
        {
            /*******************************************************************
                            EDITPERSON() - PUBLIC
            *******************************************************************/

            var sql = $"UPDATE Persons " +
                      $"SET firstName = @fname," +
                      $"lastName = @lName," +
                      $"birthDate = @birthDate," +
                      $"deathDate = @deathDate," +
                      $"motherId = @motherId," +
                      $"fatherId = @fatherId," +
                      $"birthPlace = @birthPlace, " +
                      $"deathPlace = @deathPlace " +
                      $"WHERE id = @id";


            return ExecuteSQL(sql,
                ("@fName", $"{person.FirstName}"),
                ("@lName", $"{person.LastName}"),
                ("@birthDate", $"{person.BirthDate}"),
                ("@deathDate", $"{person.DeathDate}"),
                ("@motherId", $"{person.MotherId}"),
                ("@fatherId", $"{person.FatherId}"),
                ("@birthPlace", $"{person.BirthPlace}"),
                ("@deathPlace", $"{person.DeathPlace}"),
                ("@id", $"{person.Id}"));
        }

        /// <summary>
        /// Method for searching database for 0 up to 2 keywords (firstname or lastname).
        /// </summary>
        /// <param name="searchString">Optional to include search parameter. If no parameter is included, the search will return everyone in database.</param>
        /// <returns>Returns the datatable with 0 or more rows depending on searchresult.</returns>
        public DataTable GetAllPersons(string searchString = null)
        {
            /*******************************************************************
                                 GETALLPERSONS() - PUBLIC
            *******************************************************************/
            string sql = "SELECT * FROM Persons ";
            var nameOrCityOrDate = default(string);
            var lName = default(string);
            if (searchString != null)
            {

                var split = searchString.Split(' ');

                if (split.Length > 1)
                {
                    nameOrCityOrDate = split[0];
                    lName = split[1];
                    sql += "WHERE firstName LIKE @fName OR lastName LIKE @lName ORDER BY firstName";
                    return GetDataTable(sql, ("@fName", $"%{nameOrCityOrDate}%"), ("@lName", $"%{lName}%"));
                }
                else
                {
                    nameOrCityOrDate = split[0];
                    sql += "WHERE firstName LIKE @fName OR lastName LIKE @lName OR birthPlace LIKE @bPlace OR birthDate LIKE @bDate ORDER BY firstName";
                    return GetDataTable(sql, ("@fName", $"%{nameOrCityOrDate}%"), ("@lName", $"%{nameOrCityOrDate}%"), ("bPlace", $"%{nameOrCityOrDate}%"), ("bDate", $"%{nameOrCityOrDate}%"));
                }
            }
            sql += "ORDER BY firstName";
            return GetDataTable(sql);

        }

        /// <summary>
        /// Gets information from the database and stores it in a datatable
        /// </summary>
        /// <param name="sql">The SQL string that will be queried to the database</param>
        /// <param name="parameters"></param>
        /// <returns>Returns a datatable with the query result</returns>
        public DataTable GetDataTable(string sql, params (string, string)[] parameters)
        {
            /*******************************************************************
                                 GETDATATABLE()
            *******************************************************************/
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

                    }


                }
            }
            return dataTable;
        }

        /// <summary>
        /// Gets the last added database post
        /// </summary>
        /// <returns>Returns the ID of the last added databasepost</returns>
        public string GetLastAddedDatabasePost()
        {
            /*******************************************************************
                            GETLASTADDEDDATABASEPOST() - PUBLIC
            *******************************************************************/
            var sql = "SELECT id FROM Persons " +
                "WHERE ID = (SELECT IDENT_CURRENT('Persons'))";

            var dt = GetDataTable(sql);

            DataRow dr = dt.Rows[0];
            return dr["id"].ToString();
        }

        /// <summary>
        /// Method for getting the parents of a person and return them in a list.
        /// </summary>
        /// <param name="searchForTheseParents">List of perent ID:s.</param>
        /// <returns>returns a list of parents.</returns>
        public List<Relative> GetParents(List<int> searchForTheseParents)
        {
            /*******************************************************************
                            GETPARENTS() - PUBLIC
            *******************************************************************/
            for (int i = 0; i < searchForTheseParents.Count; i++)
            {
                if (searchForTheseParents[i] == 0)
                {
                    searchForTheseParents.RemoveAt(i);
                }
            }
            List<Relative> listOfMatchingParents = new List<Relative>();
            var sql = "SELECT * from Persons WHERE id = @id ";
            if (searchForTheseParents.Count > 1)
            {
                sql += "OR id = @id2";
                var dt = GetDataTable(sql, ("@id", $"{searchForTheseParents[0]}"), ("@id2", $"{searchForTheseParents[1]}"));
                foreach (DataRow row in dt.Rows)
                {
                    var person = CreatePersonFromQuery(row);
                    listOfMatchingParents.Add(person);
                }
            }
            else
            {
                var dt = GetDataTable(sql, ("@id", $"{searchForTheseParents[0]}"));
                var person = CreatePersonFromQuery(dt.Rows[0]);
                listOfMatchingParents.Add(person);
            }
            return listOfMatchingParents;
        }

        /// <summary>
        /// Method for finding a persons siblings. Takes a List of parent ID:s as argument.
        /// </summary>
        /// <param name="searchForTheseParents">List of parent ID:S.</param>
        /// <param name="personId">The person ID of the person whom´s siblings you are trying to find.</param>
        /// <returns>Returns a list of siblings.</returns>
        public List<Relative> GetSiblings(List<int> searchForTheseParents, int personId = 0)
        {
            /*******************************************************************
                            GETSIBLINGS() - PUBLIC
            *******************************************************************/
            List<Relative> listOfMatchingParents = new List<Relative>();

            if (searchForTheseParents.Count >= 1)
            {

                if (searchForTheseParents.Count > 1)
                {
                    var sql = "SELECT * from Persons " +
                        "WHERE id != @personId " +
                        "AND motherId = @id " +
                        "AND motherId != 0 " +
                        "OR id != @personId " +
                        "AND fatherId = @id2 " +
                        "AND fatherId != 0";

                    var dt = GetDataTable(sql, ("@id", $"{searchForTheseParents[0]}"), ("@personId", $"{personId}"), ("@id2", $"{searchForTheseParents[1]}"));
                    foreach (DataRow row in dt.Rows)
                    {
                        listOfMatchingParents.Add(CreatePersonFromQuery(row));
                    }
                }
                else if (searchForTheseParents.Count == 1)
                {
                    var sql = "SELECT * from Persons " +
                        "WHERE id != @personId " +
                        "AND motherId = @id " +
                        "AND motherId != 0 " +
                        "OR id != @personId " +
                        "AND fatherId = @id " +
                        "AND fatherId != 0 ";

                    var dt = GetDataTable(sql, ("@id", $"{searchForTheseParents[0]}"), ("@personId", $"{personId}"));
                    if (personId == 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            listOfMatchingParents.Add(CreatePersonFromQuery(row));
                        }
                    }

                    else
                    {
                        listOfMatchingParents.Add(CreatePersonFromQuery(dt.Rows[0]));
                    }
                }
            }

            else
            {
                return null;
            }

            if (listOfMatchingParents.Count == 0)
            {
                return null;
            }
            return listOfMatchingParents;
        }

        /// <summary>
        /// Removes a person from the database
        /// </summary>
        /// <param name="id">Takes an id (int) of a person as a in parameter</param>
        public void RemovePerson(int id)
        {
            /*******************************************************************
                            REMOVEPERSON() - PUBLIC
            *******************************************************************/
            var sql = "UPDATE Persons SET fatherId = 0 WHERE fatherId = @id";
            ExecuteSQL(sql, ("@id", id.ToString()));
            sql = "UPDATE Persons SET motherId = 0 WHERE motherId = @id";
            ExecuteSQL(sql, ("@id", id.ToString()));
            sql = "DELETE FROM Persons WHERE Id = @id";
            ExecuteSQL(sql, ("@id", id.ToString()));
        }

        /// <summary>
        /// Method for searching a specific ID in the database
        /// </summary>
        /// <param name="id">Takes an integer ID number as in parameter</param>
        /// <returns>returns a created object of the relative class.</returns>
        public Relative SearchPerson(int id)
        {
            /*******************************************************************
                                 SEARCHPERSON() - PUBLIC
            *******************************************************************/
            DataTable dt = new DataTable();
            var sql = $"SELECT * FROM Persons WHERE id = @personId";
            dt = GetDataTable(sql, ("@personId", id.ToString()));

            if (dt.Rows.Count > 0)
            {
                return CreatePersonFromQuery(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Executes SQL query and return number of rows affected as an INT.
        /// </summary>
        /// <param name="sql">Takes an SQL query as a inparameter</param>
        /// <param name="parameters"></param>
        /// <returns>Returns number of rows affected</returns>
        private int ExecuteSQL(string sql, params (string, string)[] parameters)
        {
            /*******************************************************************
                                 EXECUTESQL()
            *******************************************************************/
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
    }
}
