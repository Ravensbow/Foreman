using Foreman.PluginManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestController
{
    public class Test : IPlugin
    {
        public string GetPluginName() => "Test";

        public string GetPluginVersion() => "2022011501";

        public void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<TestService>();
            services.AddDbContext<TestDbContext>(opt =>
                opt.UseSqlServer(/*configuration.GetConnectionString("SqlServer")*/"Data Source=STACJONARKA\\SQLEXPRESS;Initial Catalog=Foreman;Integrated Security=True"));
        }
        public void Migrate(IServiceCollection services)
        {
            using (TestDbContext db = services.BuildServiceProvider()
                    .GetService<TestDbContext>())
            {
                var test = db.Database.GenerateCreateScript().Replace("GO", "");
                db.Database.ExecuteSqlRaw(test);
            }
        }
    }
}
