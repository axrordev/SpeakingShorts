using System;
using System.Collections.Generic;
using SpeakingShorts.Domain.Commons;

namespace SpeakingShorts.Domain.Entities.Users;

public class User : Auditable
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime RegisteredAt { get; set; }
    public string ProfilePicture { get; set; }
    public long RoleId { get; set; }
    public UserRole Role { get; set; }
    public ICollection<Content> Contents { get; set; }
    public ICollection<Like> Likes { get; set; }
    public ICollection<Comment> Comments { get; set; }
} 