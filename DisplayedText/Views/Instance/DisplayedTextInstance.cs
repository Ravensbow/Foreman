﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayedText.Views.Instance
{
    public class DisplayedTextInstance : Foreman.PluginManager.IPluginRazor
    {
        public IDictionary<string, string> Parameters => new Dictionary<string, string>();

        public string Name => "displayedtext";

        public string Page => "displayedtext";

        public Type Component => typeof(Views.Instance.Instance);
    }
}
