namespace SomethingRest.Runner
{
    public class Product
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public Customer Customer { get; set; }


        public override bool Equals(object obj)
        {
            var objP = obj as Product;
            if (objP == null)
            {
                return false;
            }

            return objP.Name == this.Name && objP.Id == this.Id && objP.Customer.Equals(this.Customer);
        }
    }

    public class Customer
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override bool Equals(object obj)
        {
            var objC = obj as Customer;
            if (objC == null)
            {
                return false;
            }

            return objC.FirstName == this.FirstName && objC.LastName == this.LastName;
        }
    }
}
