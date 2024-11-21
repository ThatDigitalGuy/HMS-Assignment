using System.Globalization;
using Newtonsoft.Json;
using HMS.classes;

namespace HMS.classes
{
    internal class Person
    {
        public string Id { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public Dictionary<string, string> Address { get; set; } = [];

        // Creates the patient class that inherits the properties from the Persons class
    }
    // Patient Class
    class Patient : Person
    {
        private Utility _utils = new();
        
        private List<Dictionary<string, string>> PatientNotes { get; set; } = new();
        private List<Dictionary<string, string>> PatientAppointments { get; set; } = new();
        private List<Dictionary<string, string>> PatientMedical { get; set; } = new();
        private List<Dictionary<string, string>> PatientPrescriptions { get; set; } = new();

        // Creates a new patient record file.
        public void CreateNewPatient(string title, string name, string email, string phone, Dictionary<string, string> address, string userCreator)
        {
            
            _utils.WriteToLogFile($"(Patient | CREATE) {userCreator} has started to create a user.");
            List<Patient> patientObj = new();

            var rand = new Random();

            bool patientFileCheck = true;

            string nextGenId = rand.Next(1000, 100000).ToString();
            string patientRecordFile = $"./Patients/{nextGenId}-record.json";

            while (patientFileCheck)
            {

                if (File.Exists(patientRecordFile))
                {
                    nextGenId = rand.Next(1000, 100000).ToString();
                } else
                {
                    patientFileCheck = false;
                    _utils.WriteToLogFile($"(Patient | CREATE) The new user ID is {nextGenId} and file stored in {patientRecordFile}.");
                    break;
                }
            }

            

            patientObj.Add(new Patient
            {
                Id = nextGenId,
                Title = title,
                Name = name,
                Email = email,
                Phone = phone,
                Address = address,
                PatientNotes = [],
                PatientAppointments = [],
                PatientMedical = [],
                PatientPrescriptions = []
            });

            if (!File.Exists(patientRecordFile))
            {
                File.Create(patientRecordFile).Close();
            }

            var jsonObject = JsonConvert.SerializeObject(patientObj, Formatting.Indented);

            try
            {
                File.WriteAllText(patientRecordFile, jsonObject);

                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Clear();
                Console.WriteLine("Patient has been Created.\n\nPatient Details:\n");
                Console.WriteLine($"Patient ID: {nextGenId}");
                Console.WriteLine($"Patient Title: {title}");
                Console.WriteLine($"Patient Name: {name}");
                Console.WriteLine($"Patient Email: {email}");
                Console.WriteLine($"Patient Phone: {phone}");
                Console.WriteLine($"\n");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                _utils.WriteToLogFile($"(AUTH) Error has occurred: {ex.ToString()}.");
            }
        }
    }
    
    // Doctor Class
    class Doctor : Person
    {
        public required Dictionary<string, string> Credentials { get; set; }
        public required string[] Patients { get; set; }

        public void CreateNewDoctor(string title, string name, string email, string phone,
            Dictionary<string, string> address, string userCreator)
        {
            
        }
    }
    
    // Nurse
    class Nurse : Person
    {
        public required Dictionary<string, string> Credentials { get; set; }
    }
    
    // Admin Staff
    class Admin : Person
    {
        
    }
    
}