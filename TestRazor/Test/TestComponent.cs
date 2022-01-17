using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestRazor.Test
{
    public class TestComponent : Foreman.PluginManager.IPluginRazor
    {
        public IDictionary<string, string> Parameters => new Dictionary<string,string>();

        public string Name => "test";

        public string Page => "test";

        public Type Component => typeof(TestRazor.Test.Test);

        public void LoadMainDll(byte[] dllData)
        {
            Assembly.Load(dllData);
        }
    }
}
