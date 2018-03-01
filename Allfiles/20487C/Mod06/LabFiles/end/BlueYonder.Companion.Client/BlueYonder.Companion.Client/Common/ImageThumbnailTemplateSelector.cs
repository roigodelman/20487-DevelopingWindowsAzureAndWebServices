using BlueYonder.Companion.Client.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BlueYonder.Companion.Client.Common
{
    public class ImageThumbnailTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null) return base.SelectTemplateCore(item, container);

            var mediaItem = (MediaItem)item;
            var templateName = mediaItem.FolderType == FolderType.Local ? "Local" : "Remote";
            templateName += mediaItem.Name.EndsWith(".mp4") ? "Video" : "Image";
            templateName += "ThumbnailTemplate";
            return (DataTemplate)Application.Current.Resources[templateName];
        }
    }
}
