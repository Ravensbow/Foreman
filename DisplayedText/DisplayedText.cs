using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayedText
{
    public class DisplayedText : Foreman.PluginManager.IPlugin
    {
        public string GetPluginName() => Config.pluginName;

        public string GetPluginDescription() => Config.pluginDescription;

        public string GetPluginVersion() => Config.version;

        public void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<Services.DisplayedTextService>();
            services.AddDbContext<Data.DisplayedTextContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("SqlServer")));
        }

        public void Migrate(IServiceCollection services)
        {
            using (Data.DisplayedTextContext db = services.BuildServiceProvider()
                    .GetService<Data.DisplayedTextContext>())
            {
                var test = db.Database.GenerateCreateScript().Replace("GO", "");
                db.Database.ExecuteSqlRaw(test);
            }
        }
    }
}
