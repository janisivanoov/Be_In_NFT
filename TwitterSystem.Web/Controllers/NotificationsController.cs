namespace TwitterSystem.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Data.UowData;
    using EntityFramework.Extensions;
    using TwitterSystem.Models;
    using ViewModels.Notifications;

    [Authorize]
    public class NotificationsController : BaseController
    {
        private const int NotificationsPerPage = 10;

        public NotificationsController(ITwitterData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var notifications = this.Data.Notifications
                 .All()
                 .Where(n => !n.IsRead && n.UserId == this.CurrentUserId)
                 .OrderByDescending(n => n.NotificationTime)
                 .Take(NotificationsPerPage);

            var notificationsHomeViewModel = new NotificationsHomeViewModel
            {
                Notifications = notifications.Project().To<NotificationViewModel>().ToList()
            };

            notifications.Update(n => new Notification { IsRead = true });

            return this.View(notificationsHomeViewModel);
        }

        [ChildActionOnly]
        public ActionResult NotificationsCount()
        {
            var count = this.Data.Notifications
                .All()
                .Count(n => !n.IsRead && n.UserId == this.CurrentUserId);

            return this.Content(count.ToString());
        }
    }
}