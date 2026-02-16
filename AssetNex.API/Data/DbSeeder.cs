//using Microsoft.AspNetCore.Identity;

//public static class DbSeeder
//{
//    public static async Task SeedUsersAsync(IServiceProvider services)
//    {
//        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
//        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

//        // ROLES
//        string[] roles = { "Admin", "User" };

//        foreach (var role in roles)
//        {
//            if (!await roleManager.RoleExistsAsync(role))
//            {
//                await roleManager.CreateAsync(new IdentityRole(role));
//            }
//        }

//        // ADMIN
//        var adminEmail = "admin@test.com";
//        if (await userManager.FindByEmailAsync(adminEmail) == null)
//        {
//            var admin = new IdentityUser
//            {
//                UserName = adminEmail,
//                Email = adminEmail,
//                EmailConfirmed = true
//            };

//            await userManager.CreateAsync(admin, "Admin@123");
//            await userManager.AddToRoleAsync(admin, "Admin");
//        }

//        // USER
//        var userEmail = "user@test.com";
//        if (await userManager.FindByEmailAsync(userEmail) == null)
//        {
//            var user = new IdentityUser
//            {
//                UserName = userEmail,
//                Email = userEmail,
//                EmailConfirmed = true
//            };

//            await userManager.CreateAsync(user, "User@123");
//            await userManager.AddToRoleAsync(user, "User");
//        }
//    }
//}
