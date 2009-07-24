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

    //TODO:Remove RoutingServiceTemporaryImpl class after real implementation of IRoutingService
    //TODO:remove IoC registration of the class
    public class RoutingServiceTemporaryImpl : IRoutingService
    {
        #region IRoutingService Members

        public IList<Itinerary> FetchRoutesForSpecification(RouteSpecification routeSpecification)
        {
            if (routeSpecification.Origin.Equals(SampleLocations.HONGKONG))
            {
                var nsLegs = new List<Leg>
                                     {
                                         new Leg(SampleVoyages.v100, SampleLocations.HONGKONG, SampleLocations.NEWYORK,
                                                 DateUtil.ToDate("2009-03-03"), DateUtil.ToDate("2009-03-09")),
                                         new Leg(SampleVoyages.v200, SampleLocations.NEWYORK, SampleLocations.CHICAGO,
                                                 DateUtil.ToDate("2009-03-10"), DateUtil.ToDate("2009-03-14")),
                                         new Leg(SampleVoyages.v200, SampleLocations.CHICAGO, SampleLocations.STOCKHOLM,
                                                 DateUtil.ToDate("2009-03-07"), DateUtil.ToDate("2009-03-11"))
                                     };
                // Hongkong - NYC - Chicago - Stockholm, initial routing
                return new List<Itinerary> { new Itinerary(nsLegs) };
            }
            else
            {
                var tsLegs = new List<Leg>
                                     {
                                         new Leg(SampleVoyages.v300, SampleLocations.TOKYO, SampleLocations.HAMBURG,
                                                 DateUtil.ToDate("2009-03-08"), DateUtil.ToDate("2009-03-12")),
                                         new Leg(SampleVoyages.v400, SampleLocations.HAMBURG, SampleLocations.STOCKHOLM,
                                                 DateUtil.ToDate("2009-03-14"), DateUtil.ToDate("2009-03-15"))
                                     };
                // Tokyo - Hamburg - Stockholm, rerouting misdirected cargo from Tokyo 
                return new List<Itinerary> { new Itinerary(tsLegs) };
            }
        }

        #endregion
    }
}