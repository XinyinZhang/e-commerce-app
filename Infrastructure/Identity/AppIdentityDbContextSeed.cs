using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;

namespace Infrastructure.Identity
{
    // a seed class to seed the user and userAddress inside the database   
    // Note： this will be called in program.cs, to make sure the data is seeded into the identity
    // database once the program starts
    public class AppIdentityDbContextSeed
    {
        // instead of using DbContext directly to interact with our identity database, 
        //we will make use of a user manager and a signin manager
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
          // check if any users already created
          // we only want to seed the user if no user is existed in UserManager
          if (!userManager.Users.Any()){
              // create a new AppUser(inherit from identityUser)
              var user = new AppUser
              {
                  DisplayName = "Bob",
                  Email = "bob@test.com",
                  UserName = "bob@test.com",
                  Address = new Address{
                      FirstName = "Bob",
                      LastName = "Bobbity",
                      Street = "10 The Street",
                      City = "New York",
                      State = "NY",
                      Zipcode = "90210"
                  }

              };
              // use userManager to store the new created user and its password(in hashed form)
              // into database table
              await userManager.CreateAsync(user, "Pass$$0rd");

          }
        }
    }
}