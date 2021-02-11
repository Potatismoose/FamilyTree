using FamilyTree.Database;
using FamilyTree.Menus;
using FamilyTree.Utils;
using System;
using System.Threading;


namespace FamilyTree
{
    class StartProgram
    {

        /*****************************************************************************
         *                                RUN()
         *****************************************************************************/
        /// <summary>
        /// Method for starting up the program
        /// </summary>
        public void Run()
        {
            if (CheckDatabase())
            {
                Menu menu = new Menu();
                menu.Start();
            }

            else
            {
                Console.WriteLine("\t\tNåt gick snett, avslutar programmet");
                Thread.Sleep(2000);
            }
        }


        /*****************************************************************************
         *                            CHECKDATABASE()
         *****************************************************************************/



        /// <summary>
        /// This method checks if database exists, and if not creates database and tables.
        /// </summary>
        private bool CheckDatabase()
        {
            Crud db = new Crud("FamilyTree");
            Crud dbMaster = new Crud("Master");

            if (!dbMaster.DoesDataBaseExist(dbMaster))
            {
                if (dbMaster.CreateDatabase("FamilyTree"))
                {

                    db.CreateTable(
                        "Persons",
                        "id INT PRIMARY KEY IDENTITY (1,1), " +
                        "firstName NVARCHAR(50), " +
                        "lastName NVARCHAR(50)," +
                        "birthDate DATE," +
                        "deathDate DATE," +
                        "motherId INT," +
                        "fatherId INT");

                    Console.WriteLine("\t\tDatabas skapades, Välkommen till första användningen av Family Tree");
                    Thread.Sleep(2500);
                    MockData mock = new MockData();
                    mock.PrintText();
                    Console.ReadKey();
                    return true;

                }
                else
                {
                    Console.WriteLine("\t\tNågot gick snett när databasen skapades.");
                    Thread.Sleep(1500);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("\t\tVälkommen tillbaka");
                Thread.Sleep(1500);
                return true;
            }


        }
    }
}
