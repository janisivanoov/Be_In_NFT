namespace TwitterSystem.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<User> followers;
        private ICollection<User> following;
        private ICollection<Tweet> tweets;
        private ICollection<Tweet> favouriteTweets;
        private ICollection<Message> sentMessages;
        private ICollection<Message> receivedMessages;
        private ICollection<Report> reportsSent;
        private ICollection<Notification> notifications;

        public User()
        {
            this.followers = new HashSet<User>();
            this.following = new HashSet<User>();
            this.tweets = new HashSet<Tweet>();
            this.favouriteTweets = new HashSet<Tweet>();
            this.sentMessages = new HashSet<Message>();
            this.receivedMessages = new HashSet<Message>();
            this.reportsSent = new HashSet<Report>();
            this.notifications = new HashSet<Notification>();
        }

        public string FullName { get; set; }

        public virtual ICollection<User> Followers
        {
            get { return this.followers; }
            set { this.followers = value; }
        }

        public virtual ICollection<User> Following
        {
            get { return this.following; }
            set { this.following = value; }
        }

        public virtual ICollection<Tweet> Tweets
        {
            get { return this.tweets; }
            set { this.tweets = value; }
        }

        public virtual ICollection<Tweet> FavouriteTweets
        {
            get { return this.favouriteTweets; }
            set { this.favouriteTweets = value; }
        }

        public virtual ICollection<Message> SentMessages
        {
            get { return this.sentMessages; }
            set { this.sentMessages = value; }
        }

        public virtual ICollection<Message> ReceivedMessages
        {
            get { return this.receivedMessages; }
            set { this.receivedMessages = value; }
        }

        public virtual ICollection<Report> ReportsSent
        {
            get { return this.reportsSent; }
            set { this.reportsSent = value; }
        }

        public virtual ICollection<Notification> Notifications
        {
            get { return this.notifications; }
            set { this.notifications = value; }
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
       
            return userIdentity;
        }
    }
}
