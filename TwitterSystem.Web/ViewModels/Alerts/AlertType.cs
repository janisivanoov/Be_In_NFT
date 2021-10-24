namespace TwitterSystem.Web.ViewModels.Alerts
{
    using System.ComponentModel;

    public enum AlertType
    {
        [Description("success")]
        Success,
        [Description("info")]
        Info,
        [Description("danger")]
        Error
    }
}