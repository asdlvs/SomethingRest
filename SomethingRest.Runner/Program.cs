using System;
using SomethingRest.Core;

namespace SomethingRest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var instance = new BaseImplementation().Implement<ITestInterface>();
            instance.PostProduct(new Product
            {
                Customer = new Customer
                {
                    FirstName = "Vitaliy",
                    LastName = "Lebedev"
                },
                Name = "IPad",
                Id = 15
            });

        }
    }
}
