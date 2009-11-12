namespace NDDDSample.Interfaces.BookingRemoteService.Common
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Dto;

    #endregion

    /// <summary>
    /// This facade shields the domain layer - model, services, repositories -
    /// from concerns about such things as the user interface and remoting.
    /// </summary>
    [ServiceContract]
    public interface IBookingServiceFacade
    {
        [OperationContract, FaultContract(typeof (NDDDRemoteBookingException))]        
        string BookNewCargo(string origin, string destination, DateTime arrivalDeadline);

        [OperationContract, FaultContract(typeof (NDDDRemoteBookingException))]        
        CargoRoutingDTO LoadCargoForRouting(string trackingId);

        [OperationContract, FaultContract(typeof (NDDDRemoteBookingException))]        
        void AssignCargoToRoute(string trackingId, RouteCandidateDTO route);

        [OperationContract, FaultContract(typeof (NDDDRemoteBookingException))]        
        void ChangeDestination(string trackingId, string destinationUnLocode);

        [OperationContract, FaultContract(typeof (NDDDRemoteBookingException))]        
        IList<RouteCandidateDTO> RequestPossibleRoutesForCargo(string trackingId);

        [OperationContract, FaultContract(typeof (NDDDRemoteBookingException))]        
        IList<LocationDTO> ListShippingLocations();

        [OperationContract, FaultContract(typeof (NDDDRemoteBookingException))]        
        IList<CargoRoutingDTO> ListAllCargos();
    }
}