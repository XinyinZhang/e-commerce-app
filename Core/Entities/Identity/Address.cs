using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Identity
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        [Required]public string AppUserId{ get; set; } // a navigation property, tell entity
        // framework the relationship between the AppUser and the Address
        //(which appUser corresponding to which Address)
        // each AppUser will only have one address
        public AppUser AppUser { get; set; }
    }
}