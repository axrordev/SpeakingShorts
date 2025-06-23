using System.Threading.Tasks;

namespace SpeakingShorts.Service.Services.Processing
{
    public interface IVideoProcessingService
    {
        Task ProcessVideoAsync(long contentId, long? backgroundMusicId);
    }
} 