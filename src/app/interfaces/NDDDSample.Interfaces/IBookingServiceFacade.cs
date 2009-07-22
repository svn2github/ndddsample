namespace NDDDSample.Interfaces.BookingRemoteService.Common
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Dto;

    /// <summary>
    /// This facade shields the domain layer - model, services, repositories -
    /// from concerns about such things as the user interface and remoting.
    /// </summary>
    [ServiceContract]
    public interface IBookingServiceFacade
    {
        [OperationContract]
        string BookNewCargo(string origin, string destination, DateTime arrivalDeadline);

        [OperationContract]
        CargoRoutingDTO LoadCargoForRouting(string trackingId);

        [OperationContract]
        void AssignCargoToRoute(string trackingId, RouteCandidateDTO route);

        [OperationContract]
        void ChangeDestination(string trackingId, string destinationUnLocode);

        [OperationContract]
        IList<RouteCandidateDTO> RequestPossibleRoutesForCargo(string trackingId);

        [OperationContract]
        IList<LocationDTO> ListShippingLocations();

        [OperationContract]
        IList<CargoRoutingDTO> ListAllCargos();
    }
}