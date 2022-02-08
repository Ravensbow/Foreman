using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Services
{
    public interface IPluginService
    {
        int AddPluginInstance(Data.Courses.CourseModule cm);
        int? GetPluginId(string name);
        string GetPluginName(int id);
    }
}
