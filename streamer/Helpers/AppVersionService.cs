using System.Reflection;

namespace streamer.Helpers
{
    //How display application version in ASP.NET Core
    //https://dotnetthoughts.net/how-to-display-app-version-in-aspnet-core/

    /// <summary>
    /// IAppVersionService
    /// </summary>
    public interface IAppVersionService
    {
        /// <summary>
        /// Version
        /// </summary>
        string Version { get; }
    }
    /// <summary>
    /// AppVersionService
    /// </summary>
    public class AppVersionService : IAppVersionService
    {
        /// <summary>
        /// Version
        /// </summary>
        public string Version =>
            Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
