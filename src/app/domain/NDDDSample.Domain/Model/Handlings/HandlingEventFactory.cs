namespace NDDDSample.Domain.Model.Handlings
{
    #region Usings

    using System;
    using Cargos;
    using Exceptions;
    using Locations;
    using Voyages;

    #endregion

    /// <summary>
    /// Creates handling events.
    /// </summary>
    public class HandlingEventFactory
    {
        private readonly ICargoRepository cargoRepository;
        private readonly ILocationRepository locationRepository;
        private readonly IVoyageRepository voyageRepository;

        #region Constr

        public HandlingEventFactory(ICargoRepository cargoRepository,
                                    IVoyageRepository voyageRepository,
                                    ILocationRepository locationRepository)
        {
            this.cargoRepository = cargoRepository;
            this.voyageRepository = voyageRepository;
            this.locationRepository = locationRepository;
        }

        #endregion

        #region Factory Method

        /// <summary>
        /// Creates handling event
        /// throws UnknownVoyageException   if there's no voyage with this number
        /// throws UnknownCargoException    if there's no cargo with this tracking id
        /// throws UnknownLocationException if there's no location with this UN Locode
        /// </summary>
        /// <param name="registrationTime"> time when this event was received by the system</param>
        /// <param name="completionTime">when the event was completed, for example finished loading</param>
        /// <param name="trackingId">cargo tracking id</param>
        /// <param name="voyageNumber">voyage number</param>
        /// <param name="unlocode">United Nations Location Code for the location of the event</param>
        /// <param name="type">type of event</param>
        /// <returns> A handling event.</returns>
        public HandlingEvent CreateHandlingEvent(DateTime registrationTime, DateTime completionTime,
                                                 TrackingId trackingId, VoyageNumber voyageNumber, UnLocode unlocode,
                                                 HandlingType type)
        {
            Cargo cargo = FindCargo(trackingId);
            Voyage voyage = FindVoyage(voyageNumber);
            Location location = FindLocation(unlocode);

            try
            {
                if (voyage == null)
                {
                    return new HandlingEvent(cargo, completionTime, registrationTime, type, location);
                }

                return new HandlingEvent(cargo, completionTime, registrationTime, type, location, voyage);
            }
            catch (Exception e)
            {
                throw new CannotCreateHandlingEventException(e);
            }
        }

        #endregion

        #region Find Methods

        private Cargo FindCargo(TrackingId trackingId)
        {
            Cargo cargo = cargoRepository.Find(trackingId);
            if (cargo == null)
            {
                throw new UnknownCargoException(trackingId);
            }
            return cargo;
        }

        private Voyage FindVoyage(VoyageNumber voyageNumber)
        {
            if (voyageNumber == null)
            {
                return null;
            }

            Voyage voyage = voyageRepository.Find(voyageNumber);
            if (voyage == null)
            {
                throw new UnknownVoyageException(voyageNumber);
            }

            return voyage;
        }

        private Location FindLocation(UnLocode unlocode)
        {
            Location location = locationRepository.Find(unlocode);
            if (location == null)
            {
                throw new UnknownLocationException(unlocode);
            }

            return location;
        }

        #endregion
    }
}