using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Assignment2
{
    class Program
    {
        static IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "XG17yyJjcmzzsBdqPWbbhiEj78JlrnwoNEDMfeab",
            BasePath = "https://cinema-booking-syste.firebaseio.com/"
        };

        static IFirebaseClient client = new FirebaseClient(config);

        static User user;

        static void Main(string[] args)
        {
            login();

            switch (user.type)
            {
                case "customer":
                    customerMain();
                    break;

                case "clerk":
                    clerkMain();
                    break;

                case "manager":
                    managerMain();
                    break;
            }
        }

        public static void login()
        {
            String username, password;
            Boolean usernameLoop = true, passwordLoop = true;

            Console.WriteLine("Welcome to the GSC Cinema Booking System.");
            Console.WriteLine("Please enter your username:");
            username = Console.ReadLine();

            do
            {
                String response = getData("users/" + username);

                if (response == "null")
                {
                    Console.WriteLine("No such user exists.");
                    Console.WriteLine("Please re-enter an existing username:");
                    username = Console.ReadLine();
                }
                else
                {
                    usernameLoop = false;
                    user = JsonConvert.DeserializeObject<User>(response);

                    Console.WriteLine("Please enter your password:");
                    password = Console.ReadLine();

                    while (passwordLoop)
                    {
                        if (password != user.password)
                        {
                            Console.WriteLine("Invalid password!");
                            Console.WriteLine("Please enter a valid password for the username " + username + ":");
                            password = Console.ReadLine();
                        }
                        else
                        {
                            passwordLoop = false;
                            Console.WriteLine("Login successful!");
                            Console.WriteLine("Welcome, " + username + ".");
                        }
                    }
                }
            }
            while (usernameLoop);
        }

        public static void customerMain()
        {
            Boolean loop = true;

            Console.WriteLine("\nMain menu:");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1. Purchase advance ticket");
            Console.WriteLine("2. Exit");
            Console.WriteLine("Please enter a selection:");
            String selection = Console.ReadLine();

            while (loop)
            {
                switch (selection)
                {
                    case "1":
                        loop = false;
                        break;

                    case "2":
                        loop = false;
                        return;

                    default:
                        Console.WriteLine("Invalid input, please enter a valid choice.");
                        selection = Console.ReadLine();
                        break;
                }
            }
        }

        public static void clerkMain()
        {
            Boolean loop = true;

            Console.WriteLine("\nMain menu:");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1. Purchase tickets");
            Console.WriteLine("2. Exit");
            Console.WriteLine("Please enter a selection:");
            String selection = Console.ReadLine();

            while (loop)
            {
                switch (selection)
                {
                    case "1":
                        loop = false;
                        break;

                    case "2":
                        loop = false;
                        return;

                    default:
                        Console.WriteLine("Invalid input, please enter a valid choice.");
                        selection = Console.ReadLine();
                        break;
                }
            }
        }

        public static void managerMain()
        {
            Boolean loop = true;

            Console.WriteLine("\nMain menu:");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1. Add film");
            Console.WriteLine("2. Delete film");
            Console.WriteLine("3. Add screen");
            Console.WriteLine("4. Add showing");
            Console.WriteLine("5. Cancel showing");
            Console.WriteLine("6. Exit");
            Console.WriteLine("Please enter a selection:");
            String selection = Console.ReadLine();

            while (loop)
            {
                switch (selection)
                {
                    case "1":
                        loop = false;
                        addFilm();
                        break;

                    case "2":
                        loop = false;
                        break;

                    case "3":
                        loop = false;
                        addScreen();
                        break;

                    case "4":
                        loop = false;
                        addShowing();
                        break;

                    case "5":
                        loop = false;
                        break;

                    case "6":
                        loop = false;
                        return;

                    default:
                        Console.WriteLine("Invalid input, please enter a valid choice.");
                        selection = Console.ReadLine();
                        break;
                }
            }
        }

        public static void addFilm()
        {
            Boolean FilmLoop = true;
            Console.WriteLine("\nEnter the new film title:");
            String title = Console.ReadLine();

            do
            {
                String response = getData("films/" + title);

                if (response != "null")
                {
                    Console.WriteLine("A film with the title " + title + " already exists.");
                    Console.WriteLine("Please enter a new film title:");
                    title = Console.ReadLine();
                }
                else
                {
                    FilmLoop = false;
                    Console.WriteLine("Please enter the age rating of the new film:");
                    String ageRating = Console.ReadLine();

                    Console.WriteLine("Please enter the duration of the new film in minutes:");
                    int duration;
                    Boolean durationLoop = true, parse = int.TryParse(Console.ReadLine(), out duration);

                    while (durationLoop)
                    {
                        if (parse == false)
                        {
                            Console.WriteLine("Please enter a valid duration in minutes:");
                            parse = int.TryParse(Console.ReadLine(), out duration);
                        }
                        else
                        {
                            durationLoop = false;
                        }
                    }
                    
                    Console.WriteLine("Please enter a short trailer description for the new film:");
                    String trailer = Console.ReadLine();

                    var film = new Film
                    {
                        title = title,
                        ageRating = ageRating,
                        duration = duration,
                        trailer = trailer
                    };

                    setData("films/", film);

                    Console.WriteLine("The new film " + title + " has been successfully added.");
                }
            }
            while (FilmLoop);

            managerMain();
        }

        public static void addScreen()
        {
            Boolean ScreenLoop = true;
            Console.WriteLine("\nEnter the new screen number:");
            int screenNum;
            Boolean parse; 
            parse = int.TryParse(Console.ReadLine(), out screenNum);

            do
            {
                if (screenNum < 1 || parse == false)
                {
                    Console.WriteLine("Please enter a valid screen number:");
                    parse = int.TryParse(Console.ReadLine(), out screenNum);
                }
                else
                {
                    String response = getData("screens/" + screenNum);

                    if (response != "null")
                    {
                        Console.WriteLine("A screen with the screen number " + screenNum + " already exists.");
                        Console.WriteLine("Please enter a new screen number:");
                        parse = int.TryParse(Console.ReadLine(), out screenNum);
                    }
                    else
                    {
                        ScreenLoop = false;
                        Console.WriteLine("Please enter the capacity of the new screen:");
                        int capacity;
                        Boolean parse2, capacityLoop = true;
                        parse2 = int.TryParse(Console.ReadLine(), out capacity);

                        while (capacityLoop)
                        {
                            if (parse2 == false || capacity < 1)
                            {
                                Console.WriteLine("Please enter a valid number:");
                                parse2 = int.TryParse(Console.ReadLine(), out capacity);
                            }
                            else
                            {
                                capacityLoop = false;
                                var screen = new Screen
                                {
                                    screenNum = screenNum,
                                    capacity = capacity
                                };

                                setData("screens/" + screenNum, screen);

                                Console.WriteLine("Screen " + screenNum + " with a capacity of " + capacity + " has been successfully added.");
                            }
                        }
                    }
                }
            }
            while (ScreenLoop);

            managerMain();
        }

        public static void addShowing()
        {
            String films = getData("films");

            dynamic data = JsonConvert.DeserializeObject<dynamic>(films);
            var filmList = new List<Film>();
            foreach (var itemDynamic in data)
            {
                filmList.Add(JsonConvert.DeserializeObject<Film>(((JProperty)itemDynamic).Value.ToString()));
            }

            Console.WriteLine("Please choose a film to create a showing:");
            int count = filmList.Count;
            for (int i=0; i<count; i++)
            {
                Console.WriteLine(i+1 + ". " + filmList[i].title);
            }

            int selection;
            Boolean parse, loop = true;
            parse = int.TryParse(Console.ReadLine(), out selection);

            var film = new Film
            {
                title = filmList[selection - 1].title,
                ageRating = filmList[selection - 1].ageRating,
                duration = filmList[selection - 1].duration,
                trailer = filmList[selection - 1].trailer
            };

            while (loop)
            {
                if (parse == false || selection > count || selection < 1)
                {
                    Console.WriteLine("Please enter a valid choice:");
                    parse = int.TryParse(Console.ReadLine(), out selection);
                }
                else
                {
                    loop = false;
                    Console.WriteLine("You have chosen a film with the following details:");
                    Console.WriteLine("Title: " + film.title);
                    Console.WriteLine("Age rating: " + film.ageRating);
                    Console.WriteLine("Duration: " + film.duration + " minutes");
                    Console.WriteLine("Short description: " + film.trailer);
                }
            }

            Console.WriteLine("Enter a date and time to create a showing for " + film.title + " in the format DD/MM/YYYY hhmm:");
            String date = Console.ReadLine();
            DateTime dateTime;
            parse = false;
            loop = true;

            String[] formats = 
            { "d/M/yyyy HHmm",
              "dd/M/yyyy HHmm",
              "d/MM/yyyy HHmm",
              "dd/MM/yyyy HHmm",
              "d/M/yy HHmm",
              "dd/M/yy HHmm",
              "d/MM/yy HHmm",
              "dd/MM/yy HHmm" };

            CultureInfo provider = CultureInfo.InvariantCulture;

            parse = DateTime.TryParseExact(date, formats, provider, DateTimeStyles.AllowWhiteSpaces, out dateTime);

            while (loop)
            {
                if (parse == false)
                {
                    Console.WriteLine("Please enter a valid date:");
                    date = Console.ReadLine();
                    parse = DateTime.TryParseExact(date, formats, provider, DateTimeStyles.None, out dateTime);
                }
                else
                {
                    loop = false;
                    String.Format("{0:dd/MM/yyyy HHmm}", dateTime);
                    Console.WriteLine(dateTime);
                }
            }

            Console.WriteLine("Enter a screen number for the new showing:");
            int screenNum;
            parse = int.TryParse(Console.ReadLine(), out screenNum);

            String showings = getData("showings/" + screenNum);

            data = JsonConvert.DeserializeObject<dynamic>(showings);

            var showList = new List<Showing>();

            if (data != null)
            {
                foreach (var itemDynamic in data)
                {
                    showList.Add(JsonConvert.DeserializeObject<Showing>(((JProperty)itemDynamic).Value.ToString()));
                }
            } 

            Boolean check = true;

            for (int i=0; i<showList.Count; i++)
            {
                DateTime dTime = showList[i].dateTime;
                int duration = showList[i].duration;

                Console.WriteLine(dateTime);
                Console.WriteLine(dTime);
                Console.WriteLine("1. " + dateTime.CompareTo(dTime));

                if (dateTime.CompareTo(dTime) == 1) //dateTime is later than dTime
                {
                    Console.WriteLine("Later");
                    Console.WriteLine(duration);

                    Console.WriteLine(dateTime);
                    Console.WriteLine(dTime.AddMinutes(duration));
                    Console.WriteLine("2. " + dateTime.CompareTo(dTime.AddMinutes(duration)));

                    if (dateTime.CompareTo(dTime.AddMinutes(duration)) == -1 || dateTime.CompareTo(dTime.AddMinutes(duration)) == 0) //dateTime is earlier or same than dTime
                    {
                        check = false;
                        break;
                    }
                }
                else if (dateTime.CompareTo(dTime) == 0)
                {
                    check = false;
                    break;
                }
                else //dateTime is earlier then dTime
                {
                    if (dTime.CompareTo(dateTime.AddMinutes(film.duration)) == -1 || dTime.CompareTo(dateTime.AddMinutes(film.duration)) == 0)
                    {
                        check = false;
                        break;
                    }
                }
            }

            if (check == true)
            {
                Showing showing = new Showing
                {
                    dateTime = dateTime,
                    duration = film.duration,
                    title = film.title,
                    screenNum = screenNum,
                    ticketsPurchased = 0
                };

                pushData("showings/" + screenNum , showing);
                Console.WriteLine("Showing for " + film.title + " added.");
            }
            else
            {
                Console.WriteLine("Invalid time slot.");
            }
            managerMain();
        }

        public static String getData(String path)
        {
            FirebaseResponse response = client.Get(path);

            return response.Body;
        }

        public static void setData(String path, Object obj)
        {
            SetResponse response = client.Set(path, obj);
        }

        public static void pushData(String path, Object obj)
        {
            PushResponse response = client.Push(path, obj);
        }
    }
}
