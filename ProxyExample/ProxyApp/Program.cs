using System;
using Prise.Proxy;
using ProxyLib;

namespace ProxyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // The Calculator object does not inherit from ICalculator, 
            //  but we could use as if it were an ICalculator via the ProxyCreator.CreateProxy or ProxyCreator.CreateGenericProxy methods from Prise.Proxy.ProxyCreator!
            var foreignCalculator = new Calculator();

            // Create a proxy via the Generic <T> CreateProxy method
            var iCalc = ProxyCreator.CreateProxy<ICalculator>(foreignCalculator);
            // Or create a proxy via the CreateGenericProxy method using casting
            var iCalc2 = ProxyCreator.CreateGenericProxy(typeof(ICalculator), foreignCalculator) as ICalculator;

            Console.WriteLine(iCalc.Add(10, 20)); // 30
            Console.WriteLine(iCalc2.Add(10, 20)); // 30
            
            // Success!
            Console.Read();
        }
    }
}
