using HMS.classes;

namespace HMS;

class Program
{
    static void Main(string[] args)
    {
        Utility utility = new Utility();
        Authentication auth = new Authentication();

        bool active = true;
        
        utility.InitialiseApplication();

        while (active)
        {
            #region NotAuthenticated

            while (!auth.IsAuthenticated)
            {
                Console.Clear();
            
                // Welcome Message
                Console.WriteLine("Welcome to the HMS application! Please enter your credentials.\n");
            
                // Email
                Console.Write("Email: ");
                string? email = Console.ReadLine();
            
                // Password
                Console.Write("Password: ");
                string? password = "";
                // Found information about this on https://stackoverflow.com/questions/23433980/c-sharp-console-hide-the-input-from-console-window-while-typing
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
            
                    password += key.KeyChar.ToString();
                }
            
                if (email == "" || password == "")
                {
                    Console.Clear();
                    Console.Write("\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid credentials entered. Please try again.\n");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to return to login again.");
                    Console.Write("\n");
                    Console.ReadKey();
                }
                else
                {
                    bool usr = auth.SignInUser(email, password);
            
                    if (usr)
                    {
                        break;
                    }
                }
            }

            #endregion

            #region IsAuthenticated

            while (auth.IsAuthenticated)
            {
                Console.Clear();
                
                // Welcome Screen
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                Console.WriteLine($"Welcome {auth.Name} ({auth.Id}).");
                Console.WriteLine($"Current email: {auth.Email}.");
                Console.Write('\n');
                Console.Write('\n');

                // Options

                Console.WriteLine("1. Administration");
                Console.WriteLine("2. System Check");
                Console.WriteLine("0. Sign Out");
                
                Console.Write("\n");
                
                // Selections
                Console.Write("Please select an option [e.g. 1]: ");
                string userInputOption = Console.ReadLine();

                switch (userInputOption)
                {
                    case "0":
                        auth.SignOutUser();
                        break;
                    case "1":
                        auth.CreateStaffUser("Test User", "test@test.com", "00000000", ["global.nurse"]);
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Clear();
                        Console.WriteLine("Invalid Option selected. Press any key to continue.");
                        Console.ReadKey();
                        break;
                }
            }

            #endregion
        }
    }
}