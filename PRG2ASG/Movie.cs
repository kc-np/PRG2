using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2ASG
{
    class Movie
    {

        // properties
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Classification { get; set; }
        public DateTime OpeningDate { get; set; }
        public List<string> GenreList { get; set; }
        public List<Screening> ScreeningList { get; set; } // list of this movie's screening sessions (0 or more)
            = new List<Screening>();


        // constructors

        // non-parameterized
        public Movie() { }

        // parameterized
        public Movie(string title, int duration, string classification, DateTime openingDate, List<string> genreList)
        {
            Title = title;
            Duration = duration;
            Classification = classification;
            OpeningDate = openingDate;
            GenreList = genreList;
        }

        // methods

        // add screening for the movie
        public void AddScreening(Screening screening)
        {
            ScreeningList.Add(screening);
        }

        // add genre for the movie
        public void AddGenre(string genre)
        {
            GenreList.Add(genre);
        }
        
        // basic ToString() method
        public override string ToString()
        {
            string output = string.Format("Title: {0}\tDuration: {1}\tClassification: {2}\tOpening Date: {3}\tGenre List: {4}\tScreening List: {5}", Title, Duration, Classification, OpeningDate, GenreList, ScreeningList);
            return output;
        }
    }
}
