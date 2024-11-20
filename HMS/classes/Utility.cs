using Newtonsoft.Json;

namespace HMS.classes;

public class Utility
{
    #region UtitlityVariables

    public string LogPrefix = $"[{DateTime.UtcNow.ToString()}]";
    public string AuthFilePath = "./auth.json";
    public string patientDirectory = "./Patients";
    private string _logDirectory = "./Logs";
    // Learnt DateTime from https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
    internal string LogFilePath = $"./Logs/log_{DateTime.Now.ToString("ddMMyyyy")}.txt";
    public string roleFilePath = "./roles.json";

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
            WriteToLogFile($"{LogPrefix} Created '{_logDirectory}' directory and '{LogFilePath}' file.");
        }
        

        #endregion
        
        #region PatientFolderExist

        // Checks to see if the patient folder exists.
        if (!Directory.Exists(patientDirectory))
        {
            WriteToLogFile($"[APP | Start-up] INFO > '{patientDirectory}' folder doesn't exist. Creating...");
            
            // Create a folder for the patient records.
            Directory.CreateDirectory(patientDirectory);
        }
        else
        {
            Console.WriteLine($"[APP | Start-up] INFO > '{patientDirectory}' folder exists.");
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
            List<Authentication> userObj = new List<Authentication>();
            
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

        if (File.Exists(roleFilePath))
        {
            return;
        }
        else
        {
            try
            {
                File.Create(roleFilePath).Close();
                
                var jsonString = File.ReadAllText(roleFilePath);
                List<Roles> userObj = new List<Roles>();
                
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
                    Name = "Admin",
                    PermissionName = "global.admin",
                    Permissions = []
                });
                
                jsonString = JsonConvert.SerializeObject(userObj, Formatting.Indented);
                File.WriteAllText(roleFilePath, jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion
    }

    #endregion

    // Global Write to the Log File.
    public void WriteToLogFile(string data)
    {
        try
        {
            File.AppendAllText(LogFilePath, $"{LogPrefix}{data}\n");
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
    public void ReadFromAuthFile()
    {
        string jsonString = File.ReadAllText(AuthFilePath);
        
        Console.WriteLine(jsonString);
    }
    
    // Generate random password
    public string GenerateRandomPassword()
    {
        var rand = new Random();

        return rand.Next(10000, 100000).ToString();
    }
    
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
        string patientTCAddress = Console.ReadLine()!;

        if (string.IsNullOrEmpty(patientTCAddress))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Invalid input entered. Press any key to continue.");
            Console.ReadKey();
            return;
        } else
        {
            addressArr.Add("Town", patientTCAddress);
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
        patient.CreateNewPatient(patientTitle, patientName, patientEmail, patientPhone, addressArr, userCreator);
    }
}