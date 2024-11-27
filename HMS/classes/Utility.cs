using Newtonsoft.Json;

namespace HMS.classes;

public class Utility
{
    #region UtitlityVariables

    private readonly string _logPrefix = $"[{DateTime.UtcNow.ToString()}]";
    public const string AuthFilePath = "./auth.json";
    private const string PatientDirectory = "./Patients";

    private string _logDirectory = "./Logs";
    // Learnt DateTime from https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
    internal readonly string LogFilePath = $"./Logs/log_{DateTime.Now.ToString("ddMMyyyy")}.txt";
    public readonly string RoleFilePath = "./roles.json";
    
    
    #endregion
    
    #region InitialiseApplication

    /*
     This is run at the start of the application. This creates the default environment.
     This method does the following:
        - Creates the patient record folder
        - Creates the authentication file
            - Default credentials: admin | AdminP@ss (Application Administrator)
     */
    public void InitialiseApplication()
    {
        // I used W3Schools and the Microsoft Documentation to assist me with files in c#.
        Assistance assitance = new Assistance();
            
        #region LogFile

        if (!Directory.Exists(_logDirectory))
        {
            Directory.CreateDirectory(_logDirectory);
        }

        if (!File.Exists(LogFilePath))
        {
            File.Create(LogFilePath).Close();
       
            // File.AppendText()
            // _logWriter = new StreamWriter(LogFilePath);
            //
            // _logWriter.Write($"[LOG > APP | START-UP] Created '{_logDirectory}' directory and '{LogFilePath}' file.");
            WriteToLogFile($"{_logPrefix} Created '{_logDirectory}' directory and '{LogFilePath}' file.");
        }
        

        #endregion
        
        #region PatientFolderExist

        // Checks to see if the patient folder exists.
        if (!Directory.Exists(PatientDirectory))
        {
            WriteToLogFile($"[APP | Start-up] INFO > '{PatientDirectory}' folder doesn't exist. Creating...");
            
            // Create a folder for the patient records.
            Directory.CreateDirectory(PatientDirectory);
        }
        else
        {
            Console.WriteLine($"[APP | Start-up] INFO > '{PatientDirectory}' folder exists.");
        }
        #endregion

        #region AuthFileExists

        if (!File.Exists(AuthFilePath))
        {
            WriteToLogFile($"[APP | Start-up] INFO > '{AuthFilePath}' folder doesn't exist. Creating...");
            
            // Create the auth.json file
            File.Create(AuthFilePath).Close();
        }
        else
        {
            Console.WriteLine($"[APP | Start-up] INFO > '{AuthFilePath}' folder exists.");

        }

        #endregion

        #region CheckAuthFile

        if (File.ReadAllText(AuthFilePath) == "")
        {
            WriteToLogFile($"[APP | Start-up] INFO > '{AuthFilePath}' is empty. Creating new admin user...");
            List<Authentication> userObj = new();
            
            // Creating a default admin user
            userObj.Add(new Authentication
            {
                Id = "001", Name = "Administrator", Email = "admin@admin.com", Password = "admin", Phone = "000000000",
                Group = "global.administrator"
            });
            
            // Serialise to json
            string jsonString = JsonConvert.SerializeObject(userObj, Formatting.Indented);
            File.AppendAllText(AuthFilePath, jsonString);
        }
        else
        {
            WriteToLogFile($"[APP | Start-up] INFO > '{AuthFilePath}' has content.");
        }

        #endregion

        #region RolesFile

        if (File.Exists(RoleFilePath))
        {
            return;
        }
        else
        {
            try
            {
                File.Create(RoleFilePath).Close();
                
                var jsonString = File.ReadAllText(RoleFilePath);
                List<Roles> userObj = new();
                
                userObj.Add(new Roles
                {
                    Id = "001",
                    Name = "Global Administrator",
                    PermissionName = "global.administrator",
                    Permissions = [
                        "global.administrator",
                        "global.patient.administrator",
                        "global.user.administrator",
                        "global.medical.administrator",
                        "global.pharmacy.administrator",
                        "global.finance.administrator",
                    ]
                });
                userObj.Add(new Roles
                {
                    Id = "001",
                    Name = "Nurse",
                    PermissionName = "global.nurse",
                    Permissions = []
                });
                userObj.Add(new Roles
                {
                    Id = "002",
                    Name = "Doctor",
                    PermissionName = "global.doctor",
                    Permissions = []
                });
                userObj.Add(new Roles
                {
                    Id = "003",
                    Name = "Admin Staff",
                    PermissionName = "global.admin-staff",
                    Permissions = []
                });
                
                jsonString = JsonConvert.SerializeObject(userObj, Formatting.Indented);
                File.WriteAllText(RoleFilePath, jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region AudioTesting

        assitance.TestModule();

        #endregion

        Console.Title = "HMS.local";
    }

    #endregion

    class UserCreator
    {
        public string userId { get; set; } = String.Empty;
        public string userName { get; set; } = String.Empty;
    }
    
    // Global Write to the Log File.
    public void WriteToLogFile(string data)
    {
        try
        {
            File.AppendAllText(LogFilePath, $"{_logPrefix}{data}\n");
        }
        catch (Exception e)
        {
            Console.WriteLine("Log File Mission or Gone.");
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            if (!File.Exists(LogFilePath))
            {
                File.Create(LogFilePath).Close();
       
                // File.AppendText()
                // _logWriter = new StreamWriter(LogFilePath);
                //
                // _logWriter.Write($"[LOG > APP | START-UP] Created '{_logDirectory}' directory and '{LogFilePath}' file.");
                WriteToLogFile($"[LOG > APP | START-UP] Created '{_logDirectory}' directory and '{LogFilePath}' file.");
            }
            WriteToLogFile(e.ToString());
            throw;
        }
    }
    
    
    // Pulls the authentication users from auth file.
    /*public void ReadFromAuthFile()
    {
        string jsonString = File.ReadAllText(AuthFilePath);
        
        Console.WriteLine(jsonString);
    }*/
    
    // Generate random password
    public string GenerateRandomPassword()
    {
        var rand = new Random();

        return rand.Next(10000, 100000).ToString();
    }
    
    // Creates a Patient record in "./Patients"
    public void CreatePatient(string userCreator)
    {
        Patient patient = new Patient();
        // Patient Title
        Console.WriteLine("Please enter the Title of the patient.");
        string patientTitle = Console.ReadLine()!;

        if (string.IsNullOrEmpty(patientTitle))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Invalid input entered. Press any key to continue.");
            Console.ReadKey();
            return;
        }


        // Patient Name
        Console.WriteLine("Please enter the Name of the patient.");
        string patientName = Console.ReadLine()!;

        if (string.IsNullOrEmpty(patientName))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Invalid input entered. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        // Patient Email
        Console.WriteLine("Please enter the Email of the patient.");
        string patientEmail = Console.ReadLine()!;

        if (string.IsNullOrEmpty(patientTitle))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Invalid input entered. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        // Patient Phone 
        Console.WriteLine("Please enter the Phone of the patient.");
        string patientPhone = Console.ReadLine()!;

        if (string.IsNullOrEmpty(patientTitle))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Invalid input entered. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        Dictionary<string, string> addressArr = [];

        // Patient First Line Address 
        Console.WriteLine("Please enter the First Line Address of the patient.");
        string patientFAddress = Console.ReadLine()!;

        if (string.IsNullOrEmpty(patientFAddress))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Invalid input entered. Press any key to continue.");
            Console.ReadKey();
            return;
        } else
        {
            addressArr.Add("First Line:", patientFAddress);
        }

        // Patient Second Line Address 
        Console.WriteLine("Please enter the Second Line Address of the patient.");
        string patientSAddress = Console.ReadLine()!;

        if (string.IsNullOrEmpty(patientSAddress))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Invalid input entered. Press any key to continue.");
            Console.ReadKey();
            return;
        }
        else
        {
            addressArr.Add("Second Line", patientSAddress);
        }
        // Patient Town/City Line Address 
        Console.WriteLine("Please enter the Town/City of the patient.");
        string patientTcAddress = Console.ReadLine()!;

        if (string.IsNullOrEmpty(patientTcAddress))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Invalid input entered. Press any key to continue.");
            Console.ReadKey();
            return;
        } else
        {
            addressArr.Add("Town", patientTcAddress);
        }

