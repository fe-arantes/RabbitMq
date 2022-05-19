using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SenderApi.Models;
using SenderApi.Options;
using System.Text;

namespace SenderApi.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ConnectionFactory _factory;
        private readonly RabbitMqConfiguration _config;

        public MessagesController(IOptions<RabbitMqConfiguration> options)
        {
            _config = options.Value;

            _factory = new ConnectionFactory
            {
                HostName = _config.Host
            };
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] MessageInputModel message)
        {
            using ( var connection = _factory.CreateConnection() )
            {
                using ( var channel = connection.CreateModel() ) //Cria a fila se necessário
                {
                    channel.QueueDeclare(           //Configura a fila
                        queue: _config.Queue,
                        durable: false,             //Persistencia da mensagem
                        exclusive: false,           //Permite apenas uma conexão e após encerrar a fila é apagada
                        autoDelete: false,          //A fila vai ser apagada após todos se desconectaram e ela ficar sem conexões ativas.
                        arguments: null);

                    var stringfiedMessage = JsonConvert.SerializeObject(message);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(           //Publica a mensagem
                        exchange: "",               //Agentes responsáveis por rotear as mensagens para filas, utilizando atributos de cabeçalho, routing keys, ou bindings.
                        routingKey: _config.Queue,  //Descrevendo para qual fila a mensagem deve ser direcionada.
                        basicProperties: null,
                        body: bytesMessage);
                }
            }

            return Accepted();
        }
    }
}
