using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using ZXing;
using Newtonsoft.Json;
using Wavepager.Shared;

namespace Wavepager
{
    public sealed partial class MainPage : Page
    {
        private URLReceiver URLReceiver;
        private URLSender URLSender;
        public MainPage()
        {
            URLReceiver = new URLReceiver();
            URLSender = new URLSender();
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string qrText = "";
            string scannedResult = (string)e.Parameter;

            // QRコードを読み込んで遷移してきたとき
            if(scannedResult != null && scannedResult != "")
            {
                qrText = scannedResult.ToString();
                URLReceiver.SetProp(qrText);
                generateQrImage(qrText);
                URLReceiver.OnStarted();
                URLSender.OnStarted();
                URLReceiver.Connect();
                URLSender.OnStarted();
            }
            // QRコードを読み込んで遷移していないが，すでにQRPropはある状態
            else if (URLReceiver.Prop.PropStatus == URLReceiver.ChannelProperty.PropState.PROP_SET)
            {
                qrText = JsonConvert.SerializeObject(URLReceiver.Prop);
                generateQrImage(qrText);
                URLReceiver.OnStarted();
                URLSender.OnStarted();
                URLReceiver.Connect();
                URLSender.Connect();
            }
            // QRコードを読み込んで遷移しておらず，QRPropもない状態
            else 
            {
                SetInitialImage();
            }
        }
        public void BTN_SendURL_Click(object sender, RoutedEventArgs e)
        {
            var inputURL = TXB_URL.Text.ToString();
            URLSender.PublishUrl(inputURL);
        }

        public void BTN_SetToken_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(QRScannerPage));
        }
        public void BTN_Initialize_Click(object sender, RoutedEventArgs e)
        {
            SetInitialImage();
        }
        private void generateQrImage(string qrText)
        {
            var qrCodeWriter = new ZXing.BarcodeWriter {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.QrCode.QrCodeEncodingOptions
                {
                    CharacterSet = "UTF-8",
                    Height = 200,
                    Width = 200,
                }
            };
            IMG_QrCode.Source = qrCodeWriter.Write(qrText);
        }
        private void SetInitialImage()
        {
            URLReceiver.Initialize();

            var uri =new Uri (@"ms-appx:///Square150x150Logo_scale_200.png");
            IMG_QrCode.Source = new BitmapImage(uri);
        }
    }
}
