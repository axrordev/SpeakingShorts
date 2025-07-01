using Amazon.S3.Model;
using AutoMapper;
using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.WebApi.Models.Users;
using SpeakingShorts.WebApi.Models.Announcements;
using SpeakingShorts.WebApi.Models.Comments;
using SpeakingShorts.WebApi.Models.Likes;
using SpeakingShorts.WebApi.Models.MarkedWords;
using SpeakingShorts.WebApi.Models.Stories;
using SpeakingShorts.WebApi.Models.UserCards;
using SpeakingShorts.WebApi.Models.Contents;
using SpeakingShorts.WebApi.Models.WeeklyRankings;

namespace SpeakingShorts.WebApi.MappingProfile
{

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User and related entities
        CreateMap<UserRegisterModel, User>();
        CreateMap<UserUpdateModel, User>();
        CreateMap<User, UserViewModel>();
        CreateMap<User, LoginViewModel>();

        // Announcement
        CreateMap<AnnouncementCreateModel, Announcement>();
        CreateMap<AnnouncementModifyModel, Announcement>();
        CreateMap<Announcement, AnnouncementViewModel>();

        // Comment
        CreateMap<CommentCreateModel, Comment>();
        CreateMap<CommentModifyModel, Comment>();
        CreateMap<Comment, CommentViewModel>();

        // Like
        CreateMap<LikeCreateModel, Like>();
        CreateMap<Like, LikeViewModel>();

        // MarkedWord
        CreateMap<MarkedWordCreateModel, MarkedWord>();
        CreateMap<MarkedWordModifyModel, MarkedWord>();
        CreateMap<MarkedWord, MarkedWordViewModel>();

        // Story
        CreateMap<StoryCreateModel, Story>();
        CreateMap<StoryModifyModel, Story>();
        CreateMap<Story, StoryViewModel>();

        // UserCard
        CreateMap<UserCardCreateModel, UserCard>();
        CreateMap<UserCardModifyModel, UserCard>();
        CreateMap<UserCard, UserCardViewModel>();

        // Content
        CreateMap<ContentCreateModel, Content>();
        CreateMap<ContentModifyModel, Content>();
        CreateMap<Content, ContentViewModel>();

        // WeeklyRanking
        CreateMap<WeeklyRanking, WeeklyRankingViewModel>();
    }
}
}
