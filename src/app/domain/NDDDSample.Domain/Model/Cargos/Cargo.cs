namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using Locations;
    using NDDDSample.Domain.Shared;

    #endregion

    /// <summary>
    ///  A Cargo. This is the central class in the domain model,
    ///and it is the root of the Cargo-Itinerary-Leg-Delivery-RouteSpecification aggregate. 
    ///A cargo is identified by a unique tracking id, and it always has an origin
    /// and a route specification. The life cycle of a cargo begins with the booking procedure,
    ///when the tracking id is assigned. During a (short) period of time, between booking
    ///and initial routing, the cargo has no itinerary.
    /// The booking clerk requests a list of possible routes, matching the route specification,
    ///and assigns the cargo to one route. The route to which a cargo is assigned is described
    ///by an itinerary.
    /// A cargo can be re-routed during transport, on demand of the customer, in which case
    ///a new route is specified for the cargo and a new route is requested. The old itinerary,
    ///being a value object, is discarded and a new one is attached.
    ///It may also happen that a cargo is accidentally misrouted, which should notify the proper
    ///personnel and also trigger a re-routing procedure.
    ///When a cargo is handled, the status of the delivery changes. Everything about the delivery
    ///of the cargo is contained in the Delivery value object, which is replaced whenever a cargo
    ///is handled by an asynchronous event triggered by the registration of the handling event.
    /// The delivery can also be affected by routing changes, i.e. when a the route specification
    /// changes, or the cargo is assigned to a new route. In that case, the delivery update is performed
    ///synchronously within the cargo aggregate.
    /// The life cycle of a cargo ends when the cargo is claimed by the customer.
    ///The cargo aggregate, and the entre domain model, is built to solve the problem
    ///of booking and tracking cargo. All important business rules for determining whether
    ///or not a cargo is misdirected, what the current status of the cargo is (on board carrier,
    ///in port etc), are captured in this aggregate.
    /// </summary>
    public class Cargo : IEntity<Cargo>
    {
        private TrackingId trackingId;
        private Location origin;
        private RouteSpecification routeSpecification;
        private Itinerary itinerary;
        private Delivery delivery;
       

        /// <summary>
        /// Entities compare by identity, not by attributes.
        /// </summary>
        /// <param name="other">The other entity.</param>
        /// <returns>true if the identities are the same, regardles of other attributes.</returns>
        public bool SameIdentityAs(Cargo other)
        {
            throw new System.NotImplementedException();
        }

       
    }
}