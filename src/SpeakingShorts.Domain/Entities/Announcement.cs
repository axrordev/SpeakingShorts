using SpeakingShorts.Domain.Commons;

namespace SpeakingShorts.Domain.Entities;

public class Announcement : Auditable
{
    public string Title { get; set; }  
    public string Message { get; set; } 
    public DateTime ExpireDate { get; set; }  
    public bool IsActive { get; set; }  
}
