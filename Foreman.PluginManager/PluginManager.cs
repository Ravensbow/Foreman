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

    public interface IPluginInstance
    {
        int Id { get; set; }
        int CourseId { get; set; }
        string? Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? UserCreatorId { get; set; }
    }
}