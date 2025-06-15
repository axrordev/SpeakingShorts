using System;
using System.Collections.Generic;
using SpeakingShorts.Domain.Commons;
using SpeakingShorts.Domain.Entities.Enums;
using SpeakingShorts.Domain.Entities.Users;

namespace SpeakingShorts.Domain.Entities;

public partial class Content : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    // Backblaze B2 fields
    public string FileKey { get; set; }  // Takrorlanmas bo'lishi uchun UUID qo'shilishi mumkin
    public string FileUrl { get; set; }  // Dynamic URL, yuklashda yangilanadi
    public long FileSize { get; set; }   // Bytes
    
    public ContentType Type { get; set; }
    public int DurationLimit { get; set; }    // admin belgilagan vaqt limiti
    public int Duration { get; set; }    // yuklangan faylning vaqtini o'lchash
    public bool IsTopContent { get; set; }
    
    // Background music fields
    public long? BackgroundMusicId { get; set; }
    public BackgroundMusic BackgroundMusic { get; set; }
    public ContentStatus Status { get; set; } = ContentStatus.Pending;
    
    public ICollection<Like> Likes { get; set; }
    public ICollection<Comment> Comments { get; set; }
}
