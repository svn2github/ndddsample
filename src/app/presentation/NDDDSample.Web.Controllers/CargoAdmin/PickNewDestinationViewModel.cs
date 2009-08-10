namespace NDDDSample.Web.Controllers.CargoAdmin
{
    #region Usings

    using System.Collections.Generic;
    using Interfaces.BookingRemoteService.Common.Dto;

    #endregion

    /// <summary>
    /// The class provide a strongly typed result for PickNewDestination action 
    /// of the Cargo Admin Controller. The class incapsulates two classes and
    /// is used to provide a strongly typed result for the Pick New Destination View.
    /// </summary>
    public class PickNewDestinationViewModel
    {
        private readonly IList<LocationDTO> locations;
        private readonly CargoRoutingDTO cargo;

        public PickNewDestinationViewModel(IList<LocationDTO> locations, CargoRoutingDTO cargo)
        {
            this.locations = locations;
            this.cargo = cargo;
        }

        public IList<LocationDTO> Locations
        {
            get { return locations; }
        }

        public CargoRoutingDTO Cargo
        {
            get { return cargo; }
        }
    }
}