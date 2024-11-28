using System.Globalization;
using Newtonsoft.Json;
using HMS.classes;

namespace HMS.classes
{
    /// <summary>
    /// This is the class that gives you a structure on a person in the system.
    /// </summary>
    class Person
    {
        public string ID { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public Dictionary<string, string> Address { get; set; } = [];
        public string CreatedAt { get; set; } = String.Empty;
        public string CreatedBy { get; set; } = String.Empty;
        public string LastModified { get; set; } = String.Empty;
        public string ModifiedBy { get; set; } = String.Empty;

        // Manage Patient Menu
        public void UserManagement(Authentication authUser)
        {
            bool menuActive = true;
            string userGroup = "Patient";
            
            while (menuActive)
            {
                Console.Clear();
                if (userGroup == "Patient") { Console.BackgroundColor = ConsoleColor.DarkMagenta; }
                if (userGroup == "Staff") { Console.BackgroundColor = ConsoleColor.DarkBlue; }
                Console.ForegroundColor = ConsoleColor.White;
            
                Console.Write($"   {userGroup}   ");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("         User Management | V 0.1         \n");
                
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Gray;
                
                Console.Write("\n");
                
                if (userGroup == "Patient") {Console.WriteLine("[q] to exit, [c] to create user, [s] to search, [r] to see staff, [return] to select");}
                if (userGroup == "Staff") { Console.WriteLine("[q] to exit, [c] to create user, [s] to search, [r] to see patients, [return] to select"); }
                
                Console.Write("\n");

                #region ListUsers

                if (userGroup == "Patient")
                {
                    string[] userDir = Directory.GetFiles("./Patients", "*-record.json");

                    foreach (var user in userDir)
                    {
                        var fileUser = File.ReadAllText(user);
                        List<Patient> person = JsonConvert.DeserializeObject<List<Patient>>(fileUser);
                        
                        Console.WriteLine($" {person[0].ID}  |    {person[0].FirstName} {person[0].LastName}    |   {person[0].Email}  |    {person[0].Phone}");
                    }
                }
                
                if (userGroup == "Staff")
                {
                    Console.WriteLine("Staff");
                }

                #endregion
                
                
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.KeyChar)
                {
                    case 'q':
                        menuActive = false;
                        break;
                    case 'r':
                        if (userGroup == "Patient")
                        {
                            userGroup = "Staff";
                            break;
                        }

                        if (userGroup == "Staff")
                        {
                            userGroup = "Patient";
                            break;
                        }

                        break;
                    case 'c':
                        if (userGroup == "Staff")
                        {
                            Console.WriteLine("Create Staff");
                        }

                        if (userGroup == "Patient")
                        {
                            Utility _utility = new Utility();
                            
                            Console.Clear();
                            
                            _utility.CreatePatient(authUser.Id, authUser.Name);
                        }
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input. Please try again. Press any key to return.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }
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
        public void Create(string title, string firstname,string lastname, string email, string phone, Dictionary<string, string> address, string creatorId, string createrName)
        {
            _utils.WriteToLogFile($"[PERSON (patient) | Create] {email} is being created.");
            
            if (String.IsNullOrEmpty(email)) return;

            
            // Creates a new patients.
            List<Patient> patientObj = new();

            #region GenerateID
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
                ID = nextGenId,
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
                File.WriteAllText(patientRecordFile, jsonObject);

                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Clear();
                Console.WriteLine("Patient has been Created.\n\nPatient Details:\n");
                Console.WriteLine($"Patient ID: {nextGenId}");
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
        public void Delete(string id)
        {
            Console.Clear();
            
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            
            Console.WriteLine(@"$$\      $$\                               $$\                     
$$ | $\  $$ |                              \__|                    
$$ |$$$\ $$ | $$$$$$\   $$$$$$\  $$$$$$$\  $$\ $$$$$$$\   $$$$$$\  
$$ $$ $$\$$ | \____$$\ $$  __$$\ $$  __$$\ $$ |$$  __$$\ $$  __$$\ 
$$$$  _$$$$ | $$$$$$$ |$$ |  \__|$$ |  $$ |$$ |$$ |  $$ |$$ /  $$ |
$$$  / \$$$ |$$  __$$ |$$ |      $$ |  $$ |$$ |$$ |  $$ |$$ |  $$ |
$$  /   \$$ |\$$$$$$$ |$$ |      $$ |  $$ |$$ |$$ |  $$ |\$$$$$$$ |
\__/     \__| \_______|\__|      \__|  \__|\__|\__|  \__| \____$$ |
                                                         $$\   $$ |
                                                         \$$$$$$  |
                                                          \______/");
            Console.WriteLine("");
            Console.WriteLine("Are you sure you want to delete the following user?");
        }
    }
}