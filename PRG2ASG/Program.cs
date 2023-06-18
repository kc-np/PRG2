using System;
using System.IO;
using System.Collections.Generic;

namespace PRG2ASG
{
    class Program
    {
        // empty Console.WriteLine(); lines for spaces between output in console

        // static variables for order number/screening number
        public static int orderNo = 1;
        public static int screeningNo = 1001;

        static void Main(string[] args)
        {
            //============================================================
            // BASIC FEATURES
            //============================================================


            // 1) Load Movie and Cinema Data
            //============================================================

            List<string[]> movieData = new List<string[]>();  // list to store data from Movie.csv
            string movieLoc = "Movie.csv";
            List<string[]> cinemaData = new List<string[]>();  // list to store data from Cinema.csv
            string cinemaLoc = "Cinema.csv";

            // populate movieData and cinemaData
            InitDataList(movieData, movieLoc);
            InitDataList(cinemaData, cinemaLoc);

            List<Movie> movieObjects = new List<Movie>();  // list to store Movie objects
            List<Cinema> cinemaObjects = new List<Cinema>();  // list to store Cinema objects

            // create Movie and Cinema objects with the data extracted
            CreateMovies(movieData, movieObjects);
            CreateCinemas(cinemaData, cinemaObjects);


            // 2) Load Screening Data
            //============================================================

            List<string[]> screeningData = new List<string[]>();  // list to store data from Screening.csv
            string screeningLoc = "Screening.csv";

            // populate screeningData
            InitDataList(screeningData, screeningLoc);

            List<Screening> screeningObjects = new List<Screening>();  // list to store Screening objects

            // create Screening objects with the data extracted
            CreateScreenings(screeningData, screeningObjects, cinemaObjects, movieObjects);

            // populate screeningList for Movie objects
            InitMovieScreenings(screeningObjects, movieObjects);

            // list to store Order objects
            List<Order> orderObjects = new List<Order>();


            // main menu

            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                Console.WriteLine();

                if (choice == "0")
                {
                    Console.WriteLine("Bye");

                    break;
                }
                else if (choice == "1")
                {
                    // 3) List all movies
                    //============================================================

                    DisplayMovies(movieObjects);
                }
                else if (choice == "2")
                {
                    // 4) List movie screenings
                    //============================================================

                    DisplayMovies(movieObjects);

                    Console.WriteLine();

                    Movie selectedMovie = SelectMovie(movieObjects);

                    if (selectedMovie != null)
                    {
                        Console.WriteLine();

                        DisplayScreenings(selectedMovie.ScreeningList);
                    }
                }
                else if (choice == "3")
                {
                    // 5) Add a movie screening session
                    //============================================================

                    bool addScreeningStatus = AddScreeningSession(movieObjects, cinemaObjects, screeningObjects);

                    Console.WriteLine();

                    DisplayScreeningStatus(addScreeningStatus, "Add");
                }
                else if (choice == "4")
                {
                    // 6) Delete a movie screening session
                    //============================================================

                    bool remScreeningStatus = RemScreeningSession(screeningObjects);

                    Console.WriteLine();

                    DisplayScreeningStatus(remScreeningStatus, "Delete");
                }
                else if (choice == "5")
                {
                    // 7) Order movie ticket/s
                    //============================================================

                    bool makeOrderStatus = MakeOrder(movieObjects, orderObjects);

                    Console.WriteLine();

                    DisplayOrderStatus(makeOrderStatus, "Place");
                }
                else if (choice == "6")
                {
                    // 8) Cancel order of ticket
                    //============================================================

                    bool cancelOrderStatus = CancelOrder(orderObjects);

                    Console.WriteLine();

                    DisplayOrderStatus(cancelOrderStatus, "Cancel");

                }
                //============================================================
                // ADVANCED  FEATURES
                //============================================================
                else if (choice == "7")
                {
                    // 1) Recommend movie based on sales of tickets
                    //============================================================

                    ShowRecommendedMovies(orderObjects, movieObjects);
                }
                else if (choice == "8")
                {
                    // 2) Display available seats of screening session in descending order
                    //============================================================

                    List<Screening> sortedScreeningList = SortScreening(screeningObjects);

                    DisplayScreenings(sortedScreeningList);
                }
                else if (choice == "9")
                {
                    // 3) Display highest sold movie of selected cinema
                    //============================================================

                    ShowMostSoldMovie(orderObjects, movieObjects, cinemaObjects);
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                }

                Console.WriteLine();
            }
        }

