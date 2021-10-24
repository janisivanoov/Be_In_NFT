namespace TwitterSystem.Web.Hubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    [HubName("notifications")]
    public class NotificationsHub : Hub
    {
        public static void OnNotificationAdded(string userId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            context.Clients.Group(userId).notificationAdded();
        }

        public override Task OnConnected()
        {
            var id = this.Context.User.Identity.GetUserId();
            this.Groups.Add(this.Context.ConnectionId, id);
            return base.OnConnected();
        }
    }
}
