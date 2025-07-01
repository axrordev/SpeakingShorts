using SpeakingShorts.Domain.Entities.Enums;

namespace SpeakingShorts.WebApi.Models.Contents
{
    public class ContentCreateModel
    {
        public string Title { get; set; }
        public ContentType Type { get; set; }
    }
}
