using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Prise.AssemblyScanning;
using Prise.Infrastructure;

namespace Products.API.Infrastructure
{
    public class TenantAssemblySelector<T> :
        IAssemblySelector<T>
    {
        protected readonly IHttpContextAccessor contextAccessor;
        protected readonly TenantConfig tenantConfig;

        public TenantAssemblySelector(IHttpContextAccessor contextAccessor, TenantConfig tenantConfig)
        {
                this.tenantConfig = tenantConfig;
            this.contextAccessor = contextAccessor;
        }

        public IEnumerable<AssemblyScanResult<T>> SelectAssemblies(IEnumerable<AssemblyScanResult<T>> scanResults)
        {
            var assemblyName = "OldSQLPlugin";

            if (this.contextAccessor.HttpContext.Request.Headers["Tenant"].Any())
            {
                var tenant = this.contextAccessor.HttpContext.Request.Headers["Tenant"].First();

                var configPair = this.tenantConfig.Configuration
                    .FirstOrDefault(c => String.Compare(c.Tenant, tenant, StringComparison.OrdinalIgnoreCase) == 0);
                assemblyName = configPair.Plugin;
            }

            return scanResults.Where(a => a.PluginType.Assembly.GetName().Name == assemblyName);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}