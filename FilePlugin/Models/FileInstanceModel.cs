using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePlugin.Models
{
    public class FileInstanceModel : Data.FilePluginInstance
    {
        public Foreman.Shared.Models.Category.ForemanFileModel? File { get; set; }
    }
}
