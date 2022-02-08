using Foreman.Server.Data;
using Foreman.Shared.Data.Courses;
using Foreman.Shared.Services;
using System.Linq;

namespace Foreman.Server.Services
{
    public class PluginService : IPluginService
    {
        private readonly ApplicationContext db;
        public PluginService(ApplicationContext ctx)
        {
            db = ctx;
        }
        public int AddPluginInstance(CourseModule cm)
        {
            db.CourseModules.Add(cm);  
            db.SaveChanges();
            return cm.Id;
        }

        public int? GetPluginId(string name)
        {
            return db.Plugins.SingleOrDefault(x=> x.Name == name)?.Id; 
        }

        public string GetPluginName(int id)
        {
            return db.Plugins.Find(id).Name;
        }
    }
}
