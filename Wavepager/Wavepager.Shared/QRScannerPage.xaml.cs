using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Capture;
using ZXing;

#if NETFX_CORE
using Windows.Storage;
#elif __ANDROID__
using Android.Graphics;
using Android.Content;
#endif

namespace Wavepager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QRScannerPage : Page
    {
        public QRScannerPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ScanQRCode();
        }
        private async void ScanQRCode()
        {
            try
            {
                var captureUI = new CameraCaptureUI();

                var photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
                Uri photoUri;

                if (photo == null)
                {
                    Frame.Navigate(typeof(MainPage), "");
                    return;
                }
                else
                {
                    photoUri = new Uri(photo.Path);
                    var source = new BitmapImage(photoUri);
                    IMG_QRScannerView.Source = source;

                    var qrCodeReader = new BarcodeReader();
                    qrCodeReader.Options = new ZXing.Common.DecodingOptions
                    {
                        TryHarder = true,
                    };

#if __ANDROID__
                    var cr = Context.ContentResolver;
                    var uri = Android.Net.Uri.Parse(photoUri.AbsoluteUri);
                    var inputStream = cr.OpenInputStream(uri);
                    var bitmap = BitmapFactory.DecodeStream(inputStream);
#elif NETFX_CORE
                    var bitmap = new WriteableBitmap(source.PixelWidth, source.PixelHeight);
                    var stream = await photo.OpenAsync(FileAccessMode.Read);
                    bitmap.SetSource(stream);
#endif
                    var scannedData = qrCodeReader.Decode(bitmap);

                    if(scannedData != null)
                    {
                        Frame.Navigate(typeof(MainPage), scannedData.Text);
                    }
                    else
                    {
                        Frame.Navigate(typeof(MainPage), "");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                Frame.Navigate(typeof(MainPage), "");
            }
        }
    }
}

