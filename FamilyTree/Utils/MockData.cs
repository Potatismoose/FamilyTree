using FamilyTree.Database;
using FamilyTree.Person;
using System;
using System.IO;

namespace FamilyTree.Utils
{
    class MockData
    {

        public void PrintText()
        {
            var contents = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, @"../../../../mockdata.txt"));

            foreach (var line in contents)
            {

                if (line[0] == '/' && line[1] == '/')
                {
                    //Do nothing, commented row in file.
                }
                else
                {
                    var split = line.Split(',');
                    var name = split[0];
                    var lastName = split[1];

                    DateTime birthDate = DateTime.ParseExact(split[2], "yyyy-MM-dd", null);
                    DateTime deathDate = DateTime.ParseExact(split[3], "yyyy-MM-dd", null);
                    int.TryParse(split[4], out var motherId);
                    int.TryParse(split[5], out var fatherId);
                    var birthPlace = split[6];
                    var deathPlace = split[7];
                    Crud write = new Crud("FamilyTree");
                    write.AddPerson(new Relative(name, lastName, birthDate, deathDate, motherId, fatherId, (birthPlace, deathPlace)));


                }
            }
        }




    }
}
