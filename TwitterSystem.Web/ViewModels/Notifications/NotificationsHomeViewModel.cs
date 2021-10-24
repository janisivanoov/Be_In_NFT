namespace TwitterSystem.Web.ViewModels.Notifications
{
    using System.Collections.Generic;

    public class NotificationsHomeViewModel
    {
        public IEnumerable<NotificationViewModel> Notifications { get; set; }
    }
}