namespace TwitterSystem.Web.Hubs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    [HubName("tweets")]
    public class TweetsHub : Hub
    {
        public static void OnTweetAdded(int id, IList<string> followerIds)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TweetsHub>();
            context.Clients.All.tweetAdded(id);
            context.Clients.Groups(followerIds).friendTweeted(id);
        }

        public override Task OnConnected()
        {
            var id = this.Context.User.Identity.GetUserId();
            if (id != null)
            {
                this.Groups.Add(this.Context.ConnectionId, id);
            }
            
            return base.OnConnected();
        }
    }
}
