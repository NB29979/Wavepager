using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;

#if NETFX_CORE
using System.IO;
using Windows.Storage;
#elif __ANDROID__
using Android.App;
using Android.Preferences;
#endif

namespace Wavepager.Shared
{
    class URLClientAdapter
    {
        protected IMqttClient MqttClient;
        protected IMqttClientOptions Options;

        public ChannelProperty Prop;

        public class ChannelProperty
        {
            [JsonProperty("Host")]
            public string Host { get; set; }
            [JsonProperty("Topic")]
            public string Topic { get; set; }
            [JsonProperty("Token")]
            public string Token { get; set; }
            [JsonProperty("Port")]
            public int Port { get; set; }
            public PropState PropStatus { get; set; }
            public enum PropState
            {
                PROP_INITIALIZED,
                PROP_SET,
                PROP_UPDATE
            };
            public ChannelProperty()
            {
                Initialize();
            }
            public void Initialize()
            {
                PropStatus = PropState.PROP_INITIALIZED;
            }
        }

        public URLClientAdapter()
        {
            Prop = new ChannelProperty();
#if NETFX_CORE
            SetProp(File.ReadAllText(@"Assets/ChannelProperty.json"));
#elif __ANDROID__
            var context = Uno.UI.ContextHelper.Current;
            var p = PreferenceManager.GetDefaultSharedPreferences(context);

            var channelInfo= p.GetString("CHANNEL_INFO", "" );
            if(channelInfo != "")
            {
                SetProp(channelInfo);
            }
#endif
        }

        public void Initialize()
        {
            Prop.Initialize();
        }
        
        public bool IsInitialized()
        {
            return Prop.PropStatus == ChannelProperty.PropState.PROP_INITIALIZED;
        }

        public void SetProp(string json)
        {
            Prop = JsonConvert.DeserializeObject<ChannelProperty>(json);
            Prop.PropStatus = ChannelProperty.PropState.PROP_SET;

            var mqttFactory = new MqttFactory();
            MqttClient = mqttFactory.CreateMqttClient();
            
            Options = new MqttClientOptionsBuilder()
                .WithTcpServer(Prop.Host, Prop.Port)
                .WithCredentials(Prop.Token, "")
                .WithTls()
                .WithCleanSession()
                .Build();
        }
        public void SaveSettings() { }

        public virtual void OnStarted()
        {
            MqttClient.UseConnectedHandler(async e =>
            {
                _ = await MqttClient.SubscribeAsync(new TopicFilterBuilder()
                    .WithTopic(Prop.Topic)
                    .Build());
            });
            MqttClient.UseDisconnectedHandler(async e =>
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                try
                {
                    Connect();
                }
                catch { }
            });
        }
        public async void Connect()
        {
            var retry = 0;
            while (!MqttClient.IsConnected && retry < 10)
            {
                try
                {
                    MqttClient.ConnectAsync(Options, CancellationToken.None).Wait();
                }
                catch
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                ++retry;
            }
        }
    }
}
