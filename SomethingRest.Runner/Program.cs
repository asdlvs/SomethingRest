using System;
using SomethingRest.Core;

namespace SomethingRest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var instance = new BaseImplementation().Implement<ITestInterface>();

            for (var i = 0; i < 10; i++)
            {
                var newP = new Product
                {
                    Customer = new Customer
                    {
                        FirstName = "Vitaliy" + i,
                        LastName = "Lebedev" + i*2
                    },
                    Name = "IPad" + i*3,
                    Id = i
                };
                var id = instance.Post(newP);

                var asyn = instance.Get(id);
                var product = asyn.Result;

                Console.WriteLine(newP.Equals(product));
            }

            Console.ReadKey();
        }
    }
}
