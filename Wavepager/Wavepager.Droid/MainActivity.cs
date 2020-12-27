using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Wavepager.Shared;

namespace Wavepager
{
	[Activity(
			MainLauncher = true,
			ConfigurationChanges = Uno.UI.ActivityHelper.AllConfigChanges,
			WindowSoftInputMode = SoftInput.AdjustPan | SoftInput.StateHidden
        )]
    [IntentFilter(new[] { Intent.ActionView },
            Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
            DataSchemes = new[] { "http", "https" },
            DataHost = "INTENT_HOST",
            AutoVerify = true
        )]
    public class MainActivity : Windows.UI.Xaml.ApplicationActivity
	{
        protected override void OnCreate(Bundle bundle)
        {
            if (Intent.ActionView.Equals(Intent.Action) && Intent.Data != null)
            {
                var url = Intent.Data;
                var urlSender = new URLSender();
                urlSender.OnStarted();
                urlSender.Connect();
                urlSender.PublishUrl(url.ToString());
                Finish();
            }
            base.OnCreate(bundle);
        }
    }
}

