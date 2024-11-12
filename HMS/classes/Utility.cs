using System.IO;

namespace HMS.classes;

public class Utility
{
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

        #region PatientFolderExist

        // Checks to see if the patient folder exists.
        if (!Directory.Exists("./Patients"))
        {
            Console.WriteLine("[APP | Start-up] INFO > '/patients' folder doesn't exist. Creating...");
            
            // Create a folder for the patient records.
            Directory.CreateDirectory("./Patients");
        }
        else
        {
            Console.WriteLine("[APP | Start-up] INFO > '/patients' folder exists.");
        }
        #endregion

        #region AuthFileExists

        if (!File.Exists("./auth.json"))
        {
            Console.WriteLine("[APP | Start-up] INFO > '/auth.json' folder doesn't exist. Creating...");
            
            // Create the auth.json file
            File.Create("./auth.json");
        }
        else
        {
            Console.WriteLine("[APP | Start-up] INFO > '/auth.json' folder exists.");

        }

        #endregion
    }

    #endregion
}