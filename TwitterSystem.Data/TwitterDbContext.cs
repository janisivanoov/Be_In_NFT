namespace TwitterSystem.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public class TwitterDbContext : IdentityDbContext<User>, ITwitterDbContext
    {
        public TwitterDbContext()
            : base("TwitterConnection", false)
        {
        }

        public IDbSet<Tweet> Tweets { get; set; }

        public IDbSet<Message> Messages { get; set; }

        public IDbSet<Report> Reports { get; set; }

        public IDbSet<Notification> Notifications { get; set; }


        public static TwitterDbContext Create()
        {
            return new TwitterDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //  remove all CASCADE DELETES
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<User>().HasMany(u => u.Followers).WithMany(u => u.Following)
               .Map(u => u.MapLeftKey("UserId").MapRightKey("FollowerId").ToTable("UsersFollowers"));
            modelBuilder.Entity<User>().HasMany(u => u.FavouriteTweets).WithMany(t => t.FavouredBy)
                .Map(u => u.MapLeftKey("UserId").MapRightKey("FavouriteTweetId").ToTable("UsersFavouriteTweets"));
            modelBuilder.Entity<User>().HasMany(u => u.SentMessages).WithRequired(m => m.Sender);
            modelBuilder.Entity<User>().HasMany(u => u.ReceivedMessages).WithRequired(m => m.Recipient);
            modelBuilder.Entity<Tweet>().HasMany(t => t.Replies).WithOptional(t => t.RepliedToTweet);
            modelBuilder.Entity<Tweet>().HasMany(t => t.Retweets).WithOptional(t => t.RetweetedFrom);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
