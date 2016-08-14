using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer.Sitecore.Plugin
{
    public static class ImageExtensions
    {
        public static bool HasImage(this Image image)
        {
            return image != null && !string.IsNullOrEmpty(image.Src);
        }

        public static string GetSafeSrc(this Image image)
        {
            // We need the media item url to be in the 'DynamicLink' format so we can utilize the image resizer.  :)
            if (image.HasImage())
            {
                var url = string.Format("~/media/{0}{1}", image.MediaId.ToString("N"), image.Src.Substring(image.Src.LastIndexOf(".")));

                return url;
            }

            return "";
        }

        public static string GetSafeAlt(this Image image)
        {
            // We need the media item url to be in the 'DynamicLink' format so we can utilize the image resizer.  :)
            if (image.HasImage())
            {
                return image.Alt;
            }

            return "";
        }
    }
}
