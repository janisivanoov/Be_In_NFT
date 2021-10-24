namespace TwitterSystem.Web.InputModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class ReportInputModel
    {
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DisplayName("Report")]
        public string Text { get; set; }

        [Required]
        public int TweetId { get; set; }
    }
}
