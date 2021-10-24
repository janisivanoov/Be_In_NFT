namespace TwitterSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Data.UowData;
    using ViewModels;
    using ViewModels.Tweets;

    public class HomeController : BaseController
    {
        private const int TweetsPerPage = 5;

        public HomeController(ITwitterData data)
            : base(data)
        {
        }

        //[OutputCache(Duration = 30, VaryByParam = "page")]
        public ActionResult Index(int page = 1)
        {
           
            var latestTweets = this.Data.Tweets
                .All()
                .OrderByDescending(t => t.TweetedAt)
                .Skip(TweetsPerPage * (page - 1))
                .Take(TweetsPerPage)
                .Project()
                .To<TweetViewModel>();
            var totalPages = (int)Math.Ceiling(this.Data.Tweets.All().Count() / (decimal)TweetsPerPage);
           
            //if (this.User.IsLoggedIn())
            //{
            //    TweetViewModel.SetFavouriteFlags(latestTweets, this.CurrentUser);
            //}

            var indexViewModel = new IndexViewModel
            {
                Tweets = latestTweets,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = page,
                    TotalPages = totalPages
                }
            };

            return this.View(indexViewModel);
        }

        public ActionResult Error()
        {
            return this.View();
        }
    }
}