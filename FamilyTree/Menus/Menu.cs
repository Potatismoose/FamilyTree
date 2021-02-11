using FamilyTree.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

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
            
            List<string> startMenuOptions = new List<string> { "Sök person", "Visa alla personer", "Ändra person", "Lägga till person", "Ta bort en person", "Avsluta programmet"};
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
                    Console.WriteLine($"\t\t{i+1}. {startMenuOptions[i]}");
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
                Console.SetCursorPosition(0, top-3);
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

            List<string> searchMenuOptions = new List<string> { "Sök person:> "};
            bool error = false;
            string errorMsg = default;
            bool continueCode = false;

            do
            {
                Console.Clear();
                Print.PrintText("Search");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("\t\tHär kan du söka efter en person, eller tryck 1+enter för att avbryta");
                for (int i = 0; i < searchMenuOptions.Count; i++)
                {
                    Console.Write($"\n\t\t{searchMenuOptions[i]}");
                }

                
                string input = Console.ReadLine();
                int.TryParse(input, out int userChoice);
                switch (userChoice)
                {
                    case 1:
                        continueCode = true;
                        break;
                    default:
                        Console.WriteLine("search for input");
                        Console.ReadKey();
                        break;
                }


            } while (!continueCode);

        }




        /**************************************************************
                                SHOWALLPEOPLE()
         **************************************************************/
        private void ShowAllPeople()
        {

            
                Console.Clear();
                Print.PrintText("List");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("\t\tHär listas alla personer i databasen.\n\t\tTryck 1+enter för att avbryta");
                


                string input = Console.ReadLine();
                int.TryParse(input, out int userChoice);
                switch (userChoice)
                {
                    case 1:
                        
                        break;
                    default:
                        Console.WriteLine("search for input");
                        Console.ReadKey();
                        break;
                }


            

        }
    }
}
