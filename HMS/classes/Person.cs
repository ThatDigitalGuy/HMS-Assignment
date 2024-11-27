using System.Globalization;
using Newtonsoft.Json;
using HMS.classes;

namespace HMS.classes
{
    /// <summary>
    /// This is the class that gives you a structure on a person in the system.
    /// </summary>
    internal class Person
    {
        public string ID { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public string[] Address { get; set; } = [];
        public string CreatedAt { get; set; } = String.Empty;
        public string CreatedBy { get; set; } = String.Empty;
        public string LastModified { get; set; } = String.Empty;
        public string ModifiedBy { get; set; } = String.Empty;

        // Creates the patient class that inherits the properties from the Persons class
    }

    /// <summary>
    ///  This is the medical history.
    /// </summary>
    class Medical
    {
        public Dictionary<string, string> notes { get; set; }
        public Dictionary<string, string> appointments { get; set; }
        public Dictionary<string, string> history { get; set; }
        public Dictionary<string, string> prescription { get; set; }
    }

    class AccountCredentials
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool active { get; set; } = false;
        public string group { get; set; }
    }
    
    class UserCredentials
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool Valid { get; set; }
    }
    
    // Patient Class
    /// <summary>
    /// This gives the structure for the Patient record.
    /// </summary>
    class Patient : Person
    {
        private readonly Utility _utils = new();

        #region ClassVarables

        public Medical Medical { get; set; }

        #endregion
        
        /// <summary>
        /// Creates a new patient record in the system.
        /// </summary>
        /// <param name="title">string</param>
        /// <param name="name">string</param>
        /// <param name="email">string</param>
        /// <param name="phone">string</param>
        /// <param name="address">dictionary (string, string)</param>
        /// <param name="userCreator">string</param>
        public void Create(string title, string firstname,string lastname, string email, string phone, string[] address, string creatorId, string createrName)
        {
            #region badCodeL
            /*
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
            */
            #endregion
            
            _utils.WriteToLogFile($"[PERSON (patient) | Create] {email} is being created.");
            
            if (String.IsNullOrEmpty(email)) return;

            
            // Creates a new patients.
            string id = null;
            List<Patient> patientObj = new();

            #region GenerateID

            bool active = true;

            while (active)
            {
                id = _utils.GenerateRandomPassword();

                if (File.Exists($"/Patients/{id}-record.json"))
                {
                    return;
                }
                else
                {
                    id = ID;
                    
                    File.Create($"/Patients/{ID}-record.json").Close();
                    
                    _utils.WriteToLogFile($"[PERSON (patient) | Create] '/Patients/{ID}-record.json' has just been created.");
                    
                    active = false;
                    return;
                }
            }

            #endregion

            #region Checks

            if (String.IsNullOrEmpty(title)) return;
            if (String.IsNullOrEmpty(firstname)) return;
            if (String.IsNullOrEmpty(lastname)) return;
            if (String.IsNullOrEmpty(phone)) return;

            #endregion

            #region createUserObject

            Medical medicalObj = new()
            {
                appointments = [],
                history = [],
                notes = [],
                prescription = []
            };

            string currentDate = new DateTime().ToString();
            
            patientObj.Add(new Patient()
            {
                ID = id,
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                Phone = phone,
                Address = address,
                Medical = medicalObj,
                CreatedAt = currentDate,
                CreatedBy = creatorId,
                LastModified = currentDate,
                ModifiedBy = creatorId
            });
            #endregion

            #region SubmitToFile

            var jsonObject = JsonConvert.SerializeObject(patientObj, Formatting.Indented);

            try
            {
                File.WriteAllText($"/Patients/{ID}-record.json", jsonObject);

                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Clear();
                Console.WriteLine("Patient has been Created.\n\nPatient Details:\n");
                Console.WriteLine($"Patient ID: {ID}");
                Console.WriteLine($"Patient Title: {title}");
                Console.WriteLine($"Patient Name: {firstname} {lastname}");
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

            #endregion
        }
        
        /// <summary>
        /// Modifies a patent record.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="updateProperty">You can find the update properties at https://github.com/ThatDigitalGuy/HMS-Assignment/wiki/Patient-Class#modify</param>
        /// <param name="updateContent">The content that you wish to update.</param>
        public void Modify(string id, string updateProperty, string updateContent)
        {
            
        }
        
        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The Patient ID you wish to delete.</param>
        public void Delete(string id) {}
    }
}