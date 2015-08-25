namespace SomethingRest.Runner
{
    public class Product
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public Customer Customer { get; set; }
    }

    public class Customer
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
