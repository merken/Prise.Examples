using System;
using System.Threading.Tasks;

namespace Contract
{
    public interface IHelloPlugin
    {
        string SayHello(string input);
        Task<string> SayHelloAsync(string input);
        //Task<int> SayHelloIntAsync(int input);
    }
}
