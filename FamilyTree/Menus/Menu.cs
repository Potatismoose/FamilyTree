using FamilyTree.Database;
using FamilyTree.Person;
using FamilyTree.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FamilyTree.Menus
{
    class Menu
    {
        /**************************************************************
                                START()
         **************************************************************/
        /// <summary>
        /// Main menu. Method that prints out and handle the main menu
        /// </summary>
        public void Start()
        {

            List<string> startMenuOptions = new List<string> { "Sök person", "Visa alla personer", "Ändra person", "Lägga till person", "Ta bort en person", "Avsluta programmet" };
            bool error = false;
            string errorMsg = default;
            bool continueCode = false;

            do
            {
                Console.Clear();
                Print.PrintText("Logo");
                Console.WriteLine(Environment.NewLine);
                for (int i = 0; i < startMenuOptions.Count; i++)
                {
                    Console.WriteLine($"\t\t{i + 1}. {startMenuOptions[i]}");
                }

                if (error)
                {

                    int errorTop = Console.CursorTop;
                    Console.SetCursorPosition(0, errorTop + 2);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\t\t{errorMsg}");
                    Console.ResetColor();
                    errorMsg = default;
                }
                int top = Console.CursorTop;
                if (error)
                {
                    Console.SetCursorPosition(0, top - 3);
                    error = false;
                }

                Console.Write("\n\t\tVälj ett val > ");
                string input = Console.ReadLine();
                int.TryParse(input, out int userChoice);
                switch (userChoice)
                {
                    case 1:
                        SearchMenu();
                        break;
                    case 2:
                        ShowAllPeople();
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        continueCode = true;
                        break;



                    default:
                        errorMsg = "Felaktigt menyval";
                        error = true;
                        break;
                }


            } while (!continueCode);

        }

        /**************************************************************
                                SEARCHMENU()
         **************************************************************/
        /// <summary>
        /// Menu that print out the searchmenu and handle the input
        /// </summary>
        private void SearchMenu()
        {

            List<string> searchMenuOptions = new List<string> { "Sök person:> " };

            bool continueCode = false;

            do
            {
                Console.Clear();
                Print.PrintText("Search");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("\t\tHär kan du söka efter en person, eller tryck 0+enter för att avbryta");
                for (int i = 0; i < searchMenuOptions.Count; i++)
                {
                    Console.Write($"\n\t\t{searchMenuOptions[i]}");
                }


                string input = Console.ReadLine();
                if (int.TryParse(input, out int userChoice))
                {
                    continueCode = true;
                }
                else
                {

                    List<Relative> listOfPersons = new List<Relative>();
                    Crud crud = new Crud("FamilyTree");
                    var dt = crud.GetAllPersons(input);
                    foreach (DataRow row in dt.Rows)
                    {
                        System.Diagnostics.Debug.WriteLine(row["firstName"].ToString());
                        listOfPersons.Add(crud.GetPerson(row));
                    }
                    foreach (var person in listOfPersons)
                    {
                        if (person != null)
                        {
                            ListPerson(dt, crud);
                        }
                    }


                    //var person = 
                    //if (person != null)
                    //{
                    //    crud.ListPerson();
                    //}
                    //else
                    //{
                    //    Console.WriteLine("\t\tIngen person hittades");
                    //    Thread.Sleep(1500);
                    //}
                }


            } while (!continueCode);

        }




        /**************************************************************
                                SHOWALLPEOPLE() - PRIVATE
         **************************************************************/
        /// <summary>
        /// Method for listing all people (main menu choice 2). Calls the GetAllPersons in the Crud class to do the search in DB.
        /// Then listing them by calling ListPerson class.
        /// </summary>
        private void ShowAllPeople()
        {

            Crud crud = new Crud("FamilyTree");
            Console.Clear();
            Print.PrintText("List");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("\t\tHär listas alla personer i databasen.\n\t\tTryck 0+enter för att avbryta");



            var dt = crud.GetAllPersons();
            Console.WriteLine(Environment.NewLine);
            ListPerson(dt, crud);




        }

        /*******************************************************************
                                LISTPERSON() - PRIVATE
         *******************************************************************/
        /// <summary>
        /// Takes an datatable and an instance of the CRUD class and creates a list of peoples from the rows in datatable.
        /// Printing them out by calling on PrintListOfPersons.
        /// </summary>
        /// <param name="dt">Takes an DataTable as inparameter</param>
        /// <param name="crud">Takes an instance of Crud class as inparameter</param>
        private void ListPerson(DataTable dt, Crud crud)
        {
            List<int> fullIdList = new List<int>();
            List<Relative> persons = new List<Relative>();
            foreach (DataRow row in dt.Rows)
            {
                persons.Add(crud.GetPerson(row));
                fullIdList.Add(Convert.ToInt32(row["id"]));
            }
            PrintListOfPersons(persons, fullIdList);
        }


        /*******************************************************************
                                PRINTLISTOFPERSONS() - PRIVATE
         *******************************************************************/
        /// <summary>
        /// Prints out the list of persons created by the ListPerson method.
        /// </summary>
        /// <param name="persons">Takes an list with Relatives</param>
        /// <param name="fullIdList">Takes a list with integer ID:s</param>
        private void PrintListOfPersons(List<Relative> persons, List<int> fullIdList)
        {
            foreach (var person in persons)
            {


                string name = $"ID: {person.Id}. {person.FirstName} {person.LastName}";
                Console.Write($"\t\t{name.PadRight(30, ' ')}  ");
                if (person.BirthDate == null)
                {
                    Console.Write("Födelsedag ej angiven,");
                }
                else
                {
                    Console.Write($"född {person.BirthDate.Value.ToString("d")},");
                }
                if (person.DeathDate == null)
                {
                    Print.Green(" Lever");
                }
                else
                {
                    Print.Red($" Avliden {person.DeathDate.Value.ToString("d")}");
                }
            }

            bool continueCode = false;
            bool error = false;
            string errorMsg = default;
            do
            {
                Console.Write("\t\t");
                int top = 0;
                int left = 0;
                int leftTemp = 0;

                if (error)
                {
                    top = Console.CursorTop - 1;
                    left = Console.CursorLeft;
                    Console.SetCursorPosition(left, top);
                    Console.WriteLine(new string(' ', 40));
                    Console.SetCursorPosition(left, top);
                    Console.Write("Ange id på person >");
                    top = Console.CursorTop;
                    leftTemp = Console.CursorLeft;
                    Console.SetCursorPosition(left, top + 1);
                    error = false;
                    Console.WriteLine(new string(' ', 40));
                    Console.SetCursorPosition(left, top + 1);
                    Print.Red(errorMsg);
                    Console.SetCursorPosition(leftTemp, top);

                }
                else
                {
                    Console.WriteLine();
                    Console.Write("\t\tAnge id på person >");
                }


                string input = Console.ReadLine();
                if (int.TryParse(input, out int userChoice))
                {

                    if (userChoice == 0)
                    {
                        continueCode = true;
                    }
                    else if (fullIdList.Contains(userChoice))
                    {
                        ListPersonDetails(userChoice, persons);
                    }
                    else
                    {
                        error = true;
                        errorMsg = "Felaktigt Id, försök igen.";
                    }
                }
                else
                {
                    error = true;
                    errorMsg = "Felaktig inmatning";


                }

            } while (!continueCode);
        }

        /*******************************************************************
                                LISTPERSONDETAILS() - PRIVATE
         *******************************************************************/

        private void ListPersonDetails(int userChoice, List<Relative> persons)
        {

            foreach (var person in persons.Where(x => x.Id == userChoice))
            {
                Console.WriteLine();
                Console.WriteLine("\t\t" + new string('═', 45));
                Print.Blue($"\t\t{person.FirstName} {person.LastName}\t\t");
                Console.WriteLine("\t\t" + new string('═', 45));

                if (person.DeathDate == null)
                {
                    Console.Write("\t\tStatus: ");
                    Print.Green("Lever");

                }
                else
                {
                    Console.Write("\t\tStatus: ");
                    Print.RedW("Avliden ");
                    Console.WriteLine($"i {person.DeathPlace}");
                }
                if (person.BirthDate.Value.ToShortDateString() != "0001-01-01" || person.BirthDate != null)
                {
                    Console.Write($"\t\tFödelseinformation: {person.BirthDate.Value.ToShortDateString()}");
                    if (person.BirthPlace != null)
                    {
                        Console.Write(" född i ");
                        Console.WriteLine($"{person.BirthPlace}");
                    }
                    else
                    {
                        Console.WriteLine(" Födelseplats ej specificerad");
                    }
                }
                else
                {
                    if (person.BirthPlace != null)
                    {
                        Console.Write(" född i ");
                        Print.Green($"{person.BirthPlace}");
                    }
                    else
                    {
                        Console.WriteLine(" Födelsedatum ej specificerat");
                    }
                }

                if (person.MotherId != 0 || person.FatherId != 0)
                {
                    Console.WriteLine("\t\t" + new string('═', 10));
                    Print.DarkGrey("\t\tFöräldrar");
                    Console.WriteLine("\t\t" + new string('═', 10));
                    Console.WriteLine("\t\tMamma: ");
                    Console.Write("\t\tPappa: ");
                }
            }
        }
    }
}
