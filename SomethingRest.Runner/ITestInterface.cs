namespace SomethingRest.Runner
{
    public interface ITestInterface
    {
        object Test1(string url1, string url2);

        object Test2(object o1, string s1);
        string MyProperty { get; set; }
    }
}
