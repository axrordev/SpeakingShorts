namespace SpeakingShorts.WebApi.Models.Announcements;

public class AnnouncementViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }  
    public string Message { get; set; } 
    public DateTime ExpireDate { get; set; }  
    public bool IsActive { get; set; }  
}
