using System;
using System.Threading;
using MQTTnet;
using Newtonsoft.Json;

namespace Wavepager.Shared
{
    class URLSender : URLClientAdapter
    {
        public void PublishUrl(string url)
        {
            var dataModel = new PublishedDataModel(url, DateTime.Now.ToString("yyyyMMddHHmmss") ,false);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(Prop.Topic)
                .WithPayload(JsonConvert.SerializeObject(dataModel))
                .WithAtLeastOnceQoS()
                .WithRetainFlag()
                .Build();

            MqttClient.PublishAsync(message, CancellationToken.None);
        }
    }
}

