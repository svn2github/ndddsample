namespace NDDDSample.Application
{
    using System;
    using System.Collections.Generic;
    using Domain.Model.Cargos;
    using Domain.Model.Locations;

    /// <summary>
    /// Cargo booking service.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Registers a new cargo in the tracking system, not yet routed.
        /// </summary>
        /// <param name="origin">cargo origin</param>
        /// <param name="destination">cargo destination</param>
        /// <param name="arrivalDeadline">eadline arrival deadline</param>
        /// <returns>tracking id</returns>
        TrackingId bookNewCargo(UnLocode origin, UnLocode destination, DateTime arrivalDeadline);

        /// <summary>
        /// Requests a list of itineraries describing possible routes for this cargo.
        /// </summary>
        /// <param name="trackingId">cargo tracking id</param>
        /// <returns>A list of possible itineraries for this cargo</returns>
        IList<Itinerary> requestPossibleRoutesForCargo(TrackingId trackingId);

        /// <summary>
        /// Assign Cargo To Route
        /// </summary>
        /// <param name="itinerary">itinerary describing the selected route</param>
        /// <param name="trackingId">cargo tracking id</param>
        void assignCargoToRoute(Itinerary itinerary, TrackingId trackingId);

        /// <summary>
        /// Changes the destination of a cargo.
        /// </summary>
        /// <param name="trackingId">argo tracking id</param>
        /// <param name="unLocode">UN locode of new destination</param>
        void changeDestination(TrackingId trackingId, UnLocode unLocode);
    }
}
