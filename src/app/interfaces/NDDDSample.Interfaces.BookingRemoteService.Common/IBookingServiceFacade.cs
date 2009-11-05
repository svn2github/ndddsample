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
        [OperationContract, FaultContract(typeof (NDDDRemoteException))]        
        string BookNewCargo(string origin, string destination, DateTime arrivalDeadline);

        [OperationContract, FaultContract(typeof (NDDDRemoteException))]        
        CargoRoutingDTO LoadCargoForRouting(string trackingId);

        [OperationContract, FaultContract(typeof (NDDDRemoteException))]        
        void AssignCargoToRoute(string trackingId, RouteCandidateDTO route);

        [OperationContract, FaultContract(typeof (NDDDRemoteException))]        
        void ChangeDestination(string trackingId, string destinationUnLocode);

        [OperationContract, FaultContract(typeof (NDDDRemoteException))]        
        IList<RouteCandidateDTO> RequestPossibleRoutesForCargo(string trackingId);

        [OperationContract, FaultContract(typeof (NDDDRemoteException))]        
        IList<LocationDTO> ListShippingLocations();

        [OperationContract, FaultContract(typeof (NDDDRemoteException))]        
        IList<CargoRoutingDTO> ListAllCargos();
    }
}