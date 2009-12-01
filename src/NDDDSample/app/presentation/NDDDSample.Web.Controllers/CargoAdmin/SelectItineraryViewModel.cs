namespace NDDDSample.Web.Controllers.CargoAdmin
{
    #region Usings

    using System.Collections.Generic;
    using Interfaces.BookingRemoteService.Common.Dto;

    #endregion

    /// <summary>
    /// The class provide a strongly typed result for Select Itenerary action 
    /// of the Cargo Admin Controller. The class incapsulates two classes and
    /// is used to provide a strongly typed result for the Select Itinerary View.
    /// </summary>
    public class SelectItineraryViewModel
    {
        private readonly IList<RouteCandidateDTO> routeCandidates;
        private readonly CargoRoutingDTO cargo;

        public SelectItineraryViewModel(IList<RouteCandidateDTO> routeCandidatesDto, CargoRoutingDTO cargoDto)
        {
            cargo = cargoDto;
            routeCandidates = routeCandidatesDto;
        }

        public IList<RouteCandidateDTO> RouteCandidates
        {
            get { return routeCandidates; }
        }

        public CargoRoutingDTO Cargo
        {
            get { return cargo; }
        }
    }
}