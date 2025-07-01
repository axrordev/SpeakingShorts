using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Domain.Entities.Enums;
using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.WebApi.Models.Comments;
using SpeakingShorts.WebApi.Models.Likes;
using SpeakingShorts.WebApi.Models.Users;

public class ContentViewModel
{
    public long Id { get; set; }
    public UserViewModel User { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    // Backblaze B2 fields
    public string FileKey { get; set; }  // Takrorlanmas bo'lishi uchun UUID qo'shilishi mumkin
    public string FileUrl { get; set; }  // Dynamic URL, yuklashda yangilanadi
    public long FileSize { get; set; }   // Bytes
    
    public ContentType Type { get; set; }
    public ContentStatus Status { get; set; } = ContentStatus.Pending; 
  
    public int DurationLimit { get; set; }    // admin belgilagan vaqt limiti
    public int Duration { get; set; }    // yuklangan faylning vaqtini o'lchash
    public bool IsTopContent { get; set; }
    
    public ICollection<LikeViewModel> Likes { get; set; }
    public ICollection<CommentViewModel> Comments { get; set; }
}

