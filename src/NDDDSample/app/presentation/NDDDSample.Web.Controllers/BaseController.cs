namespace NDDDSample.Web.Controllers
{
    #region Usings

    using System.Web.Mvc;

    #endregion

    public abstract class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewData["Title"] = GetPageTitle();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            this.View("Error", new ErrorMessageViewModel(filterContext.Exception.Message)).ExecuteResult(this.ControllerContext);
        }

        public virtual void SetMessage(string msgId)
        {
            TempData["Message"] = MessageSource.GetMessage(msgId);
        }

        protected abstract string GetPageTitle();
    }
}