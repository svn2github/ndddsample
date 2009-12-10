namespace NDDDSample.Web.Controllers.Tracking
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Interfaces.BookingRemoteService.Common;
    using Interfaces.BookingRemoteService.Common.Dto;

    #endregion

    /// <summary>
    /// Cargo Tracking controller manage search requests for
    /// a list of events that were heppened for the specific Cargo,
    /// identified by Cargo TrackingId
    /// </summary>
    public class CargoTrackingController : BaseController
    {
        public const string SearchViewName = "Search";
        public const string UnknownMessageId = "cargo.unknown_id";

        private readonly ICargoRepository CargoRepository;
        private readonly IHandlingEventRepository HandlingEventRepository;        

        public CargoTrackingController(ICargoRepository cargoRepository,
                                       IHandlingEventRepository handlingEventRepository)
        {
            this.CargoRepository = cargoRepository;
            this.HandlingEventRepository = handlingEventRepository;            
        }

        public ActionResult Index()
        {
            return View(SearchViewName, null);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search([ModelBinder(typeof (TrackCommandBinder))] TrackCommand trackCommand)
        {
            string trackingIdString = trackCommand.TrackingId;

            var trackingId = new TrackingId(trackingIdString);
            Cargo cargo = CargoRepository.Find(trackingId);

            CargoTrackingViewAdapter cargoTrackingViewAdapter = null;

            if (cargo != null)
            {
                IList<HandlingEvent> handlingEvents =
                    HandlingEventRepository.LookupHandlingHistoryOfCargo(trackingId)
                    .DistinctEventsByCompletionTime();               
                cargoTrackingViewAdapter = new CargoTrackingViewAdapter(cargo, handlingEvents);
            }
            else
            {
                SetMessage(UnknownMessageId);
            }

            return View(cargoTrackingViewAdapter);
        }      

        protected override string GetPageTitle()
        {
            return "Cargo Tracking";
        }
    }
}