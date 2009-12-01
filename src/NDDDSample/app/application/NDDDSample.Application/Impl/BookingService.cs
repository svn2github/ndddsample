namespace NDDDSample.Application.Impl
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Transactions;
    using Domain.Model.Cargos;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;
    using Domain.Service;
    using Infrastructure;
    using Infrastructure.Log;

    #endregion

    public class BookingService : IBookingService
    {
        private readonly ICargoRepository cargoRepository;
        private readonly ILocationRepository locationRepository;
        private readonly ILog logger = LogFactory.GetApplicationLayerLogger();
        private readonly IRoutingService routingService;

        public BookingService(ICargoRepository cargoRepository,
                              ILocationRepository locationRepository,
                              IRoutingService routingService)
        {
            this.cargoRepository = cargoRepository;
            this.locationRepository = locationRepository;
            this.routingService = routingService;
        }

        #region IBookingService Members

        public TrackingId BookNewCargo(UnLocode originUnLocode,
                                       UnLocode destinationUnLocode,
                                       DateTime arrivalDeadline)
        {
            //TODO: Revise transaciton and UoW logic
            using (var transactionScope = new TransactionScope())
            {
                TrackingId trackingId = cargoRepository.NextTrackingId();
                Location origin = locationRepository.Find(originUnLocode);
                Location destination = locationRepository.Find(destinationUnLocode);

                Cargo cargo = CargoFactory.NewCargo(trackingId, origin, destination, arrivalDeadline);

                cargoRepository.Store(cargo);
                logger.Info("Booked new cargo with tracking id " + cargo.TrackingId);

                transactionScope.Complete();
                return cargo.TrackingId;
            }
        }

        public IList<Itinerary> RequestPossibleRoutesForCargo(TrackingId trackingId)
        {
            //TODO: Revise transaciton and UoW logic
            using (var transactionScope = new TransactionScope())
            {
                Cargo cargo = cargoRepository.Find(trackingId);

                if (cargo == null)
                {
                    return new List<Itinerary>();
                }

                IList<Itinerary> routesForSpecification =
                    routingService.FetchRoutesForSpecification(cargo.RouteSpecification);
                transactionScope.Complete();
                return routesForSpecification;
            }
        }


        public void AssignCargoToRoute(Itinerary itinerary, TrackingId trackingId)
        {
            //TODO: Revise transaciton and UoW logic
            using (var transactionScope = new TransactionScope())
            {
                Cargo cargo = cargoRepository.Find(trackingId);
                if (cargo == null)
                {
                    throw new ArgumentException("Can't assign itinerary to non-existing cargo " + trackingId);
                }

                cargo.AssignToRoute(itinerary);
                cargoRepository.Store(cargo);

                transactionScope.Complete();
                logger.Info("Assigned cargo " + trackingId + " to new route");
            }
        }

        public void ChangeDestination(TrackingId trackingId, UnLocode unLocode)
        {
            //TODO: Revise transaciton and UoW logic
            using (var transactionScope = new TransactionScope())
            {
                Cargo cargo = cargoRepository.Find(trackingId);
                Location newDestination = locationRepository.Find(unLocode);

                RouteSpecification routeSpecification = new RouteSpecification(
                    cargo.Origin, newDestination, cargo.RouteSpecification.ArrivalDeadline
                    );
                cargo.SpecifyNewRoute(routeSpecification);

                cargoRepository.Store(cargo);
                transactionScope.Complete();
                logger.Info("Changed destination for cargo " + trackingId + " to " + routeSpecification.Destination);
            }
        }

        #endregion
    }    
}