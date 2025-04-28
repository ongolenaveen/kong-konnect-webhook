namespace Api.Template.Services.Interfaces
{
    public interface IEventsService
    {
        Task ProcessEvent(string requestContent);
    }
}
