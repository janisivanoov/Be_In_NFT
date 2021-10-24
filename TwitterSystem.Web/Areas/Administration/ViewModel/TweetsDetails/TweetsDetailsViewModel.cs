namespace TwitterSystem.Web.Areas.Administration.ViewModel.TweetsDetails
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using Infrastructure.Mapping;
    using TwitterSystem.Models;
    using ViewModels.Tweets;

    public class TweetsDetailsViewModel : IMapFrom<Tweet>, IHaveCustomMappings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [DisplayName("Date")]
        public DateTime TweetedAt { get; set; }

        [Required]
        public string Username { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Tweet, TweetsDetailsViewModel>()
                .ForMember(t => t.Username, options => options.MapFrom(t => t.Owner.UserName));
        }
    }
}