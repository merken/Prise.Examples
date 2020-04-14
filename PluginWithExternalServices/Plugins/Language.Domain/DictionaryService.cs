using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Language.Domain
{
    public interface IDictionaryService
    {
        Task<Dictionary<string, string>> GetLanguageDictionary();
    }

    public class DictionaryService : IDictionaryService
    {
        public async Task<Dictionary<string, string>> GetLanguageDictionary()
        {
            // Reads the dictionary.json file from wherever MyHost.exe is running
            using (var stream = new StreamReader(Path.Combine(GetLocalExecutionPath(), "dictionary.json")))
            {
                var json = await stream.ReadToEndAsync();
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
        }

        private string GetLocalExecutionPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
