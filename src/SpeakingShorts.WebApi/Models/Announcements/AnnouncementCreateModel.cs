using SpeakingShorts.Domain.Commons;

namespace SpeakingShorts.WebApi.Models.Announcements;

public class AnnouncementCreateModel
{
    public string Title { get; set; }  
    public string Message { get; set; } 
    public DateTime ExpireDate { get; set; }  
}
