using System;
using System.Net;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZebraApi.Models;
using ZebraApi.Services;
using System.Net.Http;

namespace ZebraApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrintController : ControllerBase
    {
        private readonly ILogger<PrintController> _logger;
        public IPrintService printService = new PrintService();

        public PrintController(ILogger<PrintController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public HttpResponseMessage  Post(Print print)
        {
            string serializedPrint = JsonSerializer.Serialize(print);

            //todo: Criar Config no startup da aplicação
            var config = new ProducerConfig { BootstrapServers = "kafka:29092" };

            using var p = new ProducerBuilder<Null, string>(config).Build();
            {
                try
                {
                    //todo: parametrizar o tópico
                    //todo: validar se foi postado ou não na fila
                    p.Produce("topic", new Message<Null, string> { Value = serializedPrint }, printService.PrintHandler);
                    p.Flush(TimeSpan.FromSeconds(10));
                    return new HttpResponseMessage(HttpStatusCode.OK);

                }
                catch (Exception e)
                {
                   _logger.LogError(e.Source);
                   return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
        }
    }
}