using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePlugin.Views.Add
{
    public class FilePluginAdd : Foreman.PluginManager.IPluginRazor
    {
        public IDictionary<string, string> Parameters => new Dictionary<string, string>();

        public string Name => "FilePluginadd";

        public string Page => "FilePluginadd";

        public Type Component => typeof(AddComponent);
    }
}
