using System;
using System.Threading.Tasks;
using ProcessamentoFila.Application.Interfaces.Services;
using ProcessamentoFila.Domain.DTO;
using Confluent.Kafka;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProcessamentoFila.Application.Services.v1
{
    public class MoedaAppService : IMoedaAppService
    {
        private const string nomeTopico = "topico-moeda";

        public async Task<bool> AdicionarMoedaFilaAsync(List<MoedaDto> request)
        {
            try
            {
                string requestJson = JsonConvert.SerializeObject(request);

                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092",
                    EnableIdempotence = true
                };
                 

                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    var result = await producer.ProduceAsync(
                        nomeTopico,
                        new Message<Null, string>
                        { Value = requestJson });

                    if (result.Status == PersistenceStatus.Persisted)
                    {
                        return true;
                    }
                    return false;
                }

            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao adicionar na fila. Tente novamente mais tarde.");
            }

        }


        public async  Task<DadosMoedaResponse> PegarMoedaFilaAsync()
        {
            try
            {
                var response = new DadosMoedaResponse() { DadosMoeda = new List<MoedaDto>()};

                var config = new ConsumerConfig
                {
                    BootstrapServers = "localhost:9092",
                    GroupId = $"{nomeTopico}-group-0",
                    AutoOffsetReset = AutoOffsetReset.Latest,
                    
                };

                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe(nomeTopico);
                    var cts = new CancellationTokenSource();
                    cts.CancelAfter(50000);
                     
                    try
                    {
                        while (true)
                        {
 
                            var resposta =   consumer.Consume(cts.Token);
                            if (resposta != null)
                            {
                                response.DadosMoeda =  Task.FromResult(JsonConvert.DeserializeObject<List<MoedaDto>>(resposta.Message.Value)).Result;

                                consumer.Commit();

                                return response;
                            }
                            return response;
                        }

                    }
                    catch (OperationCanceledException)
                    {
                        consumer.Close();
                    }

                    return new DadosMoedaResponse() { DadosMoeda = new List<MoedaDto>() };

                }
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao buscar o último objeto inserido na fila. Tente novamente mais tarde.");
            }
        }


    }
}