        // Patient County Address 
        Console.WriteLine("Please enter the County of the patient.");
        string patientCounAddress = Console.ReadLine()!;

        if (string.IsNullOrEmpty(patientCounAddress))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Invalid input entered. Press any key to continue.");
            Console.ReadKey();
            return;
        } else
        {
            addressArr.Add("County", patientCounAddress);
        }
        // Patient Postcode Address 
        Console.WriteLine("Please enter the Postcode of the patient.");
        string patientPAddress = Console.ReadLine()!;

        if (string.IsNullOrEmpty(patientPAddress))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Invalid input entered. Press any key to continue.");
            Console.ReadKey();
            return;
        } else
        {
            addressArr.Add("PostCode", patientPAddress);
        }
        // Patient Country Address 
        Console.WriteLine("Please enter the Country of the patient.");
        string patientCAddress = Console.ReadLine()!;

        if (string.IsNullOrEmpty(patientCAddress))        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Invalid input entered. Press any key to continue.");
            Console.ReadKey();
            return;
        } else
        {
            addressArr.Add("Country", patientCAddress);
        }

        Console.WriteLine(addressArr.ToString());
        Console.ReadKey();
        // Create the patient record :D
        //patient.CreateNewPatient(patientTitle, patientName, patientEmail, patientPhone, addressArr, userCreator);
    }

    // Create a new Doctor
    public void CreateDoctor(string userCreator)
    {
        
    }
    
    // Test Code
    public void SystemCheck()
    {
        Utility utils = new Utility();
        Assistance assistance = new Assistance();
        utils.WriteToLogFile("[SysCheck] Running System Check (SysCheck).");

        #region FileStructure

        int count = 0;
        utils.WriteToLogFile("[SysCheck] Checking the system structure.");
        
        // Check if the patient records exists
        if (Directory.Exists(PatientDirectory))
        {
            
        }

        #endregion
        
        
    }
}