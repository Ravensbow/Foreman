using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayedText.Views.Add
{
    public class DisplayedTextAdd : Foreman.PluginManager.IPluginRazor
    {
        public IDictionary<string, string> Parameters => new Dictionary<string, string>();

        public string Name => "displayedtextadd";

        public string Page => "displayedtextadd";

        public Type Component => typeof(Views.Add.AddComponent);
    }
}
