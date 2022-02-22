using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePlugin
{
    public class FilePlugin : Foreman.PluginManager.IPlugin
    {
        public string GetPluginName() => Config.FilePlugin;

        public string GetPluginDescription() => Config.pluginDescription;

        public string GetPluginVersion() => Config.version;
        public string GetPluginIcon() => Config.pluginIcon;

        public void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<Services.FilePluginService>();
            services.AddDbContext<Data.FilePluginContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("SqlServer")));
        }

        public void Migrate(IServiceCollection services)
        {
            using (Data.FilePluginContext db = services.BuildServiceProvider()
                    .GetService<Data.FilePluginContext>())
            {
                var test = db.Database.GenerateCreateScript().Replace("GO", "");
                db.Database.ExecuteSqlRaw(test);
            }
        }

        public List<string?> GetPluginDbTables(IServiceCollection services)
        {
            using (Data.FilePluginContext db = services.BuildServiceProvider()
                    .GetService<Data.FilePluginContext>())
            {
                return db.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Distinct()
                .ToList();
            }
        }
    }
}
