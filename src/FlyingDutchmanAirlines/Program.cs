using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FlyingDutchmanAirlines
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await InitializeHost();
        }

        public static async Task InitializeHost()
        {
            await Host
                .CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder =>
                    {
                        builder.UseUrls("https://0.0.0.0:12345");
                        builder.UseStartup<Startup>();
                    })
                .Build()
                .RunAsync();
        }
    }
}
