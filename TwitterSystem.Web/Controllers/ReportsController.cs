namespace TwitterSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Data.UowData;
    using InputModels;
    using TwitterSystem.Models;
    using ViewModels.Alerts;

    public class ReportsController : BaseController
    {
        public ReportsController(ITwitterData data)
            : base(data)
        {
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddReport(ReportInputModel reportModel)
        {
            if (!this.Data.Tweets.All().Any(t => t.Id == reportModel.TweetId))
            {
                this.AddAlert("Invalid tweet", AlertType.Error);
            }
            else if (this.ModelState.IsValid)
            {
                var report = new Report
                {
                    Text = reportModel.Text,
                    TweetId = reportModel.TweetId,
                    UserId = this.CurrentUserId,
                    ReportedAt = DateTime.Now
                };

                this.Data.Reports.Add(report);
                this.Data.SaveChanges();
                this.AddAlert("Tweet reported successfully", AlertType.Info);
            }
            else
            {
                foreach (var error in this.ModelState.Values.SelectMany(m => m.Errors))
                {
                    this.AddAlert(error.ErrorMessage, AlertType.Error);
                }
            }

            var urlReferrer = this.Request.UrlReferrer;
            if (urlReferrer != null)
            {
                return this.Redirect(urlReferrer.AbsoluteUri);
            }

            return this.RedirectToAction("Index", "User");
        }
    }
}
