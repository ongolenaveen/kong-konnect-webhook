using Api.Template.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Api.Template.Services
{
    public class EventsService(ILogger<EventsService> logger) : IEventsService
    {
        public async Task ProcessEvent(string requestContent)
        {
            logger.LogInformation("In the service.");
            await Task.CompletedTask;
        }
    }
}
