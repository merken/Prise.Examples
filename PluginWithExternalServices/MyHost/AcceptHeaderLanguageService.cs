using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Contract;
using ExternalServices;
using Microsoft.AspNetCore.Http;
using MyHost.Models;

namespace MyHost
{
    public class AcceptHeaderlanguageService : IExternalService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public AcceptHeaderlanguageService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public ExternalObject GetExternalObject()
        {
            return new ExternalObject
            {
                Language = GetLanguage()
            };
        }

        public async Task<ExternalObject> GetExternalObjectAsync()
        {
            using (var stream = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}\\languages.json"))
            {
                var json = await stream.ReadToEndAsync();
                var results = System.Text.Json.JsonSerializer.Deserialize<List<LanguageInfo>>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new ExternalObject
                {
                    Language = results[0].Culture
                };
            }
        }

        public string GetLanguage()
        {
            return this.httpContextAccessor.HttpContext.Request.Headers["Accept-Language"][0];
        }

        public async Task<ExternalObject> ModifyExternalObject(ExternalObject external)
        {
            using (var stream = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}\\Languages.json"))
            {
                var json = await stream.ReadToEndAsync();
                var results = System.Text.Json.JsonSerializer.Deserialize<List<LanguageInfo>>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new ExternalObject
                {
                    Language = results.First(l => l.Culture == external.Language).Culture
                };
            }
        }

        public int GetValue() => 10;
    }
}
