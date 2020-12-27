using System;
using System.Text;
using Newtonsoft.Json;
using MQTTnet.Client;

namespace Wavepager.Shared
{
    class URLReceiver : URLClientAdapter
    {
        private WebpageAccessor WebpageAccessor;

        public URLReceiver()
        {
            WebpageAccessor = new WebpageAccessor();
        }
        public override void OnStarted()
        {
            base.OnStarted();

            MqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                var appMessage = e.ApplicationMessage;
                var payload = Encoding.UTF8.GetString(appMessage.Payload, 0, appMessage.Payload.Length);
                try
                {
                    var publishedDataModel = JsonConvert.DeserializeObject<PublishedDataModel>(payload);
                    WebpageAccessor.Access(publishedDataModel.Data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }
    }
}
