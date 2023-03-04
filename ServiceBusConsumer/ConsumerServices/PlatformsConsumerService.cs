using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ServiceBusConsumer.EventProcessors;

namespace ServiceBusConsumer.ConsumerServices
{
    public class PlatformsConsumerService : BackgroundService
    {
        #region Constructors

        public PlatformsConsumerService(IConfiguration configuration, PlatformsEventProcessor eventProcessor)
        {
            _platformsProcessor = GetProcessorAsync(configuration);
            _eventProcessor = eventProcessor;
        }

        #endregion Constructors

        #region Properties

        private readonly ServiceBusProcessor _platformsProcessor;
        private readonly PlatformsEventProcessor _eventProcessor;

        #endregion Properties

        #region Methods

        private ServiceBusProcessor GetProcessorAsync(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("ServiceBusConnection");
            ServiceBusClientOptions clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            ServiceBusClient client = new ServiceBusClient(connectionString, clientOptions);

            IConfigurationSection serviceBusSection = configuration.GetSection("ServiceBus");
            string topicName = serviceBusSection["TopicName"];
            string subscriptionName = serviceBusSection["PlatformsSubName"];

            ServiceBusProcessorOptions processorOptions = new ServiceBusProcessorOptions()
            {
                AutoCompleteMessages = false
            };

            ServiceBusProcessor result = client.CreateProcessor(topicName, subscriptionName, processorOptions);

            return result;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _platformsProcessor.ProcessMessageAsync += HandleMessageAsync;
            _platformsProcessor.ProcessErrorAsync += HandleErrorsAsync;

            async Task HandleMessageAsync(ProcessMessageEventArgs messageArgs)
            {
                bool result = await _eventProcessor.ProcessEvent(messageArgs.Message);

                if (result == true)
                {
                    await messageArgs.CompleteMessageAsync(messageArgs.Message);
                }
                else
                {
                    await messageArgs.AbandonMessageAsync(messageArgs.Message);
                }
            }

            Task HandleErrorsAsync(ProcessErrorEventArgs errorArgs)
            {
                string errorMessage = errorArgs.Exception.Message;

                Console.WriteLine(errorMessage);

                return Task.CompletedTask;
            }

            await _platformsProcessor.StartProcessingAsync();
        }

        public override async void Dispose()
        {
            if (_platformsProcessor.IsClosed == false)
            {
                await _platformsProcessor.CloseAsync();
                await _platformsProcessor.DisposeAsync();
            }
            base.Dispose();
        }

        #endregion Methods
    }
}