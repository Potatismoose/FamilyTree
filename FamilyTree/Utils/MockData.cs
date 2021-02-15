using FamilyTree.Database;
using FamilyTree.Person;
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
            
            foreach (var line in contents)
            {

                if (line[0] == '/' && line[1] == '/')
                {
                    //Do nothing, commented row in file.
                }
                else
                {
                    Console.WriteLine(line);
                    var split = line.Split(',');
                    var name = split[0];
                    var lastName = split[1];
                    
                    DateTime birthDate = DateTime.ParseExact(split[2], "yyyy-MM-dd", null);
                    DateTime deathDate = DateTime.ParseExact(split[3], "yyyy-MM-dd", null);
                    int.TryParse(split[4], out var motherId);
                    int.TryParse(split[5], out var fatherId);
                    var birthPlace = split[6];
                    var deathPlace = split[7];

                    AddMockData(new Relative(name, lastName, birthDate, deathDate, motherId, fatherId, (birthPlace, deathPlace)));
                }
            }
        }


        //Complete edition
        private void AddMockData(Relative person)
        {

            var sql = $"INSERT INTO Persons " +
                      $"(firstName,lastName,birthDate,deathDate,motherId,fatherId, birthPlace, deathPlace) " +
                      $"VALUES(@fName, @lName, @birthDate, @deathDate, @motherId, @fatherId, @birthPlace, @deathPlace)";

            Crud write = new Crud("FamilyTree");
            write.ExecuteSQL(sql,
                ("@fName", $"{person.FirstName}"),
                ("@lName", $"{person.LastName}"),
                ("@birthDate", $"{person.BirthDate}"),
                ("@deathDate", $"{person.DeathDate}"),
                ("@motherId", $"{person.MotherId}"),
                ("@fatherId", $"{person.FatherId}"),
                ("@birthPlace", $"{person.BirthPlace}"),
                ("@deathPlace", $"{person.DeathPlace}"));

        }



    }
}
