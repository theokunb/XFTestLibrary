using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XFTestLibrary.Services
{
    public class ImageExtension : IMarkupExtension
    {
        public string Source { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Source))
                return null;
            return ImageSource.FromResource(Source, typeof(ImageExtension).GetTypeInfo().Assembly);
        }
    }
}
