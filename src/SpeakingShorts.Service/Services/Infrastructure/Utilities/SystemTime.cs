namespace SpeakingShorts.Service.Services.Infrastructure.Utilities
{
    public class SystemTime : ISystemTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}