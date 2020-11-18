namespace Core.Entities.OrderAggregate
{ // Question: we already have an Address class(inside Core/Entities/Identity)
// Why do we need another one?
// A: Core/Entities/Identity/Address is tightly bound to our identity
// we want to have another Address in a separate context boundary
// (physically separate from the application)
    public class Address 
    // this address will be owned by the Order
    {
        public Address()
        {
        }

        public Address(string firstName, string lastName, string street, string city, string state, string zipcode)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            State = state;
            Zipcode = zipcode;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}