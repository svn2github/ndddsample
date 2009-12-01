namespace NDDDSample.Web.Controllers.Tracking
{
    #region Usings

    using System.Collections.Specialized;
    using System.Web.Mvc;

    #endregion

    public class TrackCommandBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection collection = controllerContext.HttpContext.Request.Form;

            var command = new TrackCommand();
            command.TrackingId = collection["trackingId"];

            return command;
        }
    }
}