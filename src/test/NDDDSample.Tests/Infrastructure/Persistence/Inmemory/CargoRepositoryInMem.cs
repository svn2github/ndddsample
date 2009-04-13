namespace NDDDSample.Tests.Infrastructure.Persistence.Inmemory
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Locations;

    #endregion

    /// <summary>
    /// CargoRepositoryInMem implement the CargoRepository interface but is a test
    /// class not intended for usage in real application.
    /// <p/>
    /// It setup a simple local hash with a number of Cargo's with TrackingId as key
    /// defined at compile time.
    /// </summary>
    public class CargoRepositoryInMem : ICargoRepository
    {
        private readonly IDictionary<string, Cargo> cargoDb;
        private IHandlingEventRepository handlingEventRepository;


        /// <summary>
        /// Constructor
        /// </summary>
        public CargoRepositoryInMem()
        {
            cargoDb = new Dictionary<string, Cargo>();
        }

        #region ICargoRepository Members

        public Cargo Find(TrackingId trackingId)
        {
            return cargoDb[trackingId.IdString];
        }

        public void Store(Cargo cargo)
        {
            if (cargoDb.ContainsKey(cargo.TrackingId.IdString))
            {
                cargoDb[cargo.TrackingId.IdString] = cargo;
            }
            else
            {
                cargoDb.Add(cargo.TrackingId.IdString, cargo);
            }
        }

        public TrackingId NextTrackingId()
        {
            string random = Guid.NewGuid().ToString().ToUpper();
            return new TrackingId(
                random.Substring(0, random.IndexOf("-"))
                );
        }

        public IList<Cargo> FindAll()
        {
            return new List<Cargo>(cargoDb.Values);
        }

        #endregion

        public void Init()
        {
            TrackingId xyz = new TrackingId("XYZ");
            Cargo cargoXYZ = createCargoWithDeliveryHistory(
                xyz, SampleLocations.STOCKHOLM, SampleLocations.MELBOURNE,
                handlingEventRepository.LookupHandlingHistoryOfCargo(xyz));
            cargoDb.Add(xyz.IdString, cargoXYZ);

            TrackingId zyx = new TrackingId("ZYX");
            Cargo cargoZYX = createCargoWithDeliveryHistory(
                zyx, SampleLocations.MELBOURNE, SampleLocations.STOCKHOLM,
                handlingEventRepository.LookupHandlingHistoryOfCargo(zyx));
            cargoDb.Add(zyx.IdString, cargoZYX);

            TrackingId abc = new TrackingId("ABC");
            Cargo cargoABC = createCargoWithDeliveryHistory(
                abc, SampleLocations.STOCKHOLM, SampleLocations.HELSINKI,
                handlingEventRepository.LookupHandlingHistoryOfCargo(abc));
            cargoDb.Add(abc.IdString, cargoABC);

            TrackingId cba = new TrackingId("CBA");
            Cargo cargoCBA = createCargoWithDeliveryHistory(
                cba, SampleLocations.HELSINKI, SampleLocations.STOCKHOLM,
                handlingEventRepository.LookupHandlingHistoryOfCargo(cba));
            cargoDb.Add(cba.IdString, cargoCBA);
        }

        public void SetHandlingEventRepository(IHandlingEventRepository handeEventRepository)
        {
            handlingEventRepository = handeEventRepository;
        }

        public static Cargo createCargoWithDeliveryHistory(TrackingId trackingId,
                                                           Location origin,
                                                           Location destination,
                                                           HandlingHistory handlingHistory)
        {
            RouteSpecification routeSpecification = new RouteSpecification(origin, destination, new DateTime());
            Cargo cargo = new Cargo(trackingId, routeSpecification);
            cargo.DeriveDeliveryProgress(handlingHistory);

            return cargo;
        }
    }
}