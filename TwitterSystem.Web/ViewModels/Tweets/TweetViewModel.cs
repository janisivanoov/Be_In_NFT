namespace TwitterSystem.Web.ViewModels.Tweets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Infrastructure.Mapping;
    using TwitterSystem.Models;

    public class TweetViewModel : IMapFrom<Tweet>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime TweetedAt { get; set; }

        public int RetweetCount { get; set; }

        public int SharedCount { get; set; }

        public string Username { get; set; }

        public int? RepliedToTweetId { get; set; }

        public bool IsFavouriteToCurrentUser { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Tweet, TweetViewModel>()
                .ForMember(t => t.Username, options => options.MapFrom(t => t.Owner.UserName));
        }

        public static void SetFavouriteFlags(IEnumerable<TweetViewModel> tweets, User currentUser)
        {
            foreach (var tweet in tweets)
            {
                tweet.IsFavouriteToCurrentUser = currentUser.FavouriteTweets.Any(t => t.Id == tweet.Id);
            }
        }
    }
}
