namespace TwitterSystem.Web.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Data.UowData;
    using Infrastructure.Helpers;
    using InputModels;
    using Hubs;
    using TwitterSystem.Models;
    using ViewModels;
    using ViewModels.Tweets;
    using ViewModels.User;

    public class UserController : BaseController
    {
        private const int TweetsPerPage = 5;

        public UserController(ITwitterData data)
            : base(data)
        {
        }

        [Authorize]
        public ActionResult Index(int page = 1)
        {
            var latestTweets = this.Data.Tweets
                                      .All()
                                      .Where(t => t.OwnerId == this.CurrentUserId)
                                      .OrderByDescending(t => t.TweetedAt);

            var totalPages = (int)Math.Ceiling(latestTweets.Count() / (decimal)TweetsPerPage);
            var tweetViewModels = latestTweets
                            .Skip(TweetsPerPage * (page - 1))
                            .Take(TweetsPerPage)
                            .Project()
                            .To<TweetViewModel>()
                            .ToList();

            TweetViewModel.SetFavouriteFlags(tweetViewModels, this.CurrentUser);

            var indexViewModel = new IndexViewModel
            {
                Tweets = tweetViewModels,
                PaginationModel = new PaginationViewModel
                {
                    CurrentPage = page,
                    TotalPages = totalPages
                }
            };

            return this.View(indexViewModel);
        }

        [Authorize]
        public ActionResult AddNewTweet()
        {
            var userNewTweetModel = new UserAddNewTweetModel();
            return this.View(userNewTweetModel);
        }

        [Authorize]
        [ActionName("Profile")]
        public ActionResult UserProfile(string username)
        {
            var user = this.Data.Users
                        .All()
                        .Include(u => u.Followers)
                        .Include(u => u.Following)
                        .Include(u => u.Tweets)
                        .Include(u => u.FavouriteTweets)
                        .Where(u => u.UserName == username)
                        .Project()
                        .To<UserViewModel>()
                        .FirstOrDefault();

            if (user == null)
            {
                return this.HttpNotFound();
            }

            user.IsCurrentUser = this.User.IsLoggedIn() && this.User.GetUsername() == user.UserName;
            user.IsFollowedByCurrentUser = !user.IsCurrentUser && this.User.IsLoggedIn() &&
                                             this.Data.Users
                                            .All()
                                            .Include(u => u.Following)
                                            .First(x => x.Id == this.CurrentUserId)
                                            .Following
                                            .Any(u => u.UserName == user.UserName);

            return this.View(user);
        }

        public ActionResult GetFollowers(string username)
        {
            var users = this.Data.Users
                .All()
                .Where(u => u.Following.Any(f => f.UserName == username))
                .Project()
                .To<UserViewModel>();

            return this.PartialView("_UsersList", users);
        }

        public ActionResult GetFollowing(string username)
        {
            var users = this.Data.Users
                .All()
                .Where(u => u.Followers.Any(f => f.UserName == username))
                .Project()
                .To<UserViewModel>();

            return this.PartialView("_UsersList", users);
        }

        [HttpPost]
        [Authorize]
        public ActionResult FollowUser(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return this.HttpNotFound();
            }

            this.CurrentUser.Following.Add(user);
            this.AddNewFollowerNotification(user, this.CurrentUser.UserName);
            this.Data.SaveChanges();
            NotificationsHub.OnNotificationAdded(user.Id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UnfollowUser(string username)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return this.HttpNotFound();
            }

            this.CurrentUser.Following.Remove(user);
            this.Data.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void AddNewFollowerNotification(User followedUser, string follower)
        {
            var notification = new Notification
            {
                Text = follower + " followed you.",
                NotificationTime = DateTime.Now,
                Type = NotificationType.NewFollower,
                IsRead = false,
                UserId = followedUser.Id
            };

            this.Data.Notifications.Add(notification);
        }
    }
}
