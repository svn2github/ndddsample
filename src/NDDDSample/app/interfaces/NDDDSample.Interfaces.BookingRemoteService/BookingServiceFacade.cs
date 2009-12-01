namespace NDDDSample.Interfaces.BookingRemoteService
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Application;
    using Assembler;
    using Common;
    using Common.Dto;
    using Domain.Model.Cargos;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;
    using Infrastructure.Log;

    #endregion

    /// <summary>
    /// This implementation has additional support from the infrastructure, for exposing as an 
    /// Remote Facade that could be exposed as WCF service, Web Service or Romoting
    /// service and for keeping the OR-mapper unit-of-work open during DTO assembly,
    /// analogous to the view rendering for web interfaces.
    /// </summary>
    public class BookingServiceFacade : IBookingServiceFacade
    {
        private readonly ILog logger = LogFactory.GetInterfaceLayerLogger();
        private readonly IBookingService BookingService;
        private readonly ICargoRepository CargoRepository;
        private readonly ILocationRepository LocationRepository;
        private readonly IVoyageRepository VoyageRepository;

        public BookingServiceFacade(IBookingService bookingService,
                                    ICargoRepository cargoRepository,
                                    ILocationRepository locationRepository,
                                    IVoyageRepository voyageRepository)
        {
            BookingService = bookingService;
            CargoRepository = cargoRepository;
            LocationRepository = locationRepository;
            VoyageRepository = voyageRepository;
        }

        #region IBookingServiceFacade Members

        public IList<LocationDTO> ListShippingLocations()
        {
            try
            {
                IList<Location> allLocations = LocationRepository.FindAll();
                var assembler = new LocationDTOAssembler();
                return assembler.ToDTOList(allLocations);
            }
            catch (Exception exception)
            {
                throw new NDDDRemoteBookingException(exception.Message);
            }
        }

        public string BookNewCargo(string origin, string destination, DateTime arrivalDeadline)
        {
            try
            {
                TrackingId trackingId = BookingService.BookNewCargo(
                    new UnLocode(origin),
                    new UnLocode(destination),
                    arrivalDeadline
                    );
                return trackingId.IdString;
            }
            catch (Exception exception)
            {
                throw new NDDDRemoteBookingException(exception.Message);
            }
        }

        public CargoRoutingDTO LoadCargoForRouting(string trackingId)
        {
            try
            {
                Cargo cargo = CargoRepository.Find(new TrackingId(trackingId));
                var assembler = new CargoRoutingDTOAssembler();
                return assembler.ToDTO(cargo);
            }
            catch (Exception exception)
            {
                throw new NDDDRemoteBookingException(exception.Message);
            }
        }

        public void AssignCargoToRoute(string trackingIdStr, RouteCandidateDTO routeCandidateDTO)
        {
            try
            {
                Itinerary itinerary = new ItineraryCandidateDTOAssembler().FromDTO(routeCandidateDTO, VoyageRepository,
                                                                                   LocationRepository);
                var trackingId = new TrackingId(trackingIdStr);

                BookingService.AssignCargoToRoute(itinerary, trackingId);
            }
            catch (Exception exception)
            {
                throw new NDDDRemoteBookingException(exception.Message);
            }
        }


        public void ChangeDestination(string trackingId, string destinationUnLocode)
        {
            try
            {
                BookingService.ChangeDestination(new TrackingId(trackingId), new UnLocode(destinationUnLocode));
            }
            catch (Exception exception)
            {
                throw new NDDDRemoteBookingException(exception.Message);
            }
        }


        public IList<CargoRoutingDTO> ListAllCargos()
        {
            try
            {
                IList<Cargo> cargoList = CargoRepository.FindAll();
                var dtoList = new List<CargoRoutingDTO>(cargoList.Count);
                var assembler = new CargoRoutingDTOAssembler();
                foreach (Cargo cargo in cargoList)
                {
                    dtoList.Add(assembler.ToDTO(cargo));
                }
                return dtoList;
            }
            catch (Exception exception)
            {
                throw new NDDDRemoteBookingException(exception.Message);
            }
        }


        public IList<RouteCandidateDTO> RequestPossibleRoutesForCargo(string trackingId)
        {
            try
            {
                var itineraries = BookingService.RequestPossibleRoutesForCargo(new TrackingId(trackingId));

                var routeCandidates = new List<RouteCandidateDTO>(itineraries.Count);
                var dtoAssembler = new ItineraryCandidateDTOAssembler();

                foreach (Itinerary itinerary in itineraries)
                {
                    routeCandidates.Add(dtoAssembler.ToDTO(itinerary));
                }

                return routeCandidates;
            }
            catch (Exception exception)
            {
                throw new NDDDRemoteBookingException(exception.Message);
            }
        }

        #endregion
    }
}