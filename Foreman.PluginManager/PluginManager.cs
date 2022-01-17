using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foreman.PluginManager
{
    public interface IPlugin
    {
        public string GetPluginName();
        public string GetPluginDescription();
        public string GetPluginVersion();
        void Initialize(IServiceCollection services, IConfiguration configuration);
        void Migrate(IServiceCollection services);
    }
    public interface IPluginRazor
    {
        IDictionary<string, string> Parameters { get; }
        string Name { get; }
        string Page { get; }
        Type Component { get; }
    }
}