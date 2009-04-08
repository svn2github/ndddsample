namespace NDDDSample.Application
{
    using System;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;

    /// <summary>
    /// Handling event service.
    /// </summary>
    public interface IHandlingEventService
    {
        /// <summary>
        /// Registers a handling event in the system, and notifies interested
        /// parties that a cargo has been handled.
        /// @throws CannotCreateHandlingEventException if a handling event that represents an actual 
        /// event that's relevant to a cargo we're tracking can't be created from the parameters. 
        /// </summary>
        /// <param name="completionTime">when the event was completed</param>
        /// <param name="trackingId">cargo tracking id</param>
        /// <param name="voyageNumber">voyage number</param>
        /// <param name="unLocode">UN locode for the location where the event occurred</param>
        /// <param name="type">type of event</param>
         void registerHandlingEvent(DateTime completionTime,
                             TrackingId trackingId,
                             VoyageNumber voyageNumber,
                             UnLocode unLocode,
                             HandlingEvent.HandlingType type);
    }
}
