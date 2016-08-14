using ImageResizer.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Links;
using System.Web;

namespace ImageResizer.Sitecore.Plugin
{
    public class SitecoreVirtualImageProviderPlugin : IPlugin, IVirtualImageProvider, IFileExtensionPlugin
    {
        public IPlugin Install(global::ImageResizer.Configuration.Config c)
        {
            c.Plugins.add_plugin(this);
            return this;
        }

        public bool Uninstall(global::ImageResizer.Configuration.Config c)
        {
            c.Plugins.remove_plugin(this);
            return true;
        }

        private string FixVirtualPath(string virtualPath)
        {
            return virtualPath.Substring(virtualPath.LastIndexOf("~")); // LOL it's the only way, I'm so sorry but it's true.
        }

        public bool FileExists(string virtualPath, NameValueCollection queryString)
        {
            var tmp = global::Sitecore.Resources.Media.MediaManager.ParseMediaRequest(HttpContext.Current.Request);
            
            virtualPath = FixVirtualPath(virtualPath);
            
            DynamicLink dynamicLink;
            return tmp != null || DynamicLink.TryParse(virtualPath, out dynamicLink);
        }

        public IVirtualFile GetFile(string virtualPath, NameValueCollection queryString)
        {
            virtualPath = FixVirtualPath(virtualPath);

            return new SitecoreVirtualFile(virtualPath, queryString, HttpContext.Current.Request);
        }

        public IEnumerable<string> GetSupportedFileExtensions()
        {
            return new string[] { "ashx" };
        }
    }
}
