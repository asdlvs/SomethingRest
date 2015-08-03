using SomethingRest.Core;

namespace SomethingRest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var instance = new BaseImplementation().Implement<ITestInterface>();
            var res = instance.Test1("Hello", " world");
            var res2 = instance.Test2(new { A = 1, B = 2 }, "Hlelo wrodl");
        }
    }
}
