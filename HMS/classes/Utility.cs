using Newtonsoft.Json;

namespace HMS.classes;

public class Utility
{
    #region UtitlityVariables

    private string _authFilePath = "./auth.json";
    private string _patientDirectory = "./Patients";
    private string _logDirectory = "./Logs";
    // Learnt DateTime from https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
    private string _logFilePath = $"./Logs/log_{DateTime.Now.ToString("ddMMyyyy")}.txt";

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

        if (!File.Exists(_logFilePath))
        {
            File.Create(_logFilePath).Close();
       
            // File.AppendText()
            // _logWriter = new StreamWriter(_logFilePath);
            //
            // _logWriter.Write($"[LOG > APP | START-UP] Created '{_logDirectory}' directory and '{_logFilePath}' file.");
            WriteToLogFile($"[LOG > APP | START-UP] Created '{_logDirectory}' directory and '{_logFilePath}' file.");
        }
        

        #endregion
        
        #region PatientFolderExist

        // Checks to see if the patient folder exists.
        if (!Directory.Exists(_patientDirectory))
        {
            WriteToLogFile($"[APP | Start-up] INFO > '{_patientDirectory}' folder doesn't exist. Creating...");
            
            // Create a folder for the patient records.
            Directory.CreateDirectory(_patientDirectory);
        }
        else
        {
            Console.WriteLine($"[APP | Start-up] INFO > '{_patientDirectory}' folder exists.");
        }
        #endregion

        #region AuthFileExists

        if (!File.Exists(_authFilePath))
        {
            WriteToLogFile($"[APP | Start-up] INFO > '{_authFilePath}' folder doesn't exist. Creating...");
            
            // Create the auth.json file
            File.Create(_authFilePath).Close();
        }
        else
        {
            Console.WriteLine($"[APP | Start-up] INFO > '{_authFilePath}' folder exists.");

        }

        #endregion

        #region CheckAuthFile

        if (File.ReadAllText(_authFilePath) == "")
        {
            WriteToLogFile($"[APP | Start-up] INFO > '{_authFilePath}' is empty. Creating new admin user...");
            List<Authentication> userObj = new List<Authentication>();
            
            // Creating a default admin user
            userObj.Add(new Authentication
            {
                Id = "001", Name = "Administrator", Email = "admin@admin.com", Password = "admin", Phone = "000000000",
                Groups = ["global.administrator"]
            });
            
            // Serialise to json
            string jsonString = JsonConvert.SerializeObject(userObj, Formatting.Indented);
            File.WriteAllText(_authFilePath, jsonString);
        }
        else
        {
            WriteToLogFile($"[APP | Start-up] INFO > '{_authFilePath}' has content.");
        }

        #endregion
    }

    #endregion

    // Global Write to the Log File.
    public void WriteToLogFile(string data)
    {
        try
        {
            File.AppendAllText(_logFilePath, data + '\n');
        }
        catch (Exception e)
        {
            Console.WriteLine("Log File Mission or Gone.");
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath).Close();
       
                // File.AppendText()
                // _logWriter = new StreamWriter(_logFilePath);
                //
                // _logWriter.Write($"[LOG > APP | START-UP] Created '{_logDirectory}' directory and '{_logFilePath}' file.");
                WriteToLogFile($"[LOG > APP | START-UP] Created '{_logDirectory}' directory and '{_logFilePath}' file.");
            }
            WriteToLogFile(e.ToString());
            throw;
        }
    }
    
    
    // Pulls the authentication users from auth file.
    public void ReadFromAuthFile()
    {
        string jsonString = File.ReadAllText(_authFilePath);
        
        Console.WriteLine(jsonString);
    }
}