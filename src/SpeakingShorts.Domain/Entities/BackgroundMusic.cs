using System;
using System.Collections.Generic;
using SpeakingShorts.Domain.Commons;

namespace SpeakingShorts.Domain.Entities;

public class BackgroundMusic : Auditable
{
    public string Title { get; set; }
    
    // Backblaze B2 fields
    public string FileKey { get; set; }  // Takrorlanmas bo'lishi uchun UUID qo'shilishi mumkin
    public string FileUrl { get; set; }  // Dynamic URL, yuklashda yangilanadi
    public long FileSize { get; set; }   // Bytes
    
    public bool IsActive { get; set; }
    public int Duration { get; set; }    // Sekundlarda
    
    public ICollection<Content> Contents { get; set; }
} 