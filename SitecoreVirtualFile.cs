using ImageResizer.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Links;
using System.Web;

namespace ImageResizer.Sitecore.Plugin
{
    public class SitecoreVirtualFile : IVirtualFile
    {
        protected ResizeSettings query;
        protected HttpRequest request;

        public SitecoreVirtualFile(string virtualPath, NameValueCollection query, HttpRequest request)
        {
            this.request = request;
            this._virtualPath = virtualPath;
            this.query = new ResizeSettings(query);
        }

        public System.IO.Stream Open()
        {
            var tmp = global::Sitecore.Resources.Media.MediaManager.ParseMediaRequest(request);
            if (tmp != null)
            {
                MediaItem item = tmp.MediaUri.Database.GetItem(tmp.MediaUri.MediaPath, tmp.MediaUri.Language, tmp.MediaUri.Version);

                return item.GetMediaStream();
            }

            DynamicLink dynamicLink;
            if (!DynamicLink.TryParse(VirtualPath, out dynamicLink))
                throw new ApplicationException("VirtualImageProviderPlugin : cannot parse virtual path: " + VirtualPath);

            MediaItem mediaItem = global::Sitecore.Context.Database.GetItem(dynamicLink.ItemId, dynamicLink.Language ?? global::Sitecore.Context.Language);

            return mediaItem.GetMediaStream();
        }

        protected string _virtualPath;
        public string VirtualPath
        {
            get { return _virtualPath; }
        }
    }
}
