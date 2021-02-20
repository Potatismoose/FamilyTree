using System;

namespace FamilyTree.Person
{

    class Test
    {

        public Test()
        {
            TestValue = 5;
        }

        public int TestValue { get; set; }
    }
    class Relative
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string BirthPlace { get; set; }
        public string DeathPlace { get; set; }
        public int MotherId { get; set; }
        public int FatherId { get; set; }
        public bool Alive { get; set; }
        public Relative()
        {
        }

        /**********************************************************
                            CONSTRUCTOR FOR RELATIVE
         **********************************************************/
        public Relative
            (string fName,
            string lName,
            DateTime birthDate,
            DateTime deathDate,
            int motherId,
            int fatherId,
            params (string, string)[] birthDeathPlace)
        {
            this.FirstName = fName;
            this.LastName = lName;
            if (birthDate.ToShortDateString()=="0001-01-01" || birthDate.ToShortDateString() == null)
            {
                this.BirthDate = null;
            }
            else
            {
                this.BirthDate = birthDate;
            }

            if (deathDate.ToShortDateString() == "0001-01-01" || deathDate.ToShortDateString() == null)
            {
                this.DeathDate = null;
                this.Alive = true;
            }
            else
            {
                this.DeathDate = deathDate;
                this.Alive = false;
            }


            foreach (var parameter in birthDeathPlace)
            {
                if (parameter.Item1 == "")
                {
                    this.BirthPlace = null;
                }
                else
                {
                    this.BirthPlace = parameter.Item1;
                }

                if (parameter.Item2 == "")
                {
                    this.DeathPlace = null;
                }
                else
                {
                    this.DeathPlace = parameter.Item2;
                }
            }
            if (motherId != 0)
            {
                this.MotherId = motherId;
            }
            if (fatherId != 0)
            {
                this.FatherId= fatherId;
            }


        }

    }
}
