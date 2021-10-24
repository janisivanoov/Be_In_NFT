namespace TwitterSystem.Data.UowData
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity;

    using Models;
    using Repository;

    public interface ITwitterData
    {
        DbContext Context { get; }

        IRepository<User> Users { get; }

        IRepository<Tweet> Tweets { get; }

        IRepository<Message> Messages { get; }

        IRepository<Notification> Notifications { get; }

        IRepository<Report> Reports { get; }

        IUserStore<User> UserStore { get; }

        void SaveChanges();
    }
}
