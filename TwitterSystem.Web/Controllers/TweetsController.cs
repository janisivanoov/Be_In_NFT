namespace TwitterSystem.Web.Controllers
{
    using AutoMapper.QueryableExtensions;
    using Data.UowData;
    using Hubs;
    using Infrastructure.Helpers;
    using InputModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using TwitterSystem.Models;
    using ViewModels;
    using ViewModels.Alerts;
    using ViewModels.Tweets;

    public class TweetsController : BaseController
    {
        private const int TweetsPerPage = 5;

        public TweetsController(ITwitterData data)
            : base(data)
        {
        }

        public ActionResult GetById(int id)
        {
            var tweet = this.Data.Tweets
                .All()
                .Project()
                .To<TweetViewModel>()
                .FirstOrDefault(t => t.Id == id);

            return this.PartialView("~/Views/Shared/DisplayTemplates/TweetViewModel.cshtml", tweet);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddTweet(TweetInputModel newTweet)
        {
            if (this.ModelState.IsValid && newTweet != null)
            {
                var tweet = new Tweet
                {
                    Text = newTweet.Text,
                    TweetedAt = DateTime.Now,
                    OwnerId = this.CurrentUserId,
                    RetweetCount = 0,
                    SharedCount = 0
                };

                this.Data.Tweets.Add(tweet);
                this.Data.SaveChanges();
                this.AddAlert("Posted successfully", AlertType.Success);
                TweetsHub.OnTweetAdded(tweet.Id, this.GetFollowerIds());
            }
            else
            {
                foreach (var error in this.ModelState.Values.SelectMany(m => m.Errors))
                {
                    this.AddAlert(error.ErrorMessage, AlertType.Error);
                }
            }

            return this.RedirectToAction("Index", "User", new { username = this.User.GetUsername() });
        }

        [HttpPost]
        [Authorize]
        public ActionResult Retweet(int id)
        {
            var tweet = this.Data.Tweets.Find(id);
            if (tweet == null)
            {
                return this.HttpNotFound();
            }

            var newTweet = new Tweet
            {
                Text = tweet.Text,
                OwnerId = this.CurrentUserId,
                RetweetedFromId = id,
                TweetedAt = DateTime.Now,
                RetweetCount = 0,
                SharedCount = 0
            };

            this.Data.Tweets.Add(newTweet);
            tweet.RetweetCount++;
            this.AddRetweetNotification(tweet, this.User.GetUsername());
            this.Data.SaveChanges();
            TweetsHub.OnTweetAdded(newTweet.Id, this.GetFollowerIds());
            NotificationsHub.OnNotificationAdded(tweet.OwnerId);

            return this.Content("Tweet retweeted successfully.");
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetUserTweets(int page = 1)
        {
            var tweets = this.Data.Tweets
                         .All()
                         .Where(t => t.OwnerId == this.CurrentUserId)
                         .OrderByDescending(t => t.TweetedAt)
                         .Project()
                         .To<TweetViewModel>()
                         .ToList();

            var indexViewModel = GetPagitration(page, tweets);

            return this.PartialView("_TweetList", indexViewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetFavouriteTweets(int page = 1)
        {
            var tweets = this.Data.Tweets
                            .All()
                            .Where(t => t.FavouredBy.Any(u => u.Id == this.CurrentUserId))
                            .OrderByDescending(t => t.TweetedAt)
                            .Project()
                            .To<TweetViewModel>()
                            .ToList();

            var indexViewModel = GetPagitration(page, tweets);

            return this.PartialView("_TweetList", indexViewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddFavouriteTweet(int id)
        {
            var tweet = this.Data.Tweets.Find(id);
            if (tweet == null)
            {
                return this.HttpNotFound();
            }

            this.CurrentUser.FavouriteTweets.Add(tweet);
            this.AddFavouriteNotification(tweet, this.User.GetUsername());
            this.Data.SaveChanges();
            NotificationsHub.OnNotificationAdded(tweet.OwnerId);

            this.AddAlert("Tweet added to favorite successfully", AlertType.Success);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [Authorize]
        public ActionResult RemoveFavouriteTweet(int id)
        {
            var tweet = this.Data.Tweets.Find(id);
            if (tweet == null)
            {
                return this.HttpNotFound();
            }

            this.CurrentUser.FavouriteTweets.Remove(tweet);
            this.Data.SaveChanges();

            this.AddAlert("Tweet removed from favorite successfully", AlertType.Success);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ShowReportForm(int id)
        {
            var reportModel = new ReportInputModel { TweetId = id };

            return this.PartialView("~/Views/Shared/EditorTemplates/ReportInputModel.cshtml", reportModel);
        }

        private void AddRetweetNotification(Tweet tweet, string username)
        {
            var notification = new Notification
            {
                Text = username + " retweeted your tweet.",
                NotificationTime = DateTime.Now,
                Type = NotificationType.TweetRetweeted,
                IsRead = false,
                UserId = tweet.OwnerId
            };

            this.Data.Notifications.Add(notification);
        }

        private void AddFavouriteNotification(Tweet tweet, string username)
        {
            var notification = new Notification
            {
                Text = username + " favoured your tweet.",
                NotificationTime = DateTime.Now,
                Type = NotificationType.TweetFavoured,
                IsRead = false,
                UserId = tweet.OwnerId
            };

            this.Data.Notifications.Add(notification);
        }

        private IList<string> GetFollowerIds()
        {
            var followerIds = this.Data.Users
                .All()
                .Where(u => u.Id == this.CurrentUserId)
                .SelectMany(u => u.Followers.Select(f => f.Id))
                .ToList();

            return followerIds;
        }

        private IndexViewModel GetPagitration(int page, List<TweetViewModel> tweets)
        {
            TweetViewModel.SetFavouriteFlags(tweets, this.CurrentUser);

            var totalPages = (int)Math.Ceiling(tweets.Count() / (decimal)TweetsPerPage);
            var indexViewModel = new IndexViewModel
            {
                Tweets = tweets,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = page,
                    TotalPages = totalPages
                }
            };

            return indexViewModel;
        }
    }
}