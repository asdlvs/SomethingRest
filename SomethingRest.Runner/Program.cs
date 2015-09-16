using SomethingRest.Core;
using SomethingRest.Core.InterfaceImplementations;

namespace SomethingRest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var instance = new RestImplementation().Implement<ITestInterface>();
            instance.MyProperty = "1";
            var res = instance.Test1("Hello", " world");
            var res2 = instance.Test2(new { A = 1, B = 2 }, "Hlelo wrodl");

            var test = new Test();
            test.TestStringProperty = "Hello world";
        }
    }

    class Test
    {
        public string TestStringProperty { get; set; }
    }
}
