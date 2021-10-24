namespace TwitterSystem.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;
    using Data.UowData;
    using Web.Controllers;

    [Authorize(Roles = "Administrator")]
    public abstract class AdminController : BaseController
    {
        public AdminController(ITwitterData data)
            : base(data)
        {
        }
    }
}