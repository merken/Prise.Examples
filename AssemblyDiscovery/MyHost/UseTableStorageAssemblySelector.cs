using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Prise.Infrastructure;

namespace MyHost
{
    public class UseTableStorageAssemblySelector<T> : IAssemblySelector<T>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UseTableStorageAssemblySelector(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<Prise.AssemblyScanning.AssemblyScanResult<T>> SelectAssemblies(IEnumerable<Prise.AssemblyScanning.AssemblyScanResult<T>> scanResults)
        {
            if (this.httpContextAccessor.HttpContext.Request.Query.ContainsKey("table"))
                // Only use table storage plugin assemblies when table is in URL querystring
                return scanResults.Where(a => a.PluginType.Name.StartsWith("TableStorage"));

            return scanResults;
        }
    }
}
