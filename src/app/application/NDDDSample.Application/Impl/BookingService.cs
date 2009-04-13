namespace NDDDSample.Application.Impl
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Transactions;
    using Domain.Model.Cargos;
    using Domain.Model.Locations;
    using Domain.Service;
    using Infrastructure.Log;

    #endregion

    public class BookingService : IBookingService
    {
        private readonly ICargoRepository cargoRepository;
        private readonly ILocationRepository locationRepository;
        private readonly ILog logger = LogFactory.GetApplicationLayer();
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
            using (var transactionScope = new TransactionScope())
            {
                // TODO modeling this as a cargo factory might be suitable
                TrackingId trackingId = cargoRepository.NextTrackingId();
                Location origin = locationRepository.Find(originUnLocode);
                Location destination = locationRepository.Find(destinationUnLocode);
                var routeSpecification = new RouteSpecification(origin, destination, arrivalDeadline);

                Cargo cargo = new Cargo(trackingId, routeSpecification);

                cargoRepository.Store(cargo);
                logger.Info("Booked new cargo with tracking id " + cargo.TrackingId.IdString);

                transactionScope.Complete();
                return cargo.TrackingId;
            }
        }

        public IList<Itinerary> RequestPossibleRoutesForCargo(TrackingId trackingId)
        {
            using (var transactionScope = new TransactionScope())
            {
                Cargo cargo = cargoRepository.Find(trackingId);

                if (cargo == null)
                {
                    return new List<Itinerary>();
                }

                transactionScope.Complete();
                return routingService.FetchRoutesForSpecification(cargo.RouteSpecification);
            }
        }


        public void AssignCargoToRoute(Itinerary itinerary, TrackingId trackingId)
        {
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