using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2ASG
{
    class Student: Ticket
    {
        // properties
        public string LevelOfStudy { get; set; }

        // constructors

        // non-parameterized
        public Student() : base() { }

        // parameterized
        public Student(Screening screening, string levelOfStudy) : base(screening)
        {
            LevelOfStudy = levelOfStudy;
        }

        // methods

        // calculate price of Student ticket based on pricing scheme
        public override double CalculatePrice()
        {
            double price;
            string sType = Screening.ScreeningType;
            DateTime sDateTime = Screening.ScreeningDateTime;
            DateTime mOpening = Screening.Movie.OpeningDate;
            int diff = sDateTime.Subtract(mOpening).Days;
            if (diff <= 7)
            {
                // create new Adult object and use its CalculatePrice() method to get price
                // if screening is within first 7 days of movie opening date
                Adult aObj = new Adult(Screening, false);
                price = aObj.CalculatePrice();
            }
            else
            {
                int day = (int)sDateTime.DayOfWeek;  // get day of week of screening date time
                if (day != 0 && day <= 4)
                {
                    if (sType == "3D")
                    {
                        price = 8.00;
                    }
                    else
                    {
                        price = 7.00;
                    }
                }
                else
                {
                    if (sType == "3D")
                    {
                        price = 14.00;
                    }
                    else
                    {
                        price = 12.50;
                    }
                }
            }

            return price;
        }

        // basic ToString() method
        public override string ToString()
        {
            string output = string.Format("{0}\tLevel of Study: {1}", base.ToString(), LevelOfStudy);
            return output;
        }
    }
}
