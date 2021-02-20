using System;
using System.IO;

namespace FamilyTree.Utils
{
    static class Print
    {


        public static void Black(string text)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(text);
            Console.ResetColor();
        }

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

        public static void ChangeBackgroundBlack()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static void ChangeBackgroundDarkGrey()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static void DarkBlue(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(text);
            Console.ResetColor();

        }

        public static void DarkCyan(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void DarkGrey(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(text);
            Console.ResetColor();

        }

        public static void DeleteRow(int left, int top)
        {
            Console.SetCursorPosition(left, top);
            Console.WriteLine(new string(' ', 100));
            Console.SetCursorPosition(left, top);
        }

        public static void Green(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void GreenW(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void Grey(string text)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(text);
            Console.ResetColor();

        }

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
        public static void ResetBackground()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Yellow(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void YellowW(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
