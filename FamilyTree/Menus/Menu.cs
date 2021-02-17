using FamilyTree.Database;
using FamilyTree.Person;
using FamilyTree.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading;

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
                        SearchMenu("Remove");
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
        private void SearchMenu(string showThisLogo = "Search")
        {

            List<string> searchMenuOptions = new List<string> { "Sök person:> " };

            bool continueCode = false;

            do
            {
                Console.Clear();
                Print.PrintText(showThisLogo);
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("\t\tHär kan du söka efter en person, eller tryck 0+enter för att avbryta");
                for (int i = 0; i < searchMenuOptions.Count; i++)
                {
                    Console.Write($"\n\t\t{searchMenuOptions[i]}");
                }

                string input = Console.ReadLine();
                if (showThisLogo == "Search")
                {
                    //If you are searching for a person
                    continueCode = searchInput(continueCode, input);
                }
                else
                {
                    //If you want to delete a person
                    continueCode = searchInput(continueCode, input, true);
                }
            } while (!continueCode);

        }


        /// <summary>
        /// Takes the input from the user search
        /// </summary>
        /// <param name="continueCode">boolean that tells the program to carry on or not</param>
        /// <param name="input">The users input</param>
        /// <returns></returns>
        private bool searchInput(bool continueCode, string input, bool removePerson = false)
        {
            if (int.TryParse(input, out int userChoice))
            {
                switch (userChoice)
                {
                    case 0:
                        continueCode = true;
                        break;
                    default:

                        Crud crud = new Crud("FamilyTree");
                        var dt = crud.GetAllPersons(input);
                        ListPerson(dt, crud, removePerson);
                        break;
                }

            }
            else
            {

                Crud crud = new Crud("FamilyTree");
                var dt = crud.GetAllPersons(input);
                ListPerson(dt, crud, removePerson);
            }

            return continueCode;
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
        private void ListPerson(DataTable dt, Crud crud, bool removePerson = false)
        {
            List<int> fullIdList = new List<int>();
            List<Relative> persons = new List<Relative>();
            foreach (DataRow row in dt.Rows)
            {
                persons.Add(crud.GetPerson(row));
                fullIdList.Add(Convert.ToInt32(row["id"]));
            }
            PrintListOfPersons(persons, fullIdList, removePerson);

        }


        /*******************************************************************
                                PRINTLISTOFPERSONS() - PRIVATE
         *******************************************************************/
        /// <summary>
        /// Prints out the list of persons created by the ListPerson method.
        /// </summary>
        /// <param name="persons">Takes an list with Relatives</param>
        /// <param name="fullIdList">Takes a list with integer ID:s</param>
        private void PrintListOfPersons(List<Relative> persons, List<int> fullIdList, bool removePerson = false)
        {

            foreach (var person in persons)
            {
                const int maxLengthOfId = 2;

                string name = default;
                if (person.Id.ToString().Length >= maxLengthOfId)
                {
                    name = $"ID: {person.Id}.  {person.FirstName} {person.LastName}";
                }
                else
                {
                    name = $"ID: {person.Id}.   {person.FirstName} {person.LastName}";
                }
                Console.Write("\t\t");

                Console.Write($"{name.PadRight(35, ' ')}  ");
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
            if (removePerson == true)
            {
                RemovePersonInputField(persons, fullIdList);
            }
            else
            {
                ShowPersonInputField(persons, fullIdList);

            }
        }

        private void ShowPersonInputField(List<Relative> persons, List<int> fullIdList)
        {
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
                    Print.DeleteRow(left, top);
                    Console.Write("Ange id på person >");
                    top = Console.CursorTop;
                    leftTemp = Console.CursorLeft;
                    Print.DeleteRow(left, top + 1);
                    Print.Red(errorMsg);
                    Console.SetCursorPosition(leftTemp, top);
                    error = false;
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
                        errorMsg = "\t\tFelaktigt Id, försök igen.";
                    }
                }
                else
                {
                    error = true;
                    errorMsg = "\t\tFelaktig inmatning";
                }
            } while (!continueCode);
        }




        private void RemovePersonInputField(List<Relative> persons, List<int> fullIdList)
        {
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
                    Print.DeleteRow(left, top);
                    Console.Write("Ange id på person du vill ta bort >");
                    top = Console.CursorTop;
                    leftTemp = Console.CursorLeft;
                    Print.DeleteRow(left, top + 1);
                    Print.Red(errorMsg);
                    Console.SetCursorPosition(leftTemp, top);
                    error = false;
                }
                else if (fullIdList.Count > 0)
                {
                    Console.WriteLine();
                    Console.Write("\t\tAnge id på person du vill ta bort >");

                }
                else
                {

                    Print.Red("Ingen träff i databasen. Tryck enter för göra en ny sökning.");
                    Console.ReadKey();
                    break;
                }
                string input = Console.ReadLine();
                Console.WriteLine();
                if (int.TryParse(input, out int userChoice))
                {
                    if (userChoice == 0)
                    {
                        continueCode = true;

                    }
                    else if (fullIdList.Contains(userChoice))
                    {
                        var indexNr = fullIdList.IndexOf(userChoice);


                        Console.Write($"\t\tÄr du säker på att du vill ta bort {persons[indexNr].FirstName} {persons[indexNr].LastName} och alla kopplingar till denne? y/n >");
                        input = Console.ReadLine();
                        switch (input.ToLower())
                        {
                            case "y":
                                Console.Write("\t\tVänta, Tar bort personen");
                                string dots = "....";
                                foreach (var dot in dots)
                                {
                                    Console.Write(dot);
                                    Thread.Sleep(500);
                                }
                                Crud crud = new Crud("FamilyTree");
                                crud.RemovePerson(persons[indexNr].Id);
                                Console.WriteLine($"\n\t\t{persons[indexNr].FirstName} {persons[indexNr].LastName} är nu borttagen, tryck enter för att fortsätta");
                                Console.ReadKey();
                                continueCode = true;
                                break;
                            case "n":
                                Console.WriteLine($"\t\tPersonen togs EJ bort, tryck enter för att fortsätta");
                                continueCode = true;
                                Console.ReadKey();
                                break;
                            default:
                                Console.WriteLine($"\t\tFelaktigt val, tryck enter för att fortsätta");
                                Console.ReadKey();
                                break;
                        }

                    }
                    else
                    {
                        Console.WriteLine($"Felaktigt Id, tryck enter för att fortsätta");
                        Console.ReadKey();
                    }
                }
                else
                {
                    error = true;
                    Console.WriteLine($"Felaktig inmatning, tryck enter för att fortsätta");
                    Console.ReadKey();
                }
            } while (!continueCode);
        }

        /*******************************************************************
                                LISTPERSONDETAILS() - PRIVATE
         *******************************************************************/

        private void ListPersonDetails(int userChoice, List<Relative> persons)
        {
            Crud crud = new Crud("FamilyTree");
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


                Console.WriteLine();
                Print.DarkGrey("\t\tBarn");
                Console.WriteLine("\t\t" + new string('═', 10));

                var childrenList = crud.GetSiblings(new List<int> { person.Id });
                if (childrenList == null)
                {
                    Console.WriteLine($"\t\tPersonen har inga barn.");
                }
                else
                {
                    foreach (var child in childrenList)
                    {
                        Console.WriteLine($"\t\t{child.FirstName} {child.LastName}");
                    }
                }


                if (person.MotherId != 0 || person.FatherId != 0)
                {
                    Console.WriteLine();
                    Print.DarkGrey("\t\tFöräldrar");
                    Console.WriteLine("\t\t" + new string('═', 10));

                    var parentList = crud.GetParents(new List<int> { person.MotherId, person.FatherId });
                    foreach (var parent in parentList)
                    {
                        Console.WriteLine($"\t\t{parent.FirstName} {parent.LastName}");
                    }
                }
                else
                {
                    Console.WriteLine();
                    Print.DarkGrey("\t\tFöräldrar");
                    Console.WriteLine("\t\t" + new string('═', 10));
                    Console.WriteLine("\t\tInga föräldrar hittades.");
                }

                Console.WriteLine();
                Print.DarkGrey("\t\tSyskon");
                Console.WriteLine("\t\t" + new string('═', 10));
                var siblingList = crud.GetSiblings(new List<int> { person.MotherId, person.FatherId }, person.Id);
                if (siblingList != null)
                {
                    foreach (var parent in siblingList.Where(x => x != null))
                    {
                        Console.WriteLine($"\t\t{parent.FirstName} {parent.LastName}");
                    }
                }
                else
                {
                    Console.WriteLine("\t\tInga syskon hittades.");
                }
            }
        }
    }
}
