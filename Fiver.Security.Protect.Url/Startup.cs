using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Fiver.Security.Protect.Url
{
    public class Startup
    {
        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddMvc();
            services.AddDataProtection()
                    .SetApplicationName("Fiver.Security")
                    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\DATA\Work\_Fiver\Keys"))
                    .SetDefaultKeyLifetime(TimeSpan.FromDays(7)) // 7 days is minimum
                    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
                    {
                         EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC, // default
                         ValidationAlgorithm = ValidationAlgorithm.HMACSHA256   // default
                    });
        }

        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseMvcWithDefaultRoute();
        }
    }
}
