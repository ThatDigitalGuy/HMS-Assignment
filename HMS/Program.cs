using HMS.classes;

namespace HMS;

class Program
{
    static void Main(string[] args)
    {
        Utility utility = new Utility();
        Authentication auth = new Authentication
        {
            Id = null,
            Name = null,
            Email = null,
            Password = null,
            Phone = null,
            Groups = new string[]
            {
            }
        };
        
        utility.InitialiseApplication();
        
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
                auth = auth.SignInUser(email, password);
            }
        }
        
        Console.WriteLine(auth.Name);
    }
}