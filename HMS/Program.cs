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
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
            
                // Welcome Message
                Console.WriteLine("Welcome to the HMS application! Please enter your credentials.\n");
            
                // Email
                Console.Write("Email: ");
                string? email = Console.ReadLine();
            
                // Password
                Console.Write("Password: ");
                var password = "";
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
                    else
                    {
                        
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Clear();
                        Console.WriteLine("Invalid email or password. Press any key to try again. Press '?' if you need assistance.");
                        var key = Console.ReadKey(true);
                        Console.WriteLine(key.KeyChar.ToString());
                        if (key.KeyChar.ToString() == "?") {
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.Clear();
                            Console.WriteLine("Assistance");
                            Console.WriteLine("----------\n");
                            Console.WriteLine("");
                            Console.WriteLine("If you need any assistance, please contact the system administrator.");
                            Console.WriteLine("");
                            Console.WriteLine("Extension: 010");
                            Console.WriteLine("Email: support@hms.local");
                            Console.WriteLine("");
                            Console.WriteLine("Press any key to return to the login screen.");
                            Console.ReadKey();
                        }
                    }
                }
            }

            #endregion

            #region IsAuthenticated

            while (auth.IsAuthenticated)
            {
                Console.Clear();
                
                // Welcome Screen
                Console.Clear();
                Console.WriteLine($"Welcome {auth.Name} ({auth.Id}).");
                Console.WriteLine($"Current email: {auth.Email}.");
                Console.Write('\n');
                Console.Write('\n');
                

                // Options

                Console.WriteLine("1. Administration");
                // Console.WriteLine("2. System Check");
                Console.WriteLine("0. Sign Out");
                
                Console.Write("\n");
                
                // Selections
                Console.Write("Please select an option [e.g. 1]: ");
                string? userInputOptionMain = Console.ReadLine();

                switch (userInputOptionMain)
                {
                    case "0":
                        auth.SignOutUser();
                        break;
                    case "1":

                        #region AdminSection

                        Console.Clear();

                        bool adminSectionActive = true;

                        while (adminSectionActive)
                        {
                            Console.ResetColor();
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            
                            Console.WriteLine($"Welcome {auth.Name} ({auth.Id}).");
                            Console.WriteLine($"Current email: {auth.Email}.");
                            Console.Write('\n');
                            Console.Write('\n');
                            
                            // Admin Menu
                            Console.WriteLine("Administration Section\n\n");
                        
                            Console.WriteLine("1. Create Patient");
                            Console.WriteLine("2. List Patients");
                            Console.WriteLine("3. Create Staff Member");
                            Console.WriteLine("9. Back to main menu");
                            Console.WriteLine("0. Sign Out");
                        
                            Console.Write("\n");
                            Console.WriteLine("Please select an option [e.g. 1]: ");
                            string? userInputAdmin = Console.ReadLine();

                            if (String.IsNullOrEmpty(userInputAdmin))
                            {
                                break;
                            }

                            switch (userInputAdmin)
                            {
                                case "0":
                                    auth.SignOutUser();
                                    adminSectionActive = false;
                                    break;
                                case "1":
                                    Console.Clear();
                                    utility.CreatePatient(auth.Id);
                                    break;
                                
                                case "3":
                                    bool validPermission = auth.ValidateUserPermissions(auth.Id, "Creating a staff member", ["global.administrator", "user.staffmember.create"], auth.Group);

                                    if (validPermission)
                                    {
                                        // Staff Name
                                        Console.WriteLine("Please enter the Name of the staff member.");
                                        string staffName = Console.ReadLine()!;

                                        if (string.IsNullOrEmpty(staffName))
                                        {
                                            Console.BackgroundColor = ConsoleColor.Red;
                                            Console.Clear();
                                            Console.WriteLine("Invalid input entered. Press any key to continue.");
                                            Console.ReadKey();
                                            return;
                                        }
                                        
                                        // Staff Name
                                        Console.WriteLine("Please enter the Email of the staff member.");
                                        string staffEmail = Console.ReadLine()!;

                                        if (string.IsNullOrEmpty(staffEmail))
                                        {
                                            Console.BackgroundColor = ConsoleColor.Red;
                                            Console.Clear();
                                            Console.WriteLine("Invalid input entered. Press any key to continue.");
                                            Console.ReadKey();
                                            return;
                                        }
                                        
                                        // Staff Name
                                        Console.WriteLine("Please enter the Phone of the staff member.");
                                        string staffPhone = Console.ReadLine()!;

                                        if (string.IsNullOrEmpty(staffPhone))
                                        {
                                            Console.BackgroundColor = ConsoleColor.Red;
                                            Console.Clear();
                                            Console.WriteLine("Invalid input entered. Press any key to continue.");
                                            Console.ReadKey();
                                            Console.ResetColor();
                                            Console.BackgroundColor = ConsoleColor.Black;
                                            return;
                                        }
                                        
                                        // Staff Name
                                        Console.WriteLine("Please enter the Role of the staff member.\n");
                                        Console.WriteLine("global.doctor");
                                        Console.WriteLine("global.nurse");
                                        Console.WriteLine("global.admin-staff");
                                        Console.WriteLine("global.administrator");
                                        Console.WriteLine("");
                                        string staffRole = Console.ReadLine()!;

                                        if (string.IsNullOrEmpty(staffRole))
                                        {
                                            Console.BackgroundColor = ConsoleColor.Red;
                                            Console.Clear();
                                            Console.WriteLine("Invalid input entered. Press any key to continue.");
                                            Console.ReadKey();
                                            return;
                                        }
                                        
                                        auth.CreateStaffUser(staffName, staffEmail, staffPhone, staffRole);
                                    }
                                    else
                                    {
                                        utility.WriteToLogFile($"(AUTH) The user {auth.Name} ({auth.Id}) is unauthorised to submit a user for the system to create.");
                                        Console.BackgroundColor = ConsoleColor.Red;
                                        Console.Clear();
                                        Console.WriteLine("[401] Unauthorised. If there is a problem contact the system administrator.\n\nPress any key to continue.");
                                        Console.ReadKey();
                                        Console.ResetColor();
                                        Console.BackgroundColor = ConsoleColor.Black;
                                    }
                                    
                                    break;
                                case "9":
                                    adminSectionActive = false;
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
                        
                        // auth.CreateStaffUser("Test User", "test@test.com", "00000000", ["global.nurse"]);
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