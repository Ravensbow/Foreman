using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Foreman.Server.Data
{
    public static class DataTool
    {
        public static T Update<T>(T entity, ApplicationContext db, bool ignoreNulls = true)
        {
            db.Attach(entity);
            db.Entry(entity).State = EntityState.Modified;

            var entry = db.Entry(entity);
            Type type = typeof(T);

            var test = db.Model.FindEntityTypes(type).
                SelectMany(t=> t.GetNavigations().Select(x=>x.PropertyInfo));
            PropertyInfo[] properties = type.GetProperties();
            if (ignoreNulls)
            {
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(entity, null) == null && !test.Contains(property))
                    {
                        entry.Property(property.Name).IsModified = false;
                    }
                } 
            }
            db.SaveChanges();
            return entity;
        }
    }
}
