namespace NDDDSample.Web.Controllers
{
    #region Usings

    using System;
    using System.Web.Mvc;

    #endregion

    [HandleError]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {         
            return View();
        }

        protected override string GetPageTitle()
        {
            return "Welcome to NDDDSample Application!";
        }
    }
}