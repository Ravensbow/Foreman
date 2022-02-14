using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePlugin.Views.Instance
{
    public class FilePluginInstance : Foreman.PluginManager.IPluginRazor
    {
        public IDictionary<string, string> Parameters => new Dictionary<string, string>();

        public string Name => "FilePlugininstance";

        public string Page => "FilePlugininstance";

        public Type Component => typeof(InstanceComponent);
    }
}
