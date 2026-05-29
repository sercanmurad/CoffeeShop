using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using MessagePack;

namespace CoffeShop.DL.Kafka
{
    public class KafkaMessageSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
            => global::MessagePack.MessagePackSerializer.Serialize(data);
    }

    public class KafkaSettings
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string SaslUsername { get; set; } = string.Empty;
        public string SaslPassword { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
    }

    public class GenericKafkaProducer<TKey, TValue> : IDisposable where TValue : class
    {
        private readonly IProducer<TKey, TValue> _producer;
        private readonly string _topic;

        public GenericKafkaProducer(KafkaSettings settings)
        {
            _topic = settings.Topic;

            var config = new ProducerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256,
                SaslUsername = settings.SaslUsername,
                SaslPassword = settings.SaslPassword,
                EnableSslCertificateVerification = false
            };

            _producer = new ProducerBuilder<TKey, TValue>(config)
                .SetValueSerializer(new KafkaMessageSerializer<TValue>())
                .Build();
        }

        public virtual async Task ProduceAsync(TKey key, TValue message)
        {
            await _producer.ProduceAsync(_topic, new Message<TKey, TValue>
            {
                Key = key,
                Value = message
            });
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
