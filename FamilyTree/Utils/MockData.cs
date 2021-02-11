using FamilyTree.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FamilyTree.Utils
{
    class MockData
    {

        public void PrintText()
        {

            var contents = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\mockdata.txt");
            List<string> rows = new List<string>();
            foreach (var line in contents)
            {

                Console.WriteLine(line);
                var split = line.Split(',');
                var name = split[0];
                var lastName = split[1];
                var birthDate = split[2];
                var deathDate = split[3];
                var motherId = split[4];
                var fatherId = split[5];

                Console.WriteLine(name);
                Console.WriteLine(lastName);
                Console.WriteLine(birthDate);
                Console.WriteLine(deathDate);
                Console.WriteLine(motherId);
                Console.WriteLine(fatherId);
                if (deathDate == "")
                {
                    if (birthDate == "")
                    {
                        AddMockData(name, lastName, motherId, fatherId);
                    }
                    else
                    {
                        AddMockData(name, lastName, birthDate, motherId, fatherId);
                    }
                }
                else if (birthDate == "")
                {
                    if (deathDate == "")
                    {
                        AddMockData(name, lastName, motherId, fatherId);
                    }
                    else
                    {
                        AddMockData(name, lastName, true, deathDate, motherId, fatherId);
                    }
                }
                else
                {
                    AddMockData(name, lastName, birthDate, deathDate, motherId, fatherId);
                }




            }

        }


        //Complete edition
        private void AddMockData(string name, string lastName, string birthDate, string deathDate, string motherId, string fatherId)
        {

            var sql = $"INSERT INTO Persons " +
                      $"(firstName,lastName,birthDate,deathDate,motherId,fatherId) " +
                      $"VALUES(@fName, @lName, @birthDate, @deathDate, @motherId, @fatherId)";

            Crud write = new Crud("FamilyTree");
            write.ExecuteSQL(sql,
                ("@fName", $"{name}"),
                ("@lName", $"{lastName}"),
                ("@birthDate", $"{birthDate}"),
                ("@deathDate", $"{deathDate}"),
                ("@motherId", $"{motherId}"),
                ("@fatherId", $"{fatherId}"));

        }

        //Dead edition
        private void AddMockData(string name, string lastName, bool dead, string deathDate, string motherId, string fatherId)
        {

            var sql = $"INSERT INTO Persons " +
                      $"(firstName,lastName,deathDate,motherId,fatherId) " +
                      $"VALUES(@fName, @lName, @deathDate, @motherId, @fatherId)";

            Crud write = new Crud("FamilyTree");
            write.ExecuteSQL(sql,
                ("@fName", $"{name}"),
                ("@lName", $"{lastName}"),
                ("@deathDate", $"{deathDate}"),
                ("@motherId", $"{motherId}"),
                ("@fatherId", $"{fatherId}"));

        }

        //Born edition
        private void AddMockData(string name, string lastName, string birthDate, string motherId, string fatherId)
        {

            var sql = $"INSERT INTO Persons " +
                      $"(firstName,lastName,birthDate,motherId,fatherId) " +
                      $"VALUES(@fName, @lName, @birthDate, @motherId, @fatherId)";

            Crud write = new Crud("FamilyTree");
            write.ExecuteSQL(sql,
                ("@fName", $"{name}"),
                ("@lName", $"{lastName}"),
                ("@birthDate", $"{birthDate}"),
                ("@motherId", $"{motherId}"),
                ("@fatherId", $"{fatherId}"));

        }

        //Name edition
        private void AddMockData(string name, string lastName, string motherId, string fatherId)
        {

            var sql = $"INSERT INTO Persons " +
                      $"(firstName,lastName,motherId,fatherId) " +
                      $"VALUES(@fName, @lName, @motherId, @fatherId)";

            Crud write = new Crud("FamilyTree");
            write.ExecuteSQL(sql,
                ("@fName", $"{name}"),
                ("@lName", $"{lastName}"),
                ("@motherId", $"{motherId}"),
                ("@fatherId", $"{fatherId}"));


        }

    }
}
