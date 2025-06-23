using System;
using System.Collections.Generic;
using SpeakingShorts.Domain.Commons;

namespace SpeakingShorts.Domain.Entities.Users;

public class User : Auditable
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string PasswordHash { get; set; }
    public DateTime RegisteredAt { get; set; }
    public long? ProfilePictureId { get; set; }
    public Asset ProfilePicture { get; set; }
    public long UserRoleId { get; set; } 
    public UserRole UserRole { get; set; }
    public ICollection<Content> Contents { get; set; }
    public ICollection<Like> Likes { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<UserActivity> UserActivities { get; set; }
    public ICollection<UserCard> UserCards { get; set; }
}