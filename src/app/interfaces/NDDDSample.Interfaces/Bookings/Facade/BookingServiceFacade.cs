namespace NDDDSample.Interfaces.Bookings.Facade
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Dto;

    #endregion

    /// <summary>
    /// This facade shields the domain layer - model, services, repositories -
    /// from concerns about such things as the user interface and remoting.
    /// </summary>
    public interface BookingServiceFacade
    {
        string BookNewCargo(string origin, string destination, DateTime arrivalDeadline);

        CargoRoutingDTO LoadCargoForRouting(string trackingId);

        void AssignCargoToRoute(string trackingId, RouteCandidateDTO route);

        void ChangeDestination(string trackingId, string destinationUnLocode);

        IList<RouteCandidateDTO> RequestPossibleRoutesForCargo(string trackingId);

        IList<LocationDTO> ListShippingLocations();

        IList<CargoRoutingDTO> ListAllCargos();
    }
}