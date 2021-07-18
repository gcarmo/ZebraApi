
using Confluent.Kafka;

namespace ZebraApi.Services
{
    public interface IPrintService
    {
        void PrintHandler(DeliveryReport<Null, string> report);
    }
}