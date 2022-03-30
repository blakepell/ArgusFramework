using Microsoft.AspNetCore.Builder;

namespace ApexGate.Extensions
{
    /// <summary>
    /// <see cref="WebApplication"/>
    /// </summary>
    public static class WebExtensions
    {
        public static void AddResourceUsage(this WebApplication? app)
        {
            if (app == null)
            {
                return;
            }

            app.MapGet("/resource-usage", () =>
            {
                var processList = System.Diagnostics.Process.GetProcesses();
                var sb = Argus.Memory.StringBuilderPool.Take();

                foreach (var item in processList)
                {
                    try
                    {
                        item.Refresh();

                        if (item.ProcessName.Contains("dotnet") || item.ProcessName.Contains("ApexGate"))
                        {
                            sb.Append(item.ProcessName).Append('|').Append(item.WorkingSet64 / 1024 / 1024).Append("MB").Append(Environment.NewLine);
                        }
                    }
                    catch
                    {
                        // Eat this error, it's possible that the process isn't available anymore by the time
                        // we get to the loop and that will throw an exception, in our case we just won't show
                        // that in our list (since it isn't valid anymore).
                    }
                }

                try
                {
                    return sb.ToString();
                }
                finally
                {
                    Argus.Memory.StringBuilderPool.Return(sb);
                }
            });
        }
    }
}