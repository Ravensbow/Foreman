using Foreman.PluginManager;
using System;
using System.Collections.Generic;

namespace Foreman.Client.Utilites
{
    public interface IComponentService
    {
        void LoadComponents(IEnumerable<byte[]> ass);
        IPluginRazor GetComponentByName(string name);
        IPluginRazor GetComponentByPage(string name);
        IEnumerable<Type> Components { get; }
    }
}
