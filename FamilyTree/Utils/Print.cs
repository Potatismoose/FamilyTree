using System;
using System.IO;

namespace FamilyTree.Utils
{
    static class Print
    {



        // 5 methods for printing out text in different colors. Takes a string as the text that should be printed.
        public static void Blue(string text)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (text.Contains("alternativ"))
            {
                Console.Write(text);
            }
            else
            {
                Console.WriteLine(text);
            }
            Console.ResetColor();

        }
        public static void DarkGrey(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(text);
            Console.ResetColor();

        }
        public static void Green(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void Red(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void RedW(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(text);
            Console.ResetColor();
        }


        /// <summary>
        /// Used for printing out an empty line (deleting current text printed there).
        /// </summary>
        /// <param name="left">Where to start (left position).</param>
        /// <param name="top">Where to start (top position).</param>
        public static void DeleteRow(int left, int top)
        {
            Console.SetCursorPosition(left, top);
            Console.WriteLine(new string(' ', 100));
            Console.SetCursorPosition(left, top);
        }
        /// <summary>
        /// Method for printing out the pages "top text"
        /// </summary>
        /// <param name="keyword"></param>
        public static void PrintText(string keyword)
        {
            /*Check for keyword in file, read from next line and print out 
            til keyword 2 is found (not including keyword2)*/
            string keyWord1 = keyword;
            string keyWord2 = "#" + keyword;
            var contents = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, @"../../../../text.txt"));
            bool printOut = false;
            foreach (var line in contents)
            {
                if (line.Equals(keyWord1) || printOut)
                {
                    if (printOut && line != keyWord2)
                    {
                        Green($" \t\t{line}");
                    }
                    else
                    {
                        if (printOut)
                        {
                            printOut = false;
                        }
                        else
                        {
                            printOut = true;
                        }
                    }
                }
            }

        }
    }
}
