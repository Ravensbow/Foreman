using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayedText
{
    public class DisplayedTextComponent : Foreman.PluginManager.IPluginRazor
    {
        public IDictionary<string, string> Parameters => new Dictionary<string, string>();

        public string Name => "DisplayedText";

        public string Page => "displayedtext";

        public Type Component => typeof(Views.TextComponent);
    }
}
