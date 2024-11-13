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

    public List<Authentication>? SignInUser(string email, string password)
    {
        _utils.WriteToLogFile($"[APP | AUTH] Trying to authenticate user with the email '{email}'.");
        
        
        
        return null;
    }
}