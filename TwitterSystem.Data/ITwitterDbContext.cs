namespace TwitterSystem.Data
{
    using System.Data.Entity;
   
    using Models;

    public interface ITwitterDbContext
    {
        IDbSet<User> Users { get; }

        IDbSet<Tweet> Tweets { get; }

        IDbSet<Message> Messages { get; }

        IDbSet<Report> Reports { get; }

        IDbSet<Notification> Notifications { get; }

        int SaveChanges();
    }
}