        // method for displaying main menu
        static void DisplayMenu()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("Main menu:");
            Console.WriteLine("============================================================");
            Console.WriteLine("1) List all movies");
            Console.WriteLine("2) List movie screenings");
            Console.WriteLine("3) Add a movie screening session");
            Console.WriteLine("4) Delete a movie screening session");
            Console.WriteLine("5) Order movie ticket(s)");
            Console.WriteLine("6) Cancel order of ticket(s)");
            Console.WriteLine("7) See recommended movie based on sales of tickets");
            Console.WriteLine("8) Display screening sessions in descending order of available seats");
            Console.WriteLine("9) View most sold movie of each cinema");
            Console.WriteLine("0) Exit\n");
            Console.Write("Enter your choice: ");
        }

        // method for increasing orderNo by 1
        static void IncreaseOrder()
        {
            orderNo += 1;
        }

        // method for increasing screeningNo by 1
        static void IncreaseScreening()
        {
            screeningNo += 1;
        }

        // method for initializing *Data list with contents from *.csv
        // receive parameters: dataList - list to populate, location - name of the file
        static void InitDataList(List<string[]> dataList, string location)
        {
            try
            {
                using (StreamReader sr = new StreamReader(location))
                {
                    string line = sr.ReadLine();  // heading --> no actions performed on it
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] contents = line.Split(',');
                        dataList.Add(contents);
                    }
                }
            }
            // display error message if file not found and terminate the program - objects cannot be created without the data and the main menu wouldn't work
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Check if there is an error in the location specified.");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        // method for creating DateTime objects from data provided
        // formats:
        // from Movie.csv: eg dd/MM/yyyy
        // from Screening.csv: eg dd/MM/yyyy hh:mmPM
        static DateTime CreateDateTime(string data)
        {
            // remove whitespace at the start and end of the string
            // (in case user inputs a date with leading or trailing whitespace but correct format eg "dd/MM/yyyy  " there will not be error)
            data = data.Trim();

            DateTime date;
            string[] fields = data.Split('/');
            int day = Convert.ToInt32(fields[0]);
            int month = Convert.ToInt32(fields[1]);

            if (data.Length == 10)  // format like dd/MM/yyyy
            {
                int year = Convert.ToInt32(fields[2]);
                date = new DateTime(year, month, day);
            }
            else  // format like dd/MM/yyyy hh:mm tt
            {
                int year = Convert.ToInt32(fields[2].Substring(0, 4));  // eg from "2022 8:00PM" returns 2022
                string timeString = fields[2].Substring(4);  // eg from "2022 8:00PM" returns "8:00PM"
                string[] times = timeString.Split(':');  // eg from "8:00PM" returns ["8", "00PM"]
                int hour = Convert.ToInt32(times[0]);
                if (times[1].ToUpper().Contains("PM"))  // eg from "00PM"/"00pm": returns true
                {
                    if (hour != 12)
                    {
                        hour += 12;
                    }
                }
                int min = Convert.ToInt32(times[1].Substring(0, 2));  // eg from "00PM" returns 00
                date = new DateTime(year, month, day, hour, min, 00);
            }

            // returns the DateTime object created above
            return date;
        }

        // method for creating Movie objects using data from dataList and store in movieList
        static void CreateMovies(List<string[]> dataList, List<Movie> movieList)
        {
            // display alert message if there is no data found in dataList
            if (dataList.Count == 0)
            {
                Console.WriteLine("No data found for creation of Movie objects.");
            }
            else
            {
                foreach (string[] content in dataList)
                {
                    // get the values of the properties needed for the Movie object constructor
                    string title = content[0];
                    int duration = Convert.ToInt32(content[1]);
                    string classification = content[3];
                    // create DateTime object
                    DateTime openingDate = CreateDateTime(content[4]);

                    // populate genreList
                    List<string> genreList = new List<string>();
                    string[] genres = content[2].Split('/');
                    foreach (string genre in genres)
                    {
                        genreList.Add(genre);
                    }

                    // create Movie object and add to movieList
                    Movie mObject = new Movie(title, duration, classification, openingDate, genreList);
                    movieList.Add(mObject);
                }
            }
        }

        // method for creating Cinema objects using data from dataList and store in cinemaList
        static void CreateCinemas(List<string[]> dataList, List<Cinema> cinemaList)
        {
            // display alert message if there is no data found in dataList
            if (dataList.Count == 0)
            {
                Console.WriteLine("No data found for creation of Cinema objects.");
            }
            else
            {
                foreach (string[] content in dataList)
                {
                    // get the values of the properties needed for the Cinema object constructor
                    string name = content[0];
                    int hallNo = Convert.ToInt32(content[1]);
                    int capacity = Convert.ToInt32(content[2]);

                    // create Cinema object and add to cinemaList
                    Cinema cObject = new Cinema(name, hallNo, capacity);
                    cinemaList.Add(cObject);
                }
            }
        }

        // method for creating Screening objects using data from dataList and store in screeningList
        // provide cinemaList to get the correct Cinema object for the Screening session
        // provide movieList to get the correct Movie object for the Screening session
        static void CreateScreenings(List<string[]> dataList, List<Screening> screeningList, List<Cinema> cinemaList, List<Movie> movieList)
        {
            // display alert message if there is no data found in dataList
            if (dataList.Count == 0)
            {
                Console.WriteLine("No data found for creation of Screening objects.");
            }
            else
            {
                foreach (string[] content in dataList)
                {
                    // get the values of the properties needed for the Screening object constructor
                    int sNo = screeningNo;

                    // create DateTime object
                    DateTime sDateTime = CreateDateTime(content[0]);

                    string sType = content[1];

                    // cinemaName and hallNo for finding the corresponding Cinema
                    string cinemaName = content[2];
                    int hallNo = Convert.ToInt32(content[3]);
                    // get Cinema object
                    Cinema cinema = GetCinema(cinemaList, cinemaName, hallNo);

                    // movieTitle for finding the corresponding Movie
                    string movieTitle = content[4];
                    // get Movie object
                    Movie movie = GetMovie(movieList, movieTitle);

                    // create Screening object and add to screeningList
                    Screening sObject = new Screening(sNo, sDateTime, sType, cinema, movie);
                    sObject.SeatsRemaining = sObject.Cinema.Capacity;  // default seats remaining is the capacity of the cinema
                    screeningList.Add(sObject);

                    // increase sNo by 1
                    IncreaseScreening();
                }
            }
        }

        // method for finding Cinema object that has matching cinemaName and hallNo from provided cinemaList
        static Cinema GetCinema(List<Cinema> cinemaList, string cinemaName, int hallNo)
        {
            Cinema target = null;
            foreach (Cinema cinema in cinemaList)
            {
                if (cinema.Name == cinemaName && cinema.HallNo == hallNo)
                {
                    target = cinema;
                    break;
                }
            }

            // returns null if cinemaList empty or wanted Cinema object not found
            // returns Cinema object if it is found
            return target;
        }

        // method for finding Movie object that has matching movieTitle from provided movieList
        static Movie GetMovie(List<Movie> movieList, string movieTitle)
        {
            Movie target = null;
            foreach (Movie movie in movieList)
            {
                if (movie.Title == movieTitle)
                {
                    target = movie;
                    break;
                }
            }

            // returns null if movieList empty or wanted Movie object not found
            // returns Movie object if it is found
            return target;
        }

        // method for adding screening sessions to Movie objects' screeningList
        static void InitMovieScreenings(List<Screening> screeningList, List<Movie> movieList)
        {
            foreach (Movie movie in movieList)
            {
                foreach (Screening screening in screeningList)
                {
                    if (screening.Movie == movie)
                    {
                        movie.AddScreening(screening);
                    }
                }
            }
        }

        // method for listing all movies
        static void DisplayMovies(List<Movie> movieList)
        {
            // display message to tell user there are no movies available if movieList is empty
            if (movieList.Count == 0)
            {
                Console.WriteLine("There are no movies available.");
            }
            else
            {
                // count to display movies with serial numbers (for selection of movies)
                int count = 1;

                Console.WriteLine("Details of Movie(s):\n");
                Console.WriteLine("{0, -5}{1, -35}{2, -17}{3, -16}{4, -14}{5, -35}", "SNo", "Title", "Duration (mins)", "Classification", "Opening Date", "Genres");

                foreach (Movie movie in movieList)
                {
                    string mTitle = movie.Title;
                    int mDuration = movie.Duration;
                    string mClass = movie.Classification;
                    DateTime mODate = movie.OpeningDate;
                    List<string> genreList = movie.GenreList;

                    string mODateString = movie.OpeningDate.ToString("dd/MM/yyyy");
                    string genreString = string.Join(", ", genreList);

                    Console.WriteLine("{0, -5}{1, -35}{2, -17}{3, -16}{4, -14}{5, -35}", count, mTitle, mDuration, mClass, mODateString, genreString);

                    count += 1;
                }
            }
        }

        // method for selecting a movie (from user input)
        static Movie SelectMovie(List<Movie> movieList)
        {
            // set defualt value of target to null
            Movie target = null;

            Console.Write("Select a Movie: ");

            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                target = movieList[choice - 1];
            }
            catch (FormatException ex)  // if user input not a number - cannot convert to int
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine("Movie not found");
            }
            catch (IndexOutOfRangeException ex)  // if user input is too large - not a valid index for a movie
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine("Movie not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine("Movie not found");
            }

            // returns null if movieList is empty or wanted movie is not found
            // returns Movie object if it is found
            return target;
        }

        // method for listing all screenings
        static void DisplayScreenings(List<Screening> screeningList)
        {
            // display message to tell user there are no screenings available if screeningList is empty
            if (screeningList.Count == 0)
            {
                Console.WriteLine("There are no existing screening sessions.");
            }
            else
            {
                Console.WriteLine("Details of Screening Sessions:\n");
                Console.WriteLine("{0, -14}{1, -24}{2, -16}{3, -17}{4, -15}{5, -16}{6, -35}", "Screening No", "Screening Date Time", "Screening Type", "Seats Remaining", "Cinema Name", "Cinema Hall No", "Movie Title");

                foreach (Screening screening in screeningList)
                {
                    int sNo = screening.ScreeningNo;
                    DateTime sDateTime = screening.ScreeningDateTime;
                    string sDateTimeString = sDateTime.ToString("dd/MM/yyyy hh:mm tt");
                    string sType = screening.ScreeningType;
                    int seatsRemaining = screening.SeatsRemaining;

                    // cinema information
                    string cinemaName = screening.Cinema.Name;
                    int cinemaHallNo = screening.Cinema.HallNo;

                    // movie information
                    string movieTitle = screening.Movie.Title;

                    Console.WriteLine("{0, -14}{1, -24}{2, -16}{3, -17}{4, -15}{5, -16}{6, -35}", sNo, sDateTimeString, sType, seatsRemaining, cinemaName, cinemaHallNo, movieTitle);
                }
            }
        }

        // method for getting screening type (input from user) + validation
        // validation ensures that this method will always return a valid screening type if it returns a value
        static string GetScreeningType()
        {
            string sType;

            while (true)
            {
                Console.Write("Enter Screening Type [2D/3D]: ");
                sType = Console.ReadLine().ToUpper();  // use ToUpper() so that 2d/3d will become 2D/3D and will be accepted

                // check that the entered screening type is either 2D or 3D
                if (sType == "2D" || sType == "3D")
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Screening Type");
                    Console.WriteLine("Try again\n");
                }
            }

            // returns the screening type entered by the user
            return sType;
        }

        // method for getting screening datetime (input from user) + validation
        // validation ensures that this method will always return a valid screening date time if it returns a value
        static DateTime GetScreeningDateTime(string mTitle, DateTime openingDate)
        {
            DateTime sDateTime;

            while (true)
            {
                try
                {
                    Console.WriteLine("Enter Screening Date & Time");
                    Console.WriteLine("Format:  dd/MM/yyyy hh:mm tt");
                    Console.WriteLine("Or:      dd/MM/yyyy HH:mm");
                    Console.Write("DateTime: ");

                    string inputDateTime = Console.ReadLine();

                    sDateTime = CreateDateTime(inputDateTime);

                    // check that the screening date time entered by the user is later than the opening date of the corresponding movie
                    if (sDateTime >= openingDate)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Screening DateTime must be later than opening date for '{0}' ({1})\n", mTitle, openingDate.ToString("dd/MM/yyyy"));
                    }
                }
                catch
                {
                    Console.WriteLine();
                    // tell the user that the input string was not in correct format - root cause for the exception
                    // if there are any exceptions while creating DateTime object with CreateDateTime()
                    Console.WriteLine("Input string was not in a correct format.");
                    Console.WriteLine("Try again\n");
                }

            }

            // returns the DateTime object created above
            return sDateTime;
        }

        // method for listing all cinemas
        static void DisplayCinemas(List<Cinema> cinemaList)
        {
            // display message to tell user there are no existing cinema halls if cinemaList is empty
            if (cinemaList.Count == 0)
            {
                Console.WriteLine("There are no existing cinema halls.");
            }
            else
            {
                // count to display cinema halls with serial numbers (for selection of cinema hall)
                int count = 1;

                Console.WriteLine("Details of All Cinema Halls:\n");
                Console.WriteLine("{0, -5}{1, -20}{2, -9}{3, -10}", "SNo", "Name", "Hall No", "Capacity");

                foreach (Cinema cinema in cinemaList)
                {
                    string cinemaName = cinema.Name;
                    int cinemaHallNo = cinema.HallNo;
                    int cinemaCap = cinema.Capacity;

                    Console.WriteLine("{0, -5}{1, -20}{2, -9}{3, -10}", count, cinemaName, cinemaHallNo, cinemaCap);

                    count += 1;
                }
            }
        }

        // method to check if there are any available cinema halls at the datetime specified by user
        static List<Cinema> GetValidCinemas(List<Cinema> cinemaList, List<Screening> screeningList, DateTime sDateTime, Movie selectedMovie)
        {
            // list to store valid cinemas (cinemas that are available during the user entered date time)
            List<Cinema> validCinemas = new List<Cinema>();

            // initialize validCinemas with contents of cinemaList
            foreach (Cinema cinema in cinemaList)
            {
                validCinemas.Add(cinema);
            }

            // newStartTime is the start time of the new screening session
            DateTime newStartTime = sDateTime;
            // newEndTime is the end time of the new screening session (newStartTime + full duration of selectedMovie + 30 min cleaning time)
            DateTime newEndTime = sDateTime.AddMinutes(selectedMovie.Duration + 30);

            // remove unavailable cinemas from validCinemas
            foreach (Screening screening in screeningList)
            {
                // startTime is the time the cinema hall becomes unavailable (movie screening starts)
                DateTime startTime = screening.ScreeningDateTime;
                // endTime is the time the cinema hall become available again (after full duration of movie + 30 min cleaning time)
                DateTime endTime = startTime.AddMinutes(screening.Movie.Duration + 30);

                // outcomes that result in the cinema hall becoming unavailable

                // newStartTime between startTime and endTime
                bool outcome1 = newStartTime >= startTime && newStartTime <= endTime;
                // newEndTime between startTime and endTime
                bool outcome2 = newEndTime >= startTime && newEndTime <= endTime;

                if (outcome1 || outcome2)
                {
                    validCinemas.Remove(screening.Cinema);
                }
            }

            // returns list of valid cinemas generated above
            return validCinemas;
        }

        // method for selecting a cinema hall (from user input) + validation
        // validation will ensure that this method will always return a valid Cinema object if it returns a value
        // overloaded method
        // requires a list of valid cinemas - cinemas available at the specified datetime
        // requires a datetime - used to tell user if the cinema hall is not available at the specified datetime
        static Cinema SelectCinema(List<Cinema> cinemaList, List<Cinema> validCinemas, DateTime sDateTime)
        {
            Cinema target;

            while (true)
            {
                try
                {
                    Console.Write("Select a Cinema Hall: ");

                    int choice = Convert.ToInt32(Console.ReadLine());
                    target = cinemaList[choice - 1];

                    if (validCinemas.Contains(target))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("{0} Hall No. {1} not available for screening session at {2}", target.Name, target.HallNo, sDateTime.ToString("dd/MM/yyyy hh:mm tt"));
                    }
                }
                catch (FormatException ex)  // if user input not a number - cannot convert to int
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Cinema hall not found\n");
                }
                catch (IndexOutOfRangeException ex)  // if user input is too large - not a valid index for a cinema hall
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Cinema hall not found\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Cinema hall not found\n");
                }
            }

            // returns Cinema object if it is found
            return target;
        }

        // overloaded method
        // does not require list of valid cinemas and datetime
        static Cinema SelectCinema(List<Cinema> cinemaList)
        {
            Cinema target;

            while (true)
            {
                try
                {
                    Console.Write("Select a Cinema Hall: ");

                    int choice = Convert.ToInt32(Console.ReadLine());
                    target = cinemaList[choice - 1];

                    break;
                }
                catch (FormatException ex)  // if user input not a number - cannot convert to int
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Cinema hall not found\n");
                }
                catch (IndexOutOfRangeException ex)  // if user input is too large - not a valid index for a cinema hall
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Cinema hall not found\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Cinema hall not found\n");
                }
            }

            // returns Cinema object if it is found
            return target;
        }

        // method to create a new screening session and add to the corresponding Movie object's screening list
        static bool AddScreeningSession(List<Movie> movieList, List<Cinema> cinemaList, List<Screening> screeningList)
        {
            bool result = false;

            // list all movies
            DisplayMovies(movieList);

            Console.WriteLine();

            // select a movie
            Movie selectedMovie = SelectMovie(movieList);

            if (selectedMovie != null)
            {
                Console.WriteLine();

                // get screening type
                string sType = GetScreeningType();

                Console.WriteLine();

                // get screening date time
                DateTime sDateTime = GetScreeningDateTime(selectedMovie.Title, selectedMovie.OpeningDate);

                Console.WriteLine();

                // get a list of all available cinema halls at the specified screening date time
                List<Cinema> validCinemasList = GetValidCinemas(cinemaList, screeningList, sDateTime, selectedMovie);
                if (validCinemasList.Count != 0)
                {
                    // list all cinema halls
                    DisplayCinemas(cinemaList);

                    Console.WriteLine();

                    // select a cinema hall
                    Cinema selectedCinema = SelectCinema(cinemaList, validCinemasList, sDateTime);
                    // create new Screening object
                    int sNo = screeningNo;

                    Screening newScreening = new Screening(sNo, sDateTime, sType, selectedCinema, selectedMovie);
                    newScreening.SeatsRemaining = selectedCinema.Capacity;

                    // add screening to relevant screening lists
                    screeningList.Add(newScreening);
                    selectedMovie.AddScreening(newScreening);

                    Console.WriteLine("Screening Session {0} added.", sNo);

                    // increase screeningNo
                    IncreaseScreening();

                    result = true;
                }
                else  // display alert message if there are no available cinema halls
                {
                    Console.WriteLine("No available cinemas at {0}", sDateTime.ToString("dd/MM/yyyy dd:mm tt"));
                }
            }

            // returns false if screening session not added
            // returns true if screening session is added
            return result;
        }

        // method to get all Screening objects that have not sold any tickets
        static List<Screening> GetNonSoldScreenings(List<Screening> screeningList)
        {
            // list to store Screening objects that have not sold any tickets
            List<Screening> nonSoldScreenings = new List<Screening>();

            foreach (Screening screening in screeningList)
            {
                if (screening.SeatsRemaining == screening.Cinema.Capacity)
                {
                    nonSoldScreenings.Add(screening);
                }
            }

            // returns list of Screening objects generated above
            return nonSoldScreenings;
        }

        // method to select a screening session (from user input) + validation
        // validation will ensure that this method will always return a valid Screening object if it returns a value
        static Screening SelectScreening(List<Screening> screeningList)
        {
            Screening target = null;

            while (true)
            {
                try
                {
                    Console.Write("Select a Screening Session: ");

                    int choice = Convert.ToInt32(Console.ReadLine());

                    foreach (Screening screening in screeningList)
                    {
                        if (choice == screening.ScreeningNo)
                        {
                            target = screening;

                            break;
                        }
                    }

                    if (target != null)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid screening session\n");
                    }
                }
                catch (FormatException ex)  // if user input not a number - cannot convert to int
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Screening session not found\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Screening session not found\n");
                }
            }

            // returns the screening session selected
            return target;
        }

        // method to remove a screening session
        static bool RemScreeningSession(List<Screening> screeningList)
        {
            bool result = false;

            // get screening sessions that have not sold any tickets
            List<Screening> nonSoldScreenings = GetNonSoldScreenings(screeningList);

            if (nonSoldScreenings.Count != 0)
            {
                // list screening sessions
                DisplayScreenings(nonSoldScreenings);

                Console.WriteLine();

                // select a screening session
                Screening selectedScreening = SelectScreening(nonSoldScreenings);

                // remove screening from all relevant screening lists
                screeningList.Remove(selectedScreening);
                selectedScreening.Movie.ScreeningList.Remove(selectedScreening);

                result = true;
            }
            else
            {
                Console.WriteLine("There is no screening session that has not sold any tickets.");
            }

            // returns false if screening session is not removed
            // returns true if screening session is removed
            return result;
        }

        // method to get total number of tickets to order (from user input) + validation
        // validation will ensure that this method will always return a valid number of tickets if it returns a value
        static int GetTicketCount(Screening screening)
        {
            int count;

            while (true)
            {
                try
                {
                    Console.Write("Enter total number of tickets to order: ");
                    count = Convert.ToInt32(Console.ReadLine());

                    if (count > 0 && count <= screening.SeatsRemaining)
                    {
                        break;
                    }
                    else if (count <= 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Number of tickets ordered must not be 0 or smaller.");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Number of tickets ordered must not be greater than the number of available seats left for\nScreening Session {0} ({1} seats remaining)\n", screening.ScreeningNo, screening.SeatsRemaining);
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Try again\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Try again\n");
                }
            }

            // returns the number of tickets to be ordered
            return count;
        }

        // method to check if all ticket holders meet movie classification requirements
        static bool CheckClassReq(int count, string movieClass)
        {
            bool result = true;
            if (movieClass == "G")
            {
                // return true if movie classification is "G" - no check needed
                return result;
            }
            else
            {
                // get minimum age required
                // get age specified in the classification using Substring() method, set start index to classification length - 2
                // eg PG13 => start index = 4-2=2 (1), length = 2 (13)
                int ageReq = Convert.ToInt32(movieClass.Substring(movieClass.Length - 2, 2));

                int i = 0;
                while (i < count)
                {
                    try
                    {
                        Console.WriteLine();

                        Console.Write("Enter age of ticket holder {0}: ", i + 1);
                        int age = Convert.ToInt32(Console.ReadLine());

                        // check if ticket holder's age meets movie classification requirements
                        if (age < ageReq)
                        {
                            result = false;
                            break;
                        }
                        else if (age > 130)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Ticket holder must be less than 130 years old.");
                            Console.WriteLine("Try again\n");
                        }
                        else
                        {
                            i += 1;
                        }
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine();
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Try again\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Try again\n");
                    }
                }

                // returns false if age requirement not met by one or more ticket holders
                // returns true if age requirement met by all ticket holders
                return result;
            }
        }

        // method to create and initialise an Order object (set status to 'unpaid')
        static Order InitOrderObj()
        {
            int oNo = orderNo;
            DateTime oDateTime = DateTime.Now;

            Order newOrder = new Order(oNo, oDateTime);
            // initial status of Order object set to Unpaid
            newOrder.Status = "Unpaid";

            // increase orderNo
            IncreaseOrder();

            // returns new Order object created above
            return newOrder;
        }

        // method to get ticket type (from user input) + validation
        // validation will ensure that this method will return a valid ticket type (int with values 1, 2, or 3) if it returns a value
        static int GetTicketType(int ticketNo)
        {
            int choice;
            List<int> validChoices = new List<int>() { 1, 2, 3 };

            while (true)
            {
                try
                {
                    Console.WriteLine("Ticket holder {0}\n", ticketNo);
                    Console.WriteLine("1: Student");
                    Console.WriteLine("2: Senior Citizen");
                    Console.WriteLine("3: Adult");
                    Console.Write("Select ticket type: ");
                    choice = Convert.ToInt32(Console.ReadLine());

                    if (validChoices.Contains(choice))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid option.");
                        Console.WriteLine("Try again\n");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Try again\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Try again\n");
                }
            }

            // returns choice entered by user
            return choice;
        }

        // method to create Student object
        static Student CreateStudent(Screening screening)
        {
            string levelOfStudy;
            Student newStudent;
            List<string> validLevels = new List<string>() { "primary", "secondary", "tertiary" };

            while (true)
            {
                Console.Write("Enter level of study: ");
                levelOfStudy = Console.ReadLine().ToLower();  // convert to lower case so checking of input is not case-sensitive

                if (validLevels.Contains(levelOfStudy))
                {
                    newStudent = new Student(screening, levelOfStudy);

                    Console.WriteLine();
                    Console.WriteLine("Student ticket added.");

                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid level of study.");
                    Console.WriteLine("Accepted level of study: Primary, Secondary, Tertiary");
                    Console.WriteLine("Try again\n");
                }
            }

            // returns new Student object created above
            return newStudent;
        }

        // method to create SeniorCitizen object
        static SeniorCitizen CreateSeniorCitizen(Screening screening)
        {
            int yearOfBirth;
            SeniorCitizen newSeniorCitizen;
            while (true)
            {
                try
                {
                    Console.Write("Enter year of birth: ");
                    yearOfBirth = Convert.ToInt32(Console.ReadLine());

                    int age = DateTime.Now.Year - yearOfBirth;
                    if (age >= 55 && age <= 130)  // assume people cannot be older than 130 years old
                    {
                        newSeniorCitizen = new SeniorCitizen(screening, yearOfBirth);

                        Console.WriteLine();
                        Console.WriteLine("Senior Citizen ticket added.");

                        break;
                    }
                    else if (age >= 130)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Ticket holder must be less than 130 years old.");
                        Console.WriteLine("Try again\n");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Ticket holder must be at least 55 years old.");
                        Console.WriteLine("Try again\n");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Try again\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Try again\n");
                }
            }

            // returns new SeniorCitizen object created above
            return newSeniorCitizen;
        }

        // method to create Adult object
        static Adult CreateAdult(Screening screening)
        {
            bool popcorn;
            Adult newAdult;

            while (true)
            {
                Console.Write("Purchase popcorn offer for $3? ");
                string option = Console.ReadLine().ToUpper();

                if (option == "Y")
                {
                    popcorn = true;

                    newAdult = new Adult(screening, popcorn);

                    Console.WriteLine();
                    Console.WriteLine("Adult ticket added.");

                    break;
                }
                else if (option == "N")
                {
                    popcorn = false;

                    newAdult = new Adult(screening, popcorn);

                    Console.WriteLine();
                    Console.WriteLine("Adult ticket added.");

                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid option.");
                    Console.WriteLine("Accepted options: Y/N");
                    Console.WriteLine("Try again\n");
                }
            }

            // returns new Adult object created above
            return newAdult;
        }

        // method to make an order
        static bool MakeOrder(List<Movie> movieList, List<Order> orderList)
        {
            bool result = false;

            // list all movies
            DisplayMovies(movieList);

            Console.WriteLine();

            // select a movie
            Movie selectedMovie = SelectMovie(movieList);

            if (selectedMovie != null)
            {
                Console.WriteLine();

                // list all screenings for the movie
                DisplayScreenings(selectedMovie.ScreeningList);
                if (selectedMovie.ScreeningList.Count != 0)
                {
                    Console.WriteLine();
                    
                    // select a screening session
                    Screening selectedScreening = SelectScreening(selectedMovie.ScreeningList);

                    Console.WriteLine();

                    if (selectedScreening.SeatsRemaining == 0)
                    {
                        Console.WriteLine("There are no available seats.");
                    }
                    else
                    {
                        // get total number of tickets to order
                        int ticketCount = GetTicketCount(selectedScreening);

                        // check age requirements
                        string movieClass = selectedMovie.Classification;
                        bool ageReqMet = CheckClassReq(ticketCount, movieClass);

                        Console.WriteLine();

                        if (ageReqMet)
                        {
                            Console.WriteLine("Age requirement met.");
                            Console.WriteLine();

                            // create an Order object
                            Order newOrder = InitOrderObj();

                            // get ticket type and create corresponding ticket object
                            // values for ticket type: 1-Student 2-Senior Citizen 3-Adult
                            for (int i = 0; i < ticketCount; i++)
                            {
                                int ticketType = GetTicketType(i + 1);

                                Console.WriteLine();

                                Ticket newTicket;
                                if (ticketType == 1)
                                {
                                    newTicket = CreateStudent(selectedScreening);
                                }
                                else if (ticketType == 2)
                                {
                                    newTicket = CreateSeniorCitizen(selectedScreening);
                                }
                                else
                                {
                                    newTicket = CreateAdult(selectedScreening);
                                }

                                // add ticket to the ticket list of the order
                                newOrder.AddTicket(newTicket);

                                // update seats remaining for the movie screening
                                selectedScreening.SeatsRemaining -= 1;

                                // add price of each ticket to the total amount for the order
                                newOrder.Amount += newTicket.CalculatePrice();

                                Console.WriteLine();
                            }

                            // display total amount payable
                            Console.WriteLine("Total amount payable: ${0}", newOrder.Amount);

                            Console.WriteLine();

                            // make payment
                            Console.WriteLine("**Press any key to make payment**");
                            Console.ReadKey();

                            // update order status
                            newOrder.Status = "Paid";

                            Console.WriteLine();
                            Console.WriteLine("Payment made successfully.");

                            // add new Order object to orderList
                            orderList.Add(newOrder);

                            result = true;

                            Console.WriteLine("Order No. {0} added.", newOrder.OrderNo);
                        }
                        else
                        {
                            Console.WriteLine("Age requirement not met.");
                        }
                    }
                }
            }

            // returns false if Order object not made successfully
            // returns true if Order object made successfully
            return result;
        }

        // method to get Order object for cancellation
        static Order SelectOrder(List<Order> orderList)
        {
            Order target = null;

            if (orderList.Count == 0)
            {
                Console.WriteLine("There are no existing orders.");
            }
            else
            {
                try
                {
                    Console.Write("Enter Order number: ");
                    int orderNo = Convert.ToInt32(Console.ReadLine());

                    foreach (Order order in orderList)
                    {
                        if (order.OrderNo == orderNo)
                        {
                            target = order;

                            break;
                        }
                    }

                    if (target == null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid order.");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                }
            }

            // returns null if Order object not found
            // returns the Order object if it is found
            return target;
        }

        // method to check if the screening session of the selected Order object has screened
        static bool CheckScreeningTime(Order order)
        {
            bool result = false;

            DateTime current = DateTime.Now;
            DateTime sDateTime = order.TicketList[0].Screening.ScreeningDateTime;

            if (current >= sDateTime)
            {
                result = true;
            }

            // returns false if the screening session has not started
            // returns true if the screening session has already started
            return result;
        }

        // method to cancel an order
        static bool CancelOrder(List<Order> orderList)
        {
            bool result = false;

            Order selectedOrder = SelectOrder(orderList);
            if (selectedOrder != null)
            {
                bool hasScreened = CheckScreeningTime(selectedOrder);
                // continue with cancellation if the screening session has yet to start
                if (!hasScreened)
                {
                    if (selectedOrder.Status == "Cancelled")  // check that the order has not been cancelled (prevent double cancellation)
                    {
                        Console.WriteLine("Order has already been cancelled.");
                    }
                    else
                    {
                        // update seats remaining of the screening session
                        int ticketCount = selectedOrder.TicketList.Count;
                        selectedOrder.TicketList[0].Screening.SeatsRemaining += ticketCount;

                        // change order status
                        selectedOrder.Status = "Cancelled";

                        double amount = selectedOrder.Amount;

                        Console.WriteLine();
                        Console.WriteLine("${0} has been refunded", amount);

                        result = true;
                    }
                }
                else
                {
                    Console.WriteLine("Screening session already started.");
                }
            }

            // returns false if cancellation not made
            // returns true if cancellation made
            return result;
        }

        // method to get an array of tickets sold
        static int[] GetSalesArray(List<Order> orderList, List<Movie> movieList)
        {
            // initialise salesArray with length of movieList
            int[] salesArray = new int[movieList.Count];

            // populate salesArray with data from orderList
            foreach (Order order in orderList)
            {
                int ticketCount = order.TicketList.Count;

                // get movie ordered
                Movie orderedMovie = order.TicketList[0].Screening.Movie;

                int index = movieList.IndexOf(orderedMovie);

                // update sales array with new ticket count for the movie
                int newTicketCount = salesArray[index] + ticketCount;
                salesArray[index] = newTicketCount;
            }

            // returns salesArray created above
            return salesArray;
        }

        // method to get the highest number in the given array
        static int GetHighest(int[] intArray)
        {
            int highest = 0;

            foreach (int number in intArray)
            {
                if (number > highest)
                {
                    highest = number;
                }
            }

            // returns the highest number found above
            return highest;
        }

        // method to get orders that are not cancelled
        static List<Order> GetValidOrders(List<Order> orderList)
        {
            List<Order> validOrders = new List<Order>();

            foreach (Order order in orderList)
            {
                if (order.Status != "Cancelled")
                {
                    validOrders.Add(order);
                }
            }

            // returns the list of non-cancelled orders
            return validOrders;
        }

        // method to get movies with most tickets sold
        // returns list in case there are movies with equal number of tickets sold
        static List<Movie> GetRecommendedMovie(List<Order> orderList, List<Movie> movieList)
        {
            List<Movie> recommendedMovies = new List<Movie>();

            // get orders that are not cancelled
            List<Order> validOrders = GetValidOrders(orderList);

            if (validOrders.Count != 0)
            {
                // get salesArray
                int[] salesArray = GetSalesArray(validOrders, movieList);

                // get highest sales count
                int highestSales = GetHighest(salesArray);

                // add movies with highest sales into recommended movie list
                for (int i = 0; i < salesArray.Length; i++)
                {
                    int ticketsSold = salesArray[i];

                    if (ticketsSold == highestSales)
                    {
                        recommendedMovies.Add(movieList[i]);
                    }
                }
            }

            // returns empty list if no tickets have been sold
            // returns list of Movie objects with the highest number of tickets sold otherwise
            return recommendedMovies;
        }

        // method to get sorted screening list according to seats remaining
        // create a new list so that the original one remains untouched
        static List<Screening> SortScreening(List<Screening> screeningList)
        {
            List<Screening> sortedScreeningList = new List<Screening>();

            // initialise sortedScreeningList with contents from screeningList
            foreach (Screening screening in screeningList)
            {
                sortedScreeningList.Add(screening);
            }

            sortedScreeningList.Sort();

            // returns the sorted screening list generated above
            return sortedScreeningList;
        }

        // method to get movies screened in the selected cinema
        static List<Movie> GetScreenedMovies(List<Movie> movieList, Cinema selectedCinema)
        {
            // list to store movies that have screening sessions in the selected cinema
            List<Movie> screenedMovies = new List<Movie>();

            // populate screenedMovies
            foreach (Movie movie in movieList)
            {
                foreach (Screening screening in movie.ScreeningList)
                {
                    if (screening.Cinema == selectedCinema)
                    {
                        screenedMovies.Add(movie);

                        break;
                    }
                }
            }

            // returns the list of movies generated above
            return screenedMovies;
        }

        // method to get non-cancelled orders placed for screening sessions in the selected cinema
        static List<Order> GetValidOrdersInCinema(List<Order> orderList, Cinema selectedCinema)
        {
            List<Order> validOrders = new List<Order>();

            foreach (Order order in orderList)
            {
                Cinema orderedCinema = order.TicketList[0].Screening.Cinema;
                if (orderedCinema == selectedCinema && order.Status != "Cancelled")
                {
                    validOrders.Add(order);
                }
            }

            // returns the list of non-cancelled orders that are for screenings in the selected cinema
            return validOrders;
        }

        // method to get highest sold movies by cinema
        // returns list in case there are movies with equal number of tickets sold
        static List<Movie> GetMostSoldMovies(List<Order> orderList, List<Movie> movieList, Cinema selectedCinema)
        {
            // list to store movies with most tickets sold
            List<Movie> mostSoldMovies = new List<Movie>();

            // get list of movies screened in the selected cinema
            List<Movie> screenedMovies = GetScreenedMovies(movieList, selectedCinema);

            if (screenedMovies.Count != 0)
            {
                // get list of non-cancelled orders that are for screening sessions in the selected cinema
                List<Order> validOrders = GetValidOrdersInCinema(orderList, selectedCinema);

                if (validOrders.Count != 0)
                {
                    // get salesArray
                    int[] salesArray = GetSalesArray(validOrders, screenedMovies);

                    // get highest sales count
                    int highestSales = GetHighest(salesArray);

                    // add movies with highest sales into recommended movie list
                    for (int i = 0; i < salesArray.Length; i++)
                    {
                        int ticketsSold = salesArray[i];

                        if (ticketsSold == highestSales)
                        {
                            mostSoldMovies.Add(screenedMovies[i]);
                        }
                    }
                }
            }

            // returns empty list if no movies are screened in the selected cinema or there are no orders placed
            // returns list of Movie objects with the highest number of tickets sold otherwise
            return mostSoldMovies;
        }

        // method to show recommended movies based on ticket sales
        static void ShowRecommendedMovies(List<Order> orderList, List<Movie> movieList)
        {
            // get list of movies with highest sales
            List<Movie> recommendedMovies = GetRecommendedMovie(orderList, movieList);

            if (recommendedMovies.Count == 0)
            {
                Console.WriteLine("There are no tickets sold yet.");
            }
            else
            {
                DisplayMovies(recommendedMovies);

                // get list of orders that are not cancelled
                List<Order> validOrders = GetValidOrders(orderList);

                int[] salesArray = GetSalesArray(validOrders, movieList);
                int ticketsSold = GetHighest(salesArray);

                Console.WriteLine();
                Console.WriteLine("Tickets sold: {0}", ticketsSold);
            }
        }

        // method to show most sold movie in the selected cinema
        static void ShowMostSoldMovie(List<Order> orderList, List<Movie> movieList, List<Cinema> cinemaList)
        {
            // list cinemas
            DisplayCinemas(cinemaList);

            if (cinemaList.Count != 0)
            {
                Console.WriteLine();

                // let user select a cinema
                Cinema selectedCinema = SelectCinema(cinemaList);

                // get list of movies with highest sales in the cinema
                List<Movie> mostSoldMovies = GetMostSoldMovies(orderList, movieList, selectedCinema);

                if (mostSoldMovies.Count == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("There are no tickets sold yet or there are no screening sessions in the selected cinema.");
                }
                else
                {
                    DisplayMovies(mostSoldMovies);

                    // get list of non-cancelled orders that are for screening sessions in the selected cinema
                    List<Order> validOrders = GetValidOrdersInCinema(orderList, selectedCinema);

                    int[] salesArray = GetSalesArray(validOrders, movieList);
                    int ticketsSold = GetHighest(salesArray);

                    Console.WriteLine();
                    Console.WriteLine("Tickets sold: {0}", ticketsSold);
                }
            }
        }

        // method to show add/remove screening session status
        static void DisplayScreeningStatus(bool status, string action)
        {
            if (status)
            {
                Console.WriteLine("{0} screening successful.", action);
            }
            else
            {
                Console.WriteLine("{0} screening unsuccessful.", action);
            }
        }

        // method to show place/cancel order status
        static void DisplayOrderStatus(bool status, string action)
        {
            if (status)
            {
                Console.WriteLine("{0} order successful.", action);
            }
            else
            {
                Console.WriteLine("{0} order unsuccessful.", action);
            }
        }
    }
}
