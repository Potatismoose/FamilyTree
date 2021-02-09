using FamilyTree.Database;
using System;

namespace FamilyTree
{
    static class Program
    {
        static void Main(string[] args)
        {
            Crud db = new Crud();
            db.CreateDatabase("FamilyTree");
            Console.WriteLine(db.DoesDataBaseExist());
            Console.ReadKey();
        }
    }
}
