using Newtonsoft.Json;
using DateTime = System.DateTime;

namespace HMS.classes;

public class Authentication
{
    Utility _utils = new Utility();
    
    // The data structure for authentication class
    #region AuthenticationProps

    public bool IsAuthenticated { get; set; } = false;
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string[] Groups { get; set; } = Array.Empty<string>();

    #endregion

    // Validates the user permissions for the task they are trying to
    public bool ValidateUserPermissions(string userId, string[] activityPermissions)
    {
        return false;
    }

    public bool SignInUser(string email, string password)
    {
        // Logs the attempt to sign in
        _utils.WriteToLogFile($"(AUTH) Trying to authenticate user with the email '{email}'.");
        
        // Gets the file and reads all text
        var jsonString = File.ReadAllText(_utils.AuthFilePath);
        
        // Deserialises the auth.json file to a list.
        List<Authentication> authList = JsonConvert.DeserializeObject<List<Authentication>>(jsonString);

        // Checks if any record is valid
        foreach (var user in authList)
        {
            // If there is a user, it sets the Current User details.
            if (user.Email == email && user.Password == password)
            {
                IsAuthenticated = true;
                Id = user.Id;
                Name = user.Name;
                Email = user.Email;
                Phone = user.Phone;
                Groups = user.Groups;
                
                _utils.WriteToLogFile($"(AUTH) {Name} ({Id}) has successfully logged in.");
                
                return true;
                
                break;
            }

            return false;
        }

        return false;
    }

    // Signs the user out :D
    public void SignOutUser()
    {
        if (IsAuthenticated)
        {
            _utils.WriteToLogFile($"(AUTH) {Name} ({Id}) has successfully logged out.");
            
            IsAuthenticated = false;
            Id = String.Empty;
            Name = String.Empty;
            Email = String.Empty;
            Phone = String.Empty;
            Groups = Array.Empty<string>();
        }
        else
        {
        }
    }
    
    
}