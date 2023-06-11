using Microsoft.AspNetCore.Identity;

namespace Database;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}