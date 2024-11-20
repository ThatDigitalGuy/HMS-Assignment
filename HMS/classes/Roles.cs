using Newtonsoft.Json;

namespace HMS.classes;

public class Roles
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PermissionName { get; set; } = string.Empty;
    public string[] Permissions { get; set; } = Array.Empty<string>();
    
    
    /*
     * 
     * Patient Permissions: global.patient.*
     * Administrator Permissions: global.user.*
     * Medical Permissions: global.medical.*
     * Pharmacy Permissions: global.pharmacy.*
     * Finance Permissions: global.finance.*
     * 
     */
    
}