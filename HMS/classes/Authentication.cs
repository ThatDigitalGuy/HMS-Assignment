using Newtonsoft.Json;

namespace HMS.classes;

public class Authentication
{
    Utility _utils = new Utility();
    
    // The data structure for authentication class
    #region AuthenticationProps

    public bool IsAuthenticated { get; private set; }
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Group { get; set; } = String.Empty;

    #endregion

    // TODO: Validates the user permissions for the task they are trying to
    public bool ValidateUserPermissions(string userId, string activity ,string[] activityPermissions, string userPermission)
    {
        _utils.WriteToLogFile($"{userId} is trying to do '{activity}'.");

        // Gets the file and reads all text
        var jsonString = File.ReadAllText(_utils.roleFilePath);
        
        // Deserialises the auth.json file to a list.
        List<Roles>? roleList = JsonConvert.DeserializeObject<List<Roles>>(jsonString);

        foreach (Roles roles in roleList)
        {
            if (roles.PermissionName == userPermission)
            {
                foreach (var permission in roles.Permissions)
                {
                    foreach (var activityperm in activityPermissions)
                    {
                        if (permission.ToLower() == activityperm.ToLower())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
        
        return false;
    }
    
    public bool SignInUser(string? email, string password)
    {
        // Logs the attempt to sign in
        _utils.WriteToLogFile($"(AUTH) Trying to authenticate user with the email '{email}'.");
        
        // Gets the file and reads all text
        var jsonString = File.ReadAllText(_utils.AuthFilePath);
        
        // Deserialises the auth.json file to a list.
        List<Authentication>? authList = JsonConvert.DeserializeObject<List<Authentication>>(jsonString);

        // Checks if any record is valid
        if (authList != null)
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
                    Group = user.Group;

                    _utils.WriteToLogFile($"(AUTH) {Name} ({Id}) has successfully logged in.");

                    return true;
                }
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
            Group = String.Empty;
        }
        else
        {
        }
    }
    
    // Create a new staff member
    public void CreateStaffUser(string name, string email, string phone, string group)
    {
        Console.Clear();
        _utils.WriteToLogFile($"(AUTH) The user {Name} ({Id}) has submitted a user ({email}) for the system to create.");
        var jsonString = File.ReadAllText(_utils.AuthFilePath);
        List<Authentication>? userObj = JsonConvert.DeserializeObject<List<Authentication>>(jsonString);

        var rand = new Random();

        string genId = rand.Next(1000, 10000).ToString();
        string genPwd = _utils.GenerateRandomPassword();

        // Creating a default admin user
        userObj?.Add(new Authentication
        {
            Id = genId, Name = name, Email = email, Password = genPwd, Phone = phone,
            Group = group
        });
            
        // Serialise to json
        jsonString = JsonConvert.SerializeObject(userObj, Formatting.Indented);
        File.WriteAllText(_utils.AuthFilePath, jsonString);
    }
}