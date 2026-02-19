using Microsoft.AspNetCore.Identity;

namespace AviMerch.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}
