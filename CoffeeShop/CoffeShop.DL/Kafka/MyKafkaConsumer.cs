using System;
using System.Threading;
using Confluent.Kafka;
using MessagePack;

namespace CoffeShop.DL.Kafka
{
    public class KafkaMessageDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull || data.IsEmpty) return default!;
            return global::MessagePack.MessagePackSerializer.Deserialize<T>(data.ToArray());
        }
    }

    public class GenericKafkaConsumer<TKey, TValue> where TValue : class
    {
        private readonly KafkaSettings _settings;
        private readonly Action<TKey, TValue> _onMessageReceived;

        public GenericKafkaConsumer(KafkaSettings settings, Action<TKey, TValue> onMessageReceived)
        {
            _settings = settings;
            _onMessageReceived = onMessageReceived;
        }

        public void StartConsuming(CancellationToken token)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                GroupId = string.IsNullOrWhiteSpace(_settings.GroupId) ? Guid.NewGuid().ToString() : _settings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Latest,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256,
                SaslUsername = _settings.SaslUsername,
                SaslPassword = _settings.SaslPassword,
                EnableSslCertificateVerification = false
            };

            using var consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetValueDeserializer(new KafkaMessageDeserializer<TValue>())
                .Build();

            consumer.Subscribe(_settings.Topic);

            try
            {
                while (!token.IsCancellationRequested)
                {
                    var result = consumer.Consume(token);

                    if (result?.Message?.Value != null)
                    {
                        _onMessageReceived(result.Message.Key, result.Message.Value);
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
