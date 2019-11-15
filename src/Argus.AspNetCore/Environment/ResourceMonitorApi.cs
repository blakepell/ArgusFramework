using Argus.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Argus.AspNetCore.Environment
{

    /// <summary>
    /// The resource methods that are used via the ResourceMonitorApi.  These methods are wired
    /// up in the "void Configure" of the Startup class of the site with:
    /// 
    /// <code>
    ///     app.MapResourceMonitor();
    ///     app.MapResourceMonitor("Authorization", "an-example-key-value");
    /// </code>
    /// </summary>
    public class ResourceMonitorApi
    {
        /// <summary>
        /// Returns a response of "pong" to the requested "ping".
        /// </summary>
        public static void Ping(IApplicationBuilder app)
        {            
            app.Run(async context =>
            {
                await context.Response.WriteAsync("pong");
            });
        }

        /// <summary>
        /// Returns the current server time.
        /// </summary>
        /// <param name="app"></param>
        public static void ServerTime(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync(DateTime.Now.ToString());
            });
        }

        /// <summary>
        /// Returns the machine name of the executing server.
        /// </summary>
        /// <param name="app"></param>
        public static void MachineName(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync(System.Environment.MachineName);
            });
        }

        /// <summary>
        /// Returns detailed machine information.
        /// </summary>
        /// <param name="app"></param>
        public static void Detailed(IApplicationBuilder app)
        {            
            app.Run(async context =>
            {
                var ri = new ResourceInfo();
                ri.MachineName = System.Environment.MachineName;
                ri.Is64Bit = System.Environment.Is64BitOperatingSystem;
                ri.Platform = System.Environment.OSVersion.VersionString;
                ri.ProcessorCount = System.Environment.ProcessorCount;
                ri.ServerTime = DateTime.Now;                
                ri.MemoryInfo = Memory.CurrentSystemMemory();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    ri.OperatingSystem = "Windows";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    ri.OperatingSystem = "Linux";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    ri.OperatingSystem = "OSX";
                }
                else if(RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                {
                    ri.OperatingSystem = "FreeBSD";
                }
                else
                {
                    ri.OperatingSystem = "Unknown";
                }
                                
                await context.Response.WriteAsync(JsonSerializer.Serialize(ri));
            });

        }

    }
}