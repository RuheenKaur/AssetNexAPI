

using Microsoft.AspNetCore.Identity;
public class HashGen
{
    public static void PrintHash()
        {
            var hasher = new PasswordHasher<IdentityUser>();
            var dummy = new IdentityUser();
            var hash = hasher.HashPassword(dummy, "admin@111");
            Console.WriteLine("Generated Hash: " + hash);
        }
    }
