namespace NDDDSample.Web.Controllers.CargoAdmin
{
    #region Usings

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Interfaces.BookingRemoteService.Common;
    using Interfaces.BookingRemoteService.Common.Dto;

    #endregion

    /// <summary>
    ///  Handles cargo booking and routing. Operates against a dedicated remoting service facade,
    /// and could easily be rewritten as a thick Swing client. Completely separated from the domain layer,
    /// unlike the tracking user interface. 
    /// In order to successfully keep the domain model shielded from user interface considerations,
    /// this approach is generally preferred to the one taken in the tracking controller. However,
    /// there is never any one perfect solution for all situations, so we've chosen to demonstrate
    /// two polarized ways to build user interfaces.
    /// </summary>
    public class CargoAdminController : Controller
    {
        private readonly IBookingServiceFacade BookingServiceFacade;

        public CargoAdminController(IBookingServiceFacade bookingServiceFacade)
        {
            BookingServiceFacade = bookingServiceFacade;
        }

        public ActionResult List()
        {
            SetPageTitle();
            IList<CargoRoutingDTO> cargoList = BookingServiceFacade.ListAllCargos();

            return View(cargoList);
        }


        private void SetPageTitle()
        {
            ViewData["Title"] = "Cargo Administration";
;
        }
    }
}