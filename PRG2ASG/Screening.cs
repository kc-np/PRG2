using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2ASG
{
    class Screening : IComparable<Screening>
    {
        // properties
        public int ScreeningNo { get; set; }
        public DateTime ScreeningDateTime { get; set; }
        public string ScreeningType { get; set; }
        public int SeatsRemaining { get; set; }
        public Cinema Cinema { get; set; }  // screening at which cinema (1)
        public Movie Movie { get; set; }  // screening of which movie (1)

        // constructors

        // non-parameterized
        public Screening() { }

        // parameterized
        public Screening(int sNo, DateTime sDateTime, string sType, Cinema cinema, Movie movie)
        {
            ScreeningNo = sNo;
            ScreeningDateTime = sDateTime;
            ScreeningType = sType;
            Cinema = cinema;
            Movie = movie;
        }

        // methods

        // IComparable CompareTo() method to sort screening sessions according to seats remaining in descending order
        public int CompareTo(Screening screening)
        {
            if (SeatsRemaining > screening.SeatsRemaining)
            {
                return -1;
            }
            else if (SeatsRemaining == screening.SeatsRemaining)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        // basic ToString() method
        public override string ToString()
        {
            string output = string.Format("Screening No.: {0}\tScreening Date Time: {1}\tScreening Type: {2}\tSeats Remaining: {3}\t Cinema: {4}\tMovie: {5}", ScreeningNo, ScreeningDateTime, ScreeningType, SeatsRemaining, Cinema, Movie);
            return output;
        }
    }
}
