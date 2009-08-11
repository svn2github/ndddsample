namespace NDDDSample.Web.Controllers.CargoAdmin
{
    #region Usings

    using System.Collections.Specialized;
    using System.Web.Mvc;

    #endregion

    public class RegistrationCommandBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection collection = controllerContext.HttpContext.Request.Form;

            var command = new RegistrationCommand(collection["originUnlocode"],
                                                  collection["destinationUnlocode"], collection["arrivalDeadline"]);

            return command;
        }
    }
}