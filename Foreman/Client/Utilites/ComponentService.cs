﻿using Foreman.PluginManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Foreman.Client.Utilites
{
    public class ComponentService : IComponentService
    {
        public IEnumerable<Type> Components { get; private set; }

        public void LoadComponents(IEnumerable<byte[]> ass)
        {
            var components = new List<Type>();
            var assemblies = LoadAssemblies(ass);

            foreach (var asm in assemblies)
            {
                var types = GetTypesWithInterface(asm);
                foreach (var typ in types) components.Add(typ);
            }

            Components = components;
        }
        public IPluginRazor GetComponentByName(string name)
        {
            return Components.Select(x => (IPluginRazor)Activator.CreateInstance(x))
                .SingleOrDefault(x => x.Name == name);
        }

        public IPluginRazor GetComponentByPage(string name)
        {
            return Components.Select(x => (IPluginRazor)Activator.CreateInstance(x))
                .SingleOrDefault(x => x.Page == name);
        }

        private IEnumerable<Assembly> LoadAssemblies(IEnumerable<byte[]> ass)
        {
            //var temp = Directory.GetDirectories(path);
            return ass.Select(dll => Assembly.Load(dll)).ToList();
        }

        private IEnumerable<Type> GetTypesWithInterface(Assembly asm)
        {
            var it = typeof(IPluginRazor);
            return GetLoadableTypes(asm).Where(it.IsAssignableFrom).ToList();
        }

        private IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}