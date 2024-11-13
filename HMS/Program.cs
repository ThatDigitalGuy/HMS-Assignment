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
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                Console.WriteLine(key.KeyChar);
            }
            
            
        }
    }
}