using Android.App;
using Android.Content;

namespace Wavepager
{
    public partial class WebpageAccessor
    {
        public Context Context { get; set; }
        partial void AccessImpl(string url)
        {
            var uri = Android.Net.Uri.Parse(url);
            var intent = new Intent(Intent.ActionView, uri);
            intent.SetFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);
        }
    }
}