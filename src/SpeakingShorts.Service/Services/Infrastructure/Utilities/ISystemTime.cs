namespace SpeakingShorts.Service.Services.Infrastructure.Utilities
{
    public interface ISystemTime
    {
        DateTime UtcNow { get; }
    }
}