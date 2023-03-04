using Azure.Messaging.ServiceBus;
using CommandService.Data.Entities;
using CommandService.Data.Repos;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusConsumer.Enums;
using ServiceBusConsumer.Models.Platforms;
using System.Text.Json;

namespace ServiceBusConsumer.EventProcessors
{
    public class PlatformsEventProcessor
    {
        #region Constructors

        public PlatformsEventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        #endregion Constructors

        #region Properties

        private readonly IServiceScopeFactory _scopeFactory;

        #endregion Properties

        #region Methods

        private async Task AddPlatformAsync(string messageBody)
        {
            PlatformCreatedEvent createdEvent = JsonSerializer.Deserialize<PlatformCreatedEvent>(messageBody);

            Platform newPlatform = new Platform()
            {
                ExternalId = createdEvent.PlatformId,
                Name = createdEvent.Name
            };

            using (var serviceScope = _scopeFactory.CreateScope())
            {
                PlatformRepo platformRepo = serviceScope.ServiceProvider.GetRequiredService<PlatformRepo>();

                await platformRepo.AddPlatformAsync(newPlatform);
            }
        }

        private async Task UpdatePlatformAsync(string messageBody)
        {
            PlatformUpdatedEvent updatedEvent = JsonSerializer.Deserialize<PlatformUpdatedEvent>(messageBody);

            using (var serviceScope = _scopeFactory.CreateScope())
            {
                PlatformRepo platformRepo = serviceScope.ServiceProvider.GetRequiredService<PlatformRepo>();

                Platform oldPlatform = await platformRepo.
                        GetPlatformByExternalIdAsync(updatedEvent.PlatformId);

                if (oldPlatform == null)
                {
                    throw new Exception("Error while updating the platform - There is no platform " +
                        "with that id. EventId: " + updatedEvent.EventId);
                }
                else
                {
                    oldPlatform.Name = updatedEvent.Name;

                    await platformRepo.UpdatePlatformAsync(oldPlatform);
                }
            }
        }

        private async Task RemovePlatformAsync(string messageBody)
        {
            PlatformRemovedEvent removedEvent = JsonSerializer.Deserialize<PlatformRemovedEvent>(messageBody);

            using (var serviceScope = _scopeFactory.CreateScope())
            {
                PlatformRepo platformRepo = serviceScope.ServiceProvider.GetRequiredService<PlatformRepo>();

                Platform platform = await platformRepo.
                        GetPlatformByExternalIdAsync(removedEvent.PlatformId);

                if (platform == null)
                {
                    throw new Exception("Error while removing the platform - There is no platform " +
                        "with that id. EventId: " + removedEvent.EventId);
                }
                else
                {
                    await platformRepo.RemovePlatformAsync(platform);
                }
            }
        }

        public async Task<bool> ProcessEvent(ServiceBusReceivedMessage message)
        {
            string subject = message.Subject;
            string messageBody = message.Body.ToString();

            try
            {
                if (subject == PlatformEventType.PLATFORM_CREATED.ToString())
                {
                    await AddPlatformAsync(messageBody);
                }
                else if (subject == PlatformEventType.PLATFORM_UPDATED.ToString())
                {
                    await UpdatePlatformAsync(messageBody);
                }
                else if (subject == PlatformEventType.PLATFORM_REMOVED.ToString())
                {
                    await RemovePlatformAsync(messageBody);
                }
                else
                {
                    Console.WriteLine("Wrong message subject!");

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while consuming the platform related message. " + ex.Message);

                return false;
            }
        }

        #endregion Methods
    }
}