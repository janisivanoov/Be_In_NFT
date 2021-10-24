namespace TwitterSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Data.UowData;
    using Microsoft.AspNet.Identity;
    using TwitterSystem.Models;
    using ViewModels.Alerts;

    [HandleError]
    public abstract class BaseController : Controller
    {
        private User currentUser;

        protected BaseController(ITwitterData data)
        {
            this.Data = data;
        }

        protected ITwitterData Data { get; private set; }

        protected string CurrentUserId
        {
            get { return this.User.Identity.GetUserId(); }
        }

        protected User CurrentUser
        {
            get { return this.currentUser ?? (this.currentUser = this.Data.Users.Find(this.CurrentUserId)); }
        }

        protected void AddAlert(string message, AlertType type)
        {
            if (this.TempData["alerts"] == null)
            {
                this.TempData["alerts"] = new HashSet<AlertViewModel>();
            }

            ((HashSet<AlertViewModel>)this.TempData["alerts"]).Add(new AlertViewModel(message, type));
        }
    }
}