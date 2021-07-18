
using System;
using Confluent.Kafka;

namespace ZebraApi.Services
{
    public class PrintService : IPrintService
    {
        public void PrintHandler(DeliveryReport<Null, string> report)
        {
            Console.WriteLine("Produced " + report.Value);
        }
    }
}