using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevTools.Kafka.Services
{
    /// <summary>
    /// Сериализатор сообщений Kafka
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    public class KafkaMessageSerializer<TPayload> : ISerializer<TPayload>
    {
        /// <summary>
        /// Сериализует сообщение
        /// </summary>
        /// <param name="data">Тело сообщения</param>
        /// <param name="context">Контекст сериализации</param>
        /// <returns>Сообщение после сериализации в формате массива байтов</returns>
        public byte[] Serialize(TPayload data, SerializationContext context) => JsonSerializer.SerializeToUtf8Bytes(data);
    }
}
