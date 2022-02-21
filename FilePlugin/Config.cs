using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePlugin
{
    public static class Config
    {
        public static readonly string version = "2022011501";
        public static readonly string FilePlugin = "FilePlugin";
        public static readonly string pluginDescription = "Plik do pobrania";
        public static readonly string pluginIcon = MudBlazor.Icons.Filled.AttachFile;
    }
    public static class FileMIMEIcons
    {
        public static string Get(string mime)
        {
            if(mime.StartsWith("image/"))
            {
                return MudBlazor.Icons.Filled.Image;
            }
            else if(mime.StartsWith("text/"))
            {
                return MudBlazor.Icons.Filled.TextSnippet;
            }
            else if(mime.StartsWith("video/"))
            {
                return MudBlazor.Icons.Filled.Videocam;
            }
            else if(mime == "application/pdf")
            {
                return MudBlazor.Icons.Filled.PictureAsPdf;
            }
            else
            {
                return MudBlazor.Icons.Filled.InsertDriveFile;
            }
        }
    }
}
