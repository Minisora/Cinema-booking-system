using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

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
            Boolean selectionLoop = true, startingLoop = true;

            while (startingLoop)
            {
                selectionLoop = true;
                Console.Clear();

                Console.WriteLine("------------------------------");
                Console.WriteLine("GSC Ticketing System");
                Console.WriteLine("------------------------------");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Exit");
                Console.WriteLine("Please enter a selection:");
                String selection = Console.ReadLine();

                while (selectionLoop)
                {
                    switch (selection)
                    {
                        case "1":
                            selectionLoop = false;
                            login();
                            break;

                        case "2":
                            return;

                        default:
                            Console.WriteLine("Invalid input, please enter a valid choice.");
                            selection = Console.ReadLine();
                            break;
                    }
                }

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
        }

        public static void login()
        {
            String username, password;
            Boolean usernameLoop = true, passwordLoop = true;

            Console.Clear();

            Console.WriteLine("------------------------------");
            Console.WriteLine("System Login");
            Console.WriteLine("------------------------------");
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
            Boolean selectionLoop = true, mainMenuLoop = true;

            while (mainMenuLoop)
            {
                selectionLoop = true;
                Console.Clear();

                Console.WriteLine("------------------------------");
                Console.WriteLine("Customer Main Menu");
                Console.WriteLine("------------------------------");
                Console.WriteLine("1. Purchase advance ticket");
                Console.WriteLine("2. View purchased tickets");
                Console.WriteLine("3. Logout");
                Console.WriteLine("Please enter a selection:");
                String selection = Console.ReadLine();

                while (selectionLoop)
                {
                    switch (selection)
                    {
                        case "1":
                            selectionLoop = false;
                            purchaseTicket();
                            break;

                        case "2":
                            selectionLoop = false;
                            listTickets();
                            break;

                        case "3":
                            return;

                        default:
                            Console.WriteLine("Invalid input, please enter a valid choice.");
                            selection = Console.ReadLine();
                            break;
                    }
                }
            }
        }

        public static void clerkMain()
        {
            Boolean selectionLoop = true, mainMenuLoop = true;

            while (mainMenuLoop)
            {
                selectionLoop = true;
                Console.Clear();

                Console.WriteLine("------------------------------");
                Console.WriteLine("Clerk Main Menu");
                Console.WriteLine("------------------------------");
                Console.WriteLine("1. Purchase tickets");
                Console.WriteLine("2. Logout");
                Console.WriteLine("Please enter a selection:");
                String selection = Console.ReadLine();

                while (selectionLoop)
                {
                    switch (selection)
                    {
                        case "1":
                            selectionLoop = false;
                            purchaseTicket();
                            break;

                        case "2":
                            return;

                        default:
                            Console.WriteLine("Invalid input, please enter a valid choice.");
                            selection = Console.ReadLine();
                            break;
                    }
                }
            }
        }

        public static void managerMain()
        {
            Boolean selectionLoop = true, mainMenuLoop = true;

            while (mainMenuLoop)
            {
                selectionLoop = true;
                Console.Clear();

                Console.WriteLine("------------------------------");
                Console.WriteLine("Manager Main Menu");
                Console.WriteLine("------------------------------");
                Console.WriteLine("1. Add film");
                Console.WriteLine("2. Delete film");
                Console.WriteLine("3. Add screen");
                Console.WriteLine("4. Add showing");
                Console.WriteLine("5. Cancel showing");
                Console.WriteLine("6. Logout");
                Console.WriteLine("Please enter a selection:");
                String selection = Console.ReadLine();

                while (selectionLoop)
                {
                    switch (selection)
                    {
                        case "1":
                            selectionLoop = false;
                            addFilm();
                            break;

                        case "2":
                            selectionLoop = false;
                            deleteFilm();
                            break;

                        case "3":
                            selectionLoop = false;
                            addScreen();
                            break;

                        case "4":
                            selectionLoop = false;
                            addShowing();
                            break;

                        case "5":
                            selectionLoop = false;
                            cancelShowing();
                            break;

                        case "6":
                            return;

                        default:
                            Console.WriteLine("Invalid input, please enter a valid choice.");
                            selection = Console.ReadLine();
                            break;
                    }
                }
            }
        }

        public static void purchaseTicket()
        {
            String title = "";
            String films = getData("films");

            dynamic data = JsonConvert.DeserializeObject<dynamic>(films);
            var filmList = new List<Film>();
            foreach (var itemDynamic in data)
            {
                filmList.Add(JsonConvert.DeserializeObject<Film>(((JProperty)itemDynamic).Value.ToString()));
            }

            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Purchase Ticket");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Please choose a film:");
            int count = filmList.Count;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(i + 1 + ". " + filmList[i].title);
            }
            Console.WriteLine(count + 1 + ". Exit to main menu");

            int selection;
            Boolean parse, loop = true;
            parse = int.TryParse(Console.ReadLine(), out selection);

            while (loop)
            {
                if (selection == count + 1)
                {
                    return;
                }
                if (parse == false || selection > count || selection < 1)
                {
                    Console.WriteLine("Please enter a valid choice:");
                    parse = int.TryParse(Console.ReadLine(), out selection);
                }
                else
                {
                    title = filmList[selection - 1].title;
                }

                String showings = getData("showing/" + title);

                data = JsonConvert.DeserializeObject<dynamic>(showings);

                if (data == null)
                {
                    Console.WriteLine("There are no showings for " + title + ".");
                    Console.WriteLine("Press select another option.");
                    parse = int.TryParse(Console.ReadLine(), out selection);
                }
                else
                {
                    loop = false;
                    var showingList = new List<Showing>();
                    foreach (var itemDynamic in data)
                    {
                        showingList.Add(JsonConvert.DeserializeObject<Showing>(((JProperty)itemDynamic).Value.ToString()));
                    }

                    count = showingList.Count;
                    Console.Clear();

                    Console.WriteLine("------------------------------");
                    Console.WriteLine("Showings for " + title);
                    Console.WriteLine("------------------------------");
                    Console.WriteLine("Please choose a showing:");

                    for (int i = 0; i < count; i++)
                    {
                        Console.WriteLine(i + 1 + ". " + showingList[i].dateTime);
                    }
                    Console.WriteLine(count + 1 + ". Exit to main menu");

                    loop = true;
                    parse = int.TryParse(Console.ReadLine(), out selection);

                    Showing showing = new Showing
                    {
                        dateTime = new DateTime(),
                        duration = 0,
                        title = "",
                        screenNum = 0,
                        ticketsPurchased = 0
                    };

                    int student = 0, child = 0, adult = 0;
                    while (loop)
                    {
                        if (selection == count + 1)
                        {
                            return;
                        }
                        if (parse == false || selection > count || selection < 1)
                        {
                            Console.WriteLine("Please enter a valid choice:");
                            parse = int.TryParse(Console.ReadLine(), out selection);
                        }
                        else
                        {
                            loop = false;
                            Boolean loop2 = true;

                            showing.dateTime = showingList[selection - 1].dateTime;
                            showing.duration = showingList[selection - 1].duration;
                            showing.title = showingList[selection - 1].title;
                            showing.screenNum = showingList[selection - 1].screenNum;
                            showing.ticketsPurchased = showingList[selection - 1].ticketsPurchased;

                            String response = getData("screens/" + showing.screenNum.ToString());

                            Screen screen = JsonConvert.DeserializeObject<Screen>(response);
                            int available = screen.capacity - showing.ticketsPurchased;

                            if (available == 0)
                            {
                                Console.WriteLine("There are no tickets available for purchase.");
                                Console.WriteLine("Press any key to return to the main menu.");
                                Console.ReadKey();
                                return;
                            }

                            DateTimeFormatInfo format = new DateTimeFormatInfo();
                            format.DateSeparator = "-";
                            String datetime = String.Format(format, "{0:dd/MMM/yy h:mm:ss tt}", showing.dateTime);
                            Console.Clear();
                            Console.WriteLine("-------------------------------------");
                            Console.WriteLine(title + " " + datetime);
                            Console.WriteLine("-------------------------------------");

                            Console.WriteLine("Please choose the number of student tickets to purchase: (" + available + " tickets available, RM 10.00 each))");
                            parse = int.TryParse(Console.ReadLine(), out selection);

                            while (loop2)
                            {
                                if (parse == false || selection < 0 || selection > available)
                                {
                                    Console.WriteLine("Please enter a valid number:");
                                    parse = int.TryParse(Console.ReadLine(), out selection);
                                }
                                else
                                {
                                    loop2 = false;
                                    showing.ticketsPurchased += selection;
                                    available -= selection;
                                    student = selection;
                                }
                            }

                            loop2 = true;

                            Console.WriteLine("Please choose the number of child tickets to purchase: (" + available + " tickets available, RM 8.00 each)");
                            parse = int.TryParse(Console.ReadLine(), out selection);

                            while (loop2)
                            {
                                if (parse == false || selection < 0 || selection > available)
                                {
                                    Console.WriteLine("Please enter a valid number:");
                                    parse = int.TryParse(Console.ReadLine(), out selection);
                                }
                                else
                                {
                                    loop2 = false;
                                    showing.ticketsPurchased += selection;
                                    available -= selection;
                                    child = selection;
                                }
                            }

                            loop2 = true;

                            Console.WriteLine("Please choose the number of adult tickets to purchase: (" + available + " tickets available, RM 13.00 each))");
                            parse = int.TryParse(Console.ReadLine(), out selection);

                            while (loop2)
                            {
                                if (parse == false || selection < 0 || selection > available)
                                {
                                    Console.WriteLine("Please enter a valid number:");
                                    parse = int.TryParse(Console.ReadLine(), out selection);
                                }
                                else
                                {
                                    loop2 = false;
                                    showing.ticketsPurchased += selection;
                                    available -= selection;
                                    adult = selection;
                                }
                            }
                        }
                    }

                    if ((student + child + adult) == 0)
                    {
                        Console.WriteLine("You did not purchase any tickets. Please press any key to return to the main menu.");
                        Console.ReadKey();
                        return;
                    }

                    Console.Clear();
                    Console.WriteLine("------------------------------");
                    Console.WriteLine("Tickets purchased");
                    Console.WriteLine("------------------------------");

                    if (student > 0)
                    {
                        Console.WriteLine("Student tickets purchased:\t" + student + "\tx RM  8.00 = RM " + (student * 8) + ".00");
                    }

                    if (child > 0)
                    {
                        Console.WriteLine("Child tickets purchased:\t" + child + "\tx RM 10.00 = RM " + (child * 10) + ".00");
                    }

                    if (adult > 0)
                    {
                        Console.WriteLine("Adult tickets purchased:\t" + adult + "\tx RM 13.00 = RM " + (adult * 13) + ".00");
                    }
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine("Total price:\t\t\t\t\t     RM " + ((student * 8) + (child * 10) + (adult * 13)) + ".00");

                    loop = true;
                    Console.WriteLine("Please confirm that you have chosen the correct amount of tickets. Press enter to continue to payment, escape to cancel.");

                    while (loop)
                    {
                        ConsoleKey key = Console.ReadKey().Key;
                        if (key.Equals(ConsoleKey.Escape))
                        {
                            return;
                        }
                        if (key.Equals(ConsoleKey.Enter))
                        {
                            loop = false;
                            //payment method

                            if (user.type.Equals("clerk"))
                            {
                                Console.Clear();
                                Console.WriteLine("------------------------------");
                                Console.WriteLine("Payment Method");
                                Console.WriteLine("------------------------------");
                                Console.WriteLine("Total price: RM " + ((student * 8) + (child * 10) + (adult * 13)) + ".00");
                                Console.WriteLine("Please choose a payment method:");
                                Console.WriteLine("1. Credit card");
                                Console.WriteLine("2. Payment card");
                                Console.WriteLine("3. Cash");
                                Console.WriteLine("4. Cancel payment");

                                Boolean loop2 = true;
                                parse = int.TryParse(Console.ReadLine(), out selection);

                                while (loop2)
                                {
                                    if (selection == 4)
                                    {
                                        return;
                                    }
                                    if (parse == false || selection > 3 || selection < 1)
                                    {
                                        Console.WriteLine("Please enter a valid choice:");
                                        parse = int.TryParse(Console.ReadLine(), out selection);
                                    }
                                    else if (selection == 1)
                                    {
                                        loop2 = false;
                                        payCreditCard(student, child, adult, 1);
                                    }
                                    else if (selection == 2)
                                    {
                                        loop2 = false;
                                        payCreditCard(student, child, adult, 2);
                                    }
                                    else
                                    {
                                        loop2 = false;
                                        Console.WriteLine("Please press enter to confirm that cash payment has been made. Press escape to cancel.");
                                        Boolean loop3 = true;
                                        while (loop3)
                                        {
                                            key = Console.ReadKey().Key;
                                            if (key.Equals(ConsoleKey.Escape))
                                            {
                                                return;
                                            }
                                            if (key.Equals(ConsoleKey.Enter))
                                            {
                                                loop3 = false;
                                                Console.WriteLine("Cash payment confirmed.");
                                            }
                                            Console.SetCursorPosition(0, Console.CursorTop);
                                            Console.Write(new string(' ', Console.WindowWidth));
                                            Console.SetCursorPosition(0, Console.CursorTop - 1);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                payCreditCard(student, child, adult, 1);                          
                            }
                            Console.WriteLine("Purchasing tickets...");                           
                        }
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                    }

                    DateTimeFormatInfo formatInfo = new DateTimeFormatInfo();
                    formatInfo.DateSeparator = "-";
                    String dateTime = String.Format(formatInfo, "{0:dd/MMM/yy h:mm:ss tt}", showing.dateTime);
                    //update tickets purchased          
                    setData("showings/" + showing.screenNum + "/" + dateTime, showing);
                    setData("showing/" + showing.title + "/" + dateTime, showing);

                    //create ticket ID and info
                    Ticket ticket = new Ticket();
                    ticket.movie = showing.title;
                    ticket.dateTime = showing.dateTime;
                    ticket.studentNumber = student;
                    ticket.childNumber = child;
                    ticket.adultNumber = adult;
                    String res = pushData("tickets/" + user.username, ticket);
                    TicketID ticketID = new TicketID();
                    ticketID = JsonConvert.DeserializeObject<TicketID>(res);

                    ticket.id = ticketID.name;
                    setData("tickets/" + user.username + "/" + ticketID.name, ticket);

                    Console.WriteLine((student + child + adult) + " tickets purchased for " + showing.title + " on " + dateTime);
                    Console.WriteLine("Your ticket ID is: " + ticketID.name);

                    Console.WriteLine("Press any key to return to the main menu.");
                    Console.ReadKey();
                }
            }
        }

        public static void payCreditCard(int student, int child, int adult, int type)
        {
            Console.Clear();
            Console.WriteLine("------------------------------");
            if (type == 1)
            {
                Console.WriteLine("Credit Card Payment");
            }
            else
            {
                Console.WriteLine("Payment Card");
            }
            Console.WriteLine("------------------------------");
            Console.WriteLine("Total price: RM " + ((student * 8) + (child * 10) + (adult * 13)) + ".00");

            if (type == 1)
            {
                Console.WriteLine("Please enter the credit card information:");
            }
            else
            {
                Console.WriteLine("Please enter the payment card information:");
            }
            
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Boolean loop = true;
            Boolean parse = false;
            long card = 0;

            if (type == 1)
            {
                Console.Write("Credit card number: ");
            }
            else
            {
                Console.Write("Payment card number: ");
            }
            
            String cardNumber = Regex.Replace(Console.ReadLine(), @"\s+", "");
            parse = long.TryParse(cardNumber, out card);       

            while (loop)
            {
                if (parse == false || cardNumber.Length != 16)
                {
                    Console.Write("Please enter a valid credit card number: ");
                    cardNumber = Regex.Replace(Console.ReadLine(), @"\s+", "");
                    parse = long.TryParse(cardNumber, out card);
                }
                else
                {
                    loop = false;
                }
            }

            Console.Write("Expiry date: (MM/YY)");
            String expiry = Console.ReadLine();

            String[] formats =
            { "M/yyyy",
              "M/yyyy",
              "MM/yyyy",
              "MM/yyyy",
              "M/yy",
              "M/yy",
              "MM/yy",
              "MM/yy" };

            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime dateTime = new DateTime();

            parse = DateTime.TryParseExact(expiry, formats, provider, DateTimeStyles.AllowWhiteSpaces, out dateTime);
            loop = true;

            while (loop)
            {
                if (parse == false || dateTime.CompareTo(DateTime.Now) < 0)
                {
                    Console.WriteLine("Please enter a valid date:");
                    expiry = Console.ReadLine();
                    parse = DateTime.TryParseExact(expiry, formats, provider, DateTimeStyles.None, out dateTime);
                }
                else
                {
                    loop = false;
                }
            }
        }

        public static void listTickets()
        {
            String ticketIDs = getData("tickets/" + user.username);

            if (ticketIDs.Equals(null) || ticketIDs == "null")
            {
                Console.WriteLine("You have not purchased any tickets.");
                Console.WriteLine("Press any key to return to the main menu.");
                Console.ReadKey();
                return;
            }

            dynamic data = JsonConvert.DeserializeObject<dynamic>(ticketIDs);
            var ticketList = new List<Ticket>();
            foreach (var itemDynamic in data)
            {
                string test = (((JProperty)itemDynamic).Value.ToString());
                ticketList.Add(JsonConvert.DeserializeObject<Ticket>(((JProperty)itemDynamic).Value.ToString()));
            }

            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Purchased Tickets");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Please select a ticket ID:");
            int count = ticketList.Count;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(i + 1 + ". " + ticketList[i].id);
            }
            Console.WriteLine(count + 1 + ". Exit to main menu");

            int selection;
            Boolean parse, loop = true;
            parse = int.TryParse(Console.ReadLine(), out selection);

            while (loop)
            {
                if (selection == count + 1)
                {
                    return;
                }
                if (parse == false || selection > count || selection < 1)
                {
                    Console.WriteLine("Please enter a valid choice:");
                    parse = int.TryParse(Console.ReadLine(), out selection);
                }
                else
                {
                    loop = false;

                    DateTimeFormatInfo formatInfo = new DateTimeFormatInfo();
                    formatInfo.DateSeparator = "-";
                    String dateTime = String.Format(formatInfo, "{0:dd/MMM/yy h:mm:ss tt}", ticketList[selection - 1].dateTime);

                    Console.Clear();
                    Console.WriteLine("------------------------------");
                    Console.WriteLine("Ticket Information");
                    Console.WriteLine("------------------------------");
                    Console.WriteLine("Ticket ID: " + ticketList[selection - 1].id);
                    Console.WriteLine("Movie: " + ticketList[selection - 1].movie);
                    Console.WriteLine("Date and time: " + dateTime);

                    if (ticketList[selection - 1].studentNumber != 0)
                    {
                        Console.WriteLine("Student tickets purchased: " + ticketList[selection - 1].studentNumber);
                    }
                    if (ticketList[selection - 1].childNumber != 0)
                    {
                        Console.WriteLine("Child tickets purchased: " + ticketList[selection - 1].childNumber);
                    }
                    if (ticketList[selection - 1].adultNumber != 0)
                    {
                        Console.WriteLine("Adult tickets purchased: " + ticketList[selection - 1].adultNumber);
                    }

                    Console.WriteLine("\nPress any key to return to the main menu.");
                    Console.ReadKey();                  
                }
            }
        }

        public static void addFilm()
        {
            Boolean FilmLoop = true;
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Add Film");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Enter the new film title:");
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

                    setData("films/" + title, film);

                    Console.WriteLine("The new film " + title + " has been successfully added.");
                    Console.WriteLine("Please press any key to return to the main menu.");
                    Console.ReadKey();
                }
            }
            while (FilmLoop);
        }

        public static void deleteFilm()
        {
            String films = getData("films");

            dynamic data = JsonConvert.DeserializeObject<dynamic>(films);
            var filmList = new List<Film>();
            foreach (var itemDynamic in data)
            {
                filmList.Add(JsonConvert.DeserializeObject<Film>(((JProperty)itemDynamic).Value.ToString()));
            }

            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Delete Film");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Please choose a film to delete:");
            int count = filmList.Count;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(i + 1 + ". " + filmList[i].title);
            }
            Console.WriteLine(count + 1 + ". Exit to main menu");

            int selection;
            Boolean parse, loop = true;
            parse = int.TryParse(Console.ReadLine(), out selection);

            var film = new Film
            {
                title = "",
                ageRating = "",
                duration = 0,
                trailer = ""
            };

            while (loop)
            {
                if (selection == count + 1)
                {
                    return;
                }
                if (parse == false || selection > count || selection < 1)
                {
                    Console.WriteLine("Please enter a valid choice:");
                    parse = int.TryParse(Console.ReadLine(), out selection);
                }
                else
                {
                    String showing = getData("showing/" + filmList[selection - 1].title);
                    if (showing != "null")
                    {
                        Console.WriteLine("The selected film has existing showings. Film cannot be deleted.");
                        Console.WriteLine("Please select another option.");
                        parse = int.TryParse(Console.ReadLine(), out selection);
                    }
                    else
                    {
                        loop = false;

                        film.title = filmList[selection - 1].title;
                        film.ageRating = filmList[selection - 1].ageRating;
                        film.duration = filmList[selection - 1].duration;
                        film.trailer = filmList[selection - 1].trailer;
                    }
                }
            }
            deleteData("films/" + film.title);

            Console.WriteLine("Film " + film.title + " has been deleted.");
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
        }

        public static void addScreen()
        {
            String screens = getData("screens");

            dynamic data = JsonConvert.DeserializeObject<dynamic>(screens);
            var screenList = new List<Screen>();
            foreach (var itemDynamic in data)
            {
                screenList.Add(JsonConvert.DeserializeObject<Screen>(((JProperty)itemDynamic).Value.ToString()));
            }

            Boolean ScreenLoop = true;
            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Add Screen");
            Console.WriteLine("------------------------------");
            Console.Write("Existing screen numbers: ");
            int count = screenList.Count;
            for (int i = 0; i < count; i++)
            {
                if ((i + 1) != count)
                {
                    Console.Write(screenList[i].screenNum + ", ");
                }
                else
                {
                    Console.Write(screenList[i].screenNum + "\n");
                }
            }
            Console.WriteLine("Enter the new screen number:");
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
                    for (int i = 0; i < count; i++)
                    {
                        if (screenList[i].screenNum == screenNum)
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
                                    Console.WriteLine("Please press any key to return to the main menu.");
                                    Console.ReadKey();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            while (ScreenLoop);
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

            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Add Showing");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Please choose a film to create a showing:");
            int count = filmList.Count;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(i + 1 + ". " + filmList[i].title);
            }
            Console.WriteLine(count + 1 + ". Exit to main menu");

            int selection;
            Boolean parse, loop = true;
            parse = int.TryParse(Console.ReadLine(), out selection);

            var film = new Film
            {
                title = "",
                ageRating = "",
                duration = 0,
                trailer = ""
            };

            while (loop)
            {
                if (selection == count + 1)
                {
                    return;
                }
                if (parse == false || selection > count || selection < 1)
                {
                    Console.WriteLine("Please enter a valid choice:");
                    parse = int.TryParse(Console.ReadLine(), out selection);
                }
                else
                {
                    loop = false;

                    film.title = filmList[selection - 1].title;
                    film.ageRating = filmList[selection - 1].ageRating;
                    film.duration = filmList[selection - 1].duration;
                    film.trailer = filmList[selection - 1].trailer;
                }
            }

            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Add Showing for " + film.title);
            Console.WriteLine("------------------------------");
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

            String datetime = "";
            CultureInfo provider = CultureInfo.InvariantCulture;

            parse = DateTime.TryParseExact(date, formats, provider, DateTimeStyles.AllowWhiteSpaces, out dateTime);

            while (loop)
            {
                if (parse == false || dateTime.CompareTo(DateTime.Now) < 0)
                {
                    Console.WriteLine("Please enter a valid date:");
                    date = Console.ReadLine();
                    parse = DateTime.TryParseExact(date, formats, provider, DateTimeStyles.None, out dateTime);
                }
                else
                {
                    loop = false;
                    DateTimeFormatInfo formatInfo = new DateTimeFormatInfo();
                    formatInfo.DateSeparator = "-";
                    datetime = String.Format(formatInfo, "{0:dd/MMM/yy h:mm:ss tt}", dateTime);
                    Console.WriteLine(datetime);
                }
            }

            //get list of screens
            String screens = getData("screens");

            data = JsonConvert.DeserializeObject<dynamic>(screens);
            var screenList = new List<Screen>();
            foreach (var itemDynamic in data)
            {
                screenList.Add(JsonConvert.DeserializeObject<Screen>(((JProperty)itemDynamic).Value.ToString()));
            }

            Console.Write("Existing screen numbers: ");
            count = screenList.Count;
            for (int i = 0; i < count; i++)
            {
                if ((i + 1) != count)
                {
                    Console.Write(screenList[i].screenNum + ", ");
                }
                else
                {
                    Console.Write(screenList[i].screenNum + "\n");
                }
            }
            Console.WriteLine("Enter a screen number for the new showing:");

            Boolean screenLoop = true;
            int screenNum;
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
                    Boolean screenCheck = false;
                    for (int i = 0; i < count; i++)
                    {
                        if (screenList[i].screenNum == screenNum)
                        {
                            screenCheck = true;
                            screenLoop = false;

                        }
                    }

                    if (screenCheck == false)
                    {
                        Console.WriteLine("A screen with the screen number " + screenNum + " does not exist.");
                        Console.WriteLine("Please enter an existing screen number:");
                        parse = int.TryParse(Console.ReadLine(), out screenNum);
                    }
                }
            }
            while (screenLoop);

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

            for (int i = 0; i < showList.Count; i++)
            {
                DateTime dTime = showList[i].dateTime;
                int duration = showList[i].duration;

                if (dateTime.CompareTo(dTime) == 1) //dateTime is later than dTime
                {
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

                setData("showings/" + screenNum + "/" + datetime, showing);
                setData("showing/" + showing.title + "/" + datetime, showing);
                Console.WriteLine("Showing for " + film.title + " added.");
                Console.WriteLine("Please press any key to return to the main menu.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid time slot.");
                Console.WriteLine("Please press any key to return to the main menu.");
                Console.ReadKey();
            }
        }

        public static void cancelShowing()
        {
            String films = getData("films");

            dynamic data = JsonConvert.DeserializeObject<dynamic>(films);
            var filmList = new List<Film>();

            foreach (var itemDynamic in data)
            {
                filmList.Add(JsonConvert.DeserializeObject<Film>(((JProperty)itemDynamic).Value.ToString()));
            }

            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine("Cancel Showing");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Please choose a film to delete a showing:");
            int count = filmList.Count;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(i + 1 + ". " + filmList[i].title);
            }
            Console.WriteLine(count + 1 + ". Exit to main menu");

            int selection;
            Boolean parse, loop = true;
            parse = int.TryParse(Console.ReadLine(), out selection);

            var film = new Film
            {
                title = "",
                ageRating = "",
                duration = 0,
                trailer = ""
            };

            while (loop)
            {
                if (selection == count + 1)
                {
                    return;
                }
                if (parse == false || selection > count || selection < 1)
                {
                    Console.WriteLine("Please enter a valid choice:");
                    parse = int.TryParse(Console.ReadLine(), out selection);
                }
                else
                {
                    String showings = getData("showing/" + filmList[selection - 1].title);

                    data = JsonConvert.DeserializeObject<dynamic>(showings);

                    if (data == null)
                    {
                        Console.WriteLine("The selected screen has no showings.");
                        Console.WriteLine("Please select another option.");
                        parse = int.TryParse(Console.ReadLine(), out selection);
                    }
                    else
                    {
                        loop = false;

                        film.title = filmList[selection - 1].title;
                        film.ageRating = filmList[selection - 1].ageRating;
                        film.duration = filmList[selection - 1].duration;
                        film.trailer = filmList[selection - 1].trailer;
                    }
                }
            }

            var showList = new List<Showing>();
            foreach (var itemDynamic in data)
            {
                showList.Add(JsonConvert.DeserializeObject<Showing>(((JProperty)itemDynamic).Value.ToString()));
            }

            Console.Clear();
            Console.WriteLine("------------------------------");
            Console.WriteLine(filmList[selection - 1].title + " showings");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Please choose a showing to delete:");
            count = showList.Count;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(i + 1 + ". " + showList[i].dateTime);
            }
            Console.WriteLine(count + 1 + ". Exit to main menu");

            loop = true;
            parse = int.TryParse(Console.ReadLine(), out selection);

            var showing = new Showing
            {
                dateTime = new DateTime(),
                duration = 0,
                title = "",
                screenNum = 0,
                ticketsPurchased = 0
            };

            while (loop)
            {
                if (selection == count + 1)
                {
                    return;
                }
                if (parse == false || selection > count || selection < 1)
                {
                    Console.WriteLine("Please enter a valid choice:");
                    parse = int.TryParse(Console.ReadLine(), out selection);
                }
                else
                {
                    if (showList[selection - 1].ticketsPurchased > 0)
                    {
                        Console.WriteLine("The selected showing has sold tickets. Unable to delete showing.");
                        Console.WriteLine("Please select another option.");
                        parse = int.TryParse(Console.ReadLine(), out selection);
                    }
                    else
                    {
                        loop = false;

                        showing.dateTime = showList[selection - 1].dateTime;
                        showing.duration = showList[selection - 1].duration;
                        showing.title = showList[selection - 1].title;
                        showing.screenNum = showList[selection - 1].screenNum;
                        showing.ticketsPurchased = showList[selection - 1].ticketsPurchased;

                        Console.WriteLine("You have chosen a showing with the following details:");
                        Console.WriteLine("Date and time: " + showing.dateTime);
                        Console.WriteLine("Duration: " + showing.duration);
                        Console.WriteLine("Title: " + showing.title);
                        Console.WriteLine("Screen number: " + showing.screenNum);
                        Console.WriteLine("Tickets purchased: " + showing.ticketsPurchased);
                    }
                }
            }

            deleteData("showing/" + film.title + "/" + showing.dateTime);
            deleteData("showings/" + showing.screenNum + "/" + showing.dateTime);

            Console.WriteLine("Showing deleted successfully.");
            Console.WriteLine("Please press any key to return to the main menu.");
            Console.ReadKey();
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

        public static String pushData(String path, Object obj)
        {
            PushResponse response = client.Push(path, obj);
            return response.Body;
        }

        public static void deleteData(String path)
        {
            FirebaseResponse response = client.Delete(path);
        }
    }
}