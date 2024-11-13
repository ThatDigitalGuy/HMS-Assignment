using Newtonsoft.Json;

namespace HMS.classes;

public class Authentication
{
    Utility _utils = new Utility();
    
    internal bool IsAuthenticated = false;
    // The data structure for authentication class
    #region AuthenticationProps

    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Phone { get; set; }
    public required string[] Groups { get; set; }

    #endregion

    // Validates the user permissions for the task they are trying to
    public bool ValidateUserPermissions(string userId, string[] activityPermissions)
    {
        return false;
    }

    public Authentication SignInUser(string email, string password)
    {
        _utils.WriteToLogFile($"[APP | AUTH] Trying to authenticate user with the email '{email}'.");
        
        /*
         * 1. Read the auth.json file.
         * 2. Check the credentials sent.
         * 3. If valid, return user object.
         * 4. If invalid return null.
         */

        #region GetDataFromAuth

        string jsonString = File.ReadAllText(_utils.AuthFilePath);
        
        List<Authentication> persons = JsonConvert.DeserializeObject<List<Authentication>>(jsonString);

        foreach (Authentication person in persons)
        {
            if (person.Email == email && person.Password == password)
            {
                IsAuthenticated = true;

                Authentication userObj = new Authentication
                {
                    Id = person.Id,
                    Name = person.Name,
                    Email = person.Email,
                    Password = person.Password,
                    Phone = person.Phone,
                    Groups = person.Groups
                };

                return userObj;
            }
        }
        
        
        #endregion
        
        return null;
    }
}