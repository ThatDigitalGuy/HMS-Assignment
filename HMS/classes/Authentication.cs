using Newtonsoft.Json;
using System;

namespace HMS.classes;

public class Authentication
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string[] Address { get; set; }
    public List<Dictionary<string,string>> Notes { get; set; }
    public List<Dictionary<string,string>> Appointments { get; set; }
}