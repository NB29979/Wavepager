using System;
using System.Text.RegularExpressions;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Wavepager
{
    public partial class WebpageAccessor
    {
        private CoreDispatcher Dispatcher;
        public WebpageAccessor()
        {
            Dispatcher = Window.Current.Dispatcher;
        }
        async partial void AccessImpl(string url)
        {
            // URLチェック
            var isURL = Regex.IsMatch(url, @"^s?(https|http)?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$");
            if (isURL == false)
            {
                return;
            }

            await Dispatcher.RunAsync
                    (CoreDispatcherPriority.Normal,
                async () =>
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
                });
        }
    }
}
