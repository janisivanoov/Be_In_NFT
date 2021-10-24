namespace TwitterSystem.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data.UowData;

    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;

    using Model = TwitterSystem.Models.Tweet;
    using ViewModel = ViewModel.TweetsDetails.TweetsDetailsViewModel;

    [Authorize(Roles = "Administrator")]
    public class TweetsDetailsController : AdminController
    {
        public TweetsDetailsController(ITwitterData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = this.Data.Tweets
                            .All()
                            .Project()
                            .To<ViewModel>();

            return this.Json(data.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, ViewModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var tweet = Mapper.Map<Model>(model);
                this.Data.Tweets.Add(tweet);
                this.Data.SaveChanges();
            }

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, ViewModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var tweet = this.Data.Tweets.Find(model.Id);
                tweet.Text = model.Text;
                this.Data.SaveChanges();
            }

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, ViewModel model)
        {
            this.Data.Tweets.Remove(model.Id);
            this.Data.SaveChanges();

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }
    }
}