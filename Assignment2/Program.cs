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
                FirebaseResponse response = client.Get("users/" + username);

                if (response.Body == "null")
                {
                    Console.WriteLine("No such user exists.");
                    Console.WriteLine("Please re-enter an existing username:");
                    username = Console.ReadLine();
                }
                else
                {
                    usernameLoop = false;
                    user = JsonConvert.DeserializeObject<User>(response.Body);

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
                FirebaseResponse response = client.Get("films/" + title);

                if (response.Body != "null")
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

                    PushResponse response2 = client.Push("films/", film);

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
            String screenNum = Console.ReadLine();

            do
            {
                FirebaseResponse response = client.Get("screens/" + screenNum);

                if (response.Body != "null")
                {
                    Console.WriteLine("A screen with the screen number " + screenNum + " already exists.");
                    Console.WriteLine("Please enter a new screen number:");
                    screenNum = Console.ReadLine();
                }
                else
                {
                    ScreenLoop = false;
                    Console.WriteLine("Please enter the capacity of the new screen:");
                    int capacity;
                    Boolean parse, capacityLoop = true;
                    parse = int.TryParse(Console.ReadLine(), out capacity);

                    while (capacityLoop)
                    {
                        if (parse == false)
                        {
                            Console.WriteLine("Please enter a valid number:");
                            parse = int.TryParse(Console.ReadLine(), out capacity);
                        }
                        else
                        {
                            capacityLoop = false;
                            var screen = new Screen
                            {
                                screenNum = screenNum,
                                capacity = capacity
                            };

                            SetResponse response2 = client.Set("screens/" + screenNum, screen);

                            Console.WriteLine("Screen " + screenNum + " with a capacity of " + capacity + " has been successfully added.");
                        }
                    }               
                }
            }
            while (ScreenLoop);

            managerMain();
        }

        public static void addShowing()
        {
            FirebaseResponse films = client.Get("films");

            dynamic data = JsonConvert.DeserializeObject<dynamic>(films.Body);
            var list = new List<Film>();
            foreach (var itemDynamic in data)
            {
                list.Add(JsonConvert.DeserializeObject<Film>(((JProperty)itemDynamic).Value.ToString()));
            }

            Console.WriteLine("Please choose a film to create a showing:");

            for (int i=0; i<list.Count; i++)
            {
                Console.WriteLine(i+1 + ". " + list[i].title);
            }

            managerMain();
        }

        // For reference on how to make API calls to Firebase
        public static void connect()
        {
            var user = new User
            {
                username = "user1",
                password = "pass1"
            };
            SetResponse response = client.Set("users/user1", user);
        }

        public static void getUser(String username)
        {
            FirebaseResponse response = client.Get("users/" + username);

            User user = JsonConvert.DeserializeObject<User>(response.Body);
        }
    }
}
