using System;
using System.Threading.Tasks;

namespace ExternalServices
{
    public class ExternalObject
    {
        public string Language { get; set; }
        public int Value { get; set; }
    }

    public interface IExternalService
    {
        Task<ExternalObject> ModifyExternalObject(ExternalObject external);
        Task<ExternalObject> GetExternalObjectAsync();
        ExternalObject GetExternalObject();
        //int GetValue();
    }
}
