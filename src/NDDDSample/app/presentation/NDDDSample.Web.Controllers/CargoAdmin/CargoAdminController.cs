namespace NDDDSample.Web.Controllers.CargoAdmin
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
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
    public class CargoAdminController : BaseController
    {
        public const string RegisterDateFormat = "M/dd/yyyy";
        public const string ShowActionName = "Show";

        private readonly IBookingServiceFacade BookingServiceFacade;

        public CargoAdminController(IBookingServiceFacade bookingServiceFacade)
        {
            BookingServiceFacade = bookingServiceFacade;
        }

        public ActionResult RegistrationForm()
        {         
            IList<LocationDTO> dtoList = BookingServiceFacade.ListShippingLocations();

            var unLocodeStrings = new List<string>();
            unLocodeStrings.AddRange(
                dtoList.Select(code => code.UnLocode)
                );

            return View(new RegistrationFormViewModel(dtoList, unLocodeStrings));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(
            [ModelBinder(typeof (RegistrationCommandBinder))] RegistrationCommand registrationCommand)
        {
            DateTime arrivalDeadlineDateTime = DateTime.ParseExact(registrationCommand.ArrivalDeadline, RegisterDateFormat,
                                                                   CultureInfo.InvariantCulture);

            string trackingId = BookingServiceFacade.BookNewCargo(
                registrationCommand.OriginUnlocode, registrationCommand.DestinationUnlocode, arrivalDeadlineDateTime
                );

            return RedirectToAction(ShowActionName, new RouteValueDictionary(new {trackingId}));
        }

        

        public ActionResult List()
        {         
            IList<CargoRoutingDTO> cargoList = BookingServiceFacade.ListAllCargos();

            return View(cargoList);
        }

        public ActionResult Show(string trackingId)
        {         
            CargoRoutingDTO dto = BookingServiceFacade.LoadCargoForRouting(trackingId);
            return View(dto);
        }

        public ActionResult SelectItinerary(string trackingId)
        {         
            IList<RouteCandidateDTO> routeCandidatesDto = BookingServiceFacade.RequestPossibleRoutesForCargo(trackingId);
            CargoRoutingDTO cargoDto = BookingServiceFacade.LoadCargoForRouting(trackingId);

            var selectItineraryResult = new SelectItineraryViewModel(routeCandidatesDto, cargoDto);

            return View(selectItineraryResult);
        }

        public ActionResult AssignItinerary(string trackingId, IList<LegCommand> legCommands)
        {           
            var legDTOs = new List<LegDTO>(legCommands.Count);

            legDTOs.AddRange(
                legCommands.Select(
                    leg => new LegDTO(
                               leg.VoyageNumber,
                               leg.FromUnLocode,
                               leg.ToUnLocode,
                               leg.FromDate,
                               leg.ToDate
                               ))
                );

            var selectedRoute = new RouteCandidateDTO(legDTOs);

            BookingServiceFacade.AssignCargoToRoute(trackingId, selectedRoute);

            return RedirectToAction("Show", new RouteValueDictionary(new {trackingId}));
        }

        public ActionResult PickNewDestination(string trackingId)
        {           
            IList<LocationDTO> locations = BookingServiceFacade.ListShippingLocations();
            CargoRoutingDTO cargo = BookingServiceFacade.LoadCargoForRouting(trackingId);

            return View(new PickNewDestinationViewModel(locations, cargo));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeDestination(string trackingId, string unLocode)
        {           
            BookingServiceFacade.ChangeDestination(trackingId, unLocode);
            return RedirectToAction("Show", new RouteValueDictionary(new {trackingId}));
        }
        
        protected override string GetPageTitle()
        {            
            return "Cargo Administration";
        }

        public virtual new ActionResult RedirectToAction(string actionName, RouteValueDictionary routeValueDictionary)
        {
           return base.RedirectToAction(actionName, routeValueDictionary);
        }
    }
}