using System;
using SomethingRest.Core;

namespace SomethingRest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var instance = new BaseImplementation().Implement<ITestInterface>();
            var res = instance.Test1(5);
        }
    }
}
