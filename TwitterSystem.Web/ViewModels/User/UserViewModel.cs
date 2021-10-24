namespace TwitterSystem.Web.ViewModels.User
{
    using AutoMapper;
    using Infrastructure.Mapping;
    using TwitterSystem.Models;

    public class UserViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public bool IsCurrentUser { get; set; }

        public bool IsFollowedByCurrentUser { get; set; }

        public int Tweets { get; set; }

        public int Following { get; set; }

        public int Followers { get; set; }

        public int FavouriteTweets { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserViewModel>()
                .ForMember(u => u.Followers, options => options.MapFrom(u => u.Followers.Count))
                .ForMember(u => u.Following, options => options.MapFrom(u => u.Following.Count))
                .ForMember(u => u.Tweets, options => options.MapFrom(u => u.Tweets.Count))
                .ForMember(u => u.FavouriteTweets, options => options.MapFrom(u => u.FavouriteTweets.Count));
        }
    }
}
