namespace NDDDSample.Interfaces.Bookings.Facade.Internal
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Application;
    using Assembler;
    using Domain.Model.Cargos;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;
    using Dto;
    using Infrastructure.Log;

    #endregion

    /// <summary>
    /// This implementation has additional support from the infrastructure, for exposing as an 
    /// Remote Facade that could be exposed as WCF service, Web Service or Romoting
    /// service and for keeping the OR-mapper unit-of-work open during DTO assembly,
    /// analogous to the view rendering for web interfaces.
    /// </summary>
    public class BookingServiceFacadeImpl : BookingServiceFacade
    {
        private readonly ILog logger = LogFactory.GetInterfaceLayerLogger();
        private IBookingService bookingService;
        private ICargoRepository cargoRepository;
        private ILocationRepository locationRepository;
        private IVoyageRepository voyageRepository;

        #region BookingServiceFacade Members

        public IList<LocationDTO> ListShippingLocations()
        {
            IList<Location> allLocations = locationRepository.FindAll();
            var assembler = new LocationDTOAssembler();
            return assembler.ToDTOList(allLocations);
        }

        public string BookNewCargo(string origin, string destination, DateTime arrivalDeadline)
        {
            TrackingId trackingId = bookingService.BookNewCargo(
                new UnLocode(origin),
                new UnLocode(destination),
                arrivalDeadline
                );
            return trackingId.IdString;
        }


        public CargoRoutingDTO LoadCargoForRouting(string trackingId)
        {
            Cargo cargo = cargoRepository.Find(new TrackingId(trackingId));
            var assembler = new CargoRoutingDTOAssembler();
            return assembler.ToDTO(cargo);
        }

        public void AssignCargoToRoute(string trackingIdStr, RouteCandidateDTO routeCandidateDTO)
        {
            Itinerary itinerary = new ItineraryCandidateDTOAssembler().FromDTO(routeCandidateDTO, voyageRepository,
                                                                               locationRepository);
            TrackingId trackingId = new TrackingId(trackingIdStr);

            bookingService.AssignCargoToRoute(itinerary, trackingId);
        }


        public void ChangeDestination(string trackingId, string destinationUnLocode)
        {
            bookingService.ChangeDestination(new TrackingId(trackingId), new UnLocode(destinationUnLocode));
        }


        public IList<CargoRoutingDTO> ListAllCargos()
        {
            IList<Cargo> cargoList = cargoRepository.FindAll();
            List<CargoRoutingDTO> dtoList = new List<CargoRoutingDTO>(cargoList.Count);
            CargoRoutingDTOAssembler assembler = new CargoRoutingDTOAssembler();
            foreach (Cargo cargo in cargoList)
            {
                dtoList.Add(assembler.ToDTO(cargo));
            }
            return dtoList;
        }


        public IList<RouteCandidateDTO> RequestPossibleRoutesForCargo(string trackingId)
        {
            IList<Itinerary> itineraries = bookingService.RequestPossibleRoutesForCargo(new TrackingId(trackingId));

            var routeCandidates = new List<RouteCandidateDTO>(itineraries.Count);
            var dtoAssembler = new ItineraryCandidateDTOAssembler();

            foreach (Itinerary itinerary in itineraries)
            {
                routeCandidates.Add(dtoAssembler.ToDTO(itinerary));
            }

            return routeCandidates;
        }

        #endregion

        public void SetBookingService(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        public void SetLocationRepository(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        public void SetCargoRepository(ICargoRepository cargoRepository)
        {
            this.cargoRepository = cargoRepository;
        }

        public void SetVoyageRepository(IVoyageRepository voyageRepository)
        {
            this.voyageRepository = voyageRepository;
        }
    }
}