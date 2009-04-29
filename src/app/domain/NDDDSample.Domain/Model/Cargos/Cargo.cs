namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using Handlings;
    using Infrastructure.Validations;
    using Locations;
    using Shared;

    #endregion

    /// <summary>
    ///  Cargo (or freight) refers to goods or produce transported, 
    /// generally for commercial gain, by ship, aircraft, train, van or truck.  
    ///  Cargo is the central class in the domain model,
    /// and it is the root of the Cargo-Itinerary-Leg-Delivery-RouteSpecification aggregate. 
    /// A cargo is identified by a unique tracking id, and it always has an origin
    /// and a route specification. The life cycle of a cargo begins with the booking procedure,
    /// when the tracking id is assigned. During a (short) period of time, between booking
    /// and initial routing, the cargo has no itinerary.
    ///  The booking clerk requests a list of possible routes, matching the route specification,
    /// and assigns the cargo to one route. The route to which a cargo is assigned is described
    /// by an itinerary.
    /// A cargo can be re-routed during transport, on demand of the customer, in which case
    /// a new route is specified for the cargo and a new route is requested. The old itinerary,
    /// being a value object, is discarded and a new one is attached.
    /// It may also happen that a cargo is accidentally misrouted, which should notify the proper
    /// personnel and also trigger a re-routing procedure.
    ///  When a cargo is handled, the status of the delivery changes. Everything about the delivery
    /// of the cargo is contained in the Delivery value object, which is replaced whenever a cargo
    /// is handled by an asynchronous event triggered by the registration of the handling event.
    /// The delivery can also be affected by routing changes, i.e. when a the route specification
    /// changes, or the cargo is assigned to a new route. In that case, the delivery update is performed
    /// synchronously within the cargo aggregate.
    ///  The life cycle of a cargo ends when the cargo is claimed by the customer.
    /// The cargo aggregate, and the entre domain model, is built to solve the problem
    /// of booking and tracking cargo. All important business rules for determining whether
    /// or not a cargo is misdirected, what the current status of the cargo is (on board carrier,
    /// in port etc), are captured in this aggregate.
    /// </summary>
    public class Cargo : IEntity<Cargo>
    {
        private readonly Location origin;
        private readonly TrackingId trackingId;
        private Delivery delivery;
        private int id;
        private Itinerary itinerary;
        private RouteSpecification routeSpecification;

        #region Constr

        protected Cargo()
        {
            // Needed by Hibernate
        }

        public Cargo(TrackingId trackingId, RouteSpecification routeSpecification)
        {
            Validate.NotNull(trackingId, "Tracking ID is required");
            Validate.NotNull(routeSpecification, "Route specification is required");

            this.trackingId = trackingId;
            // Cargo origin never changes, even if the route specification changes.
            // However, at creation, cargo orgin can be derived from the initial route specification.
            origin = routeSpecification.Origin;
            this.routeSpecification = routeSpecification;

            delivery = Delivery.DerivedFrom(this.routeSpecification, itinerary, HandlingHistory.EMPTY);
        }

        #endregion

        #region IEntity<Cargo> Members

        /// <summary>
        /// Entities compare by identity, not by attributes.
        /// </summary>
        /// <param name="other">The other entity.</param>
        /// <returns>true if the identities are the same, regardles of other attributes.</returns>
        public virtual bool SameIdentityAs(Cargo other)
        {
            return other != null && trackingId.SameValueAs(other.trackingId);
        }

        #endregion

        #region Object's override

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Cargo) obj;
            return SameIdentityAs(other);
        }

        public override int GetHashCode()
        {
            return trackingId.GetHashCode();
        }


        public override string ToString()
        {
            return trackingId.ToString();
        }

        #endregion

        #region Props

        /// <summary>
        /// The tracking id is the identity of this entity, and is unique.
        /// </summary>      
        public virtual TrackingId TrackingId
        {
            get { return trackingId; }
        }

        /// <summary>
        /// Origin location.
        /// </summary>      
        public virtual Location Origin
        {
            get { return origin; }
        }

        /// <summary>
        /// The delivery. Never null.
        /// </summary>       
        public virtual Delivery Delivery
        {
            get { return delivery; }
        }

        /// <summary>
        /// The itinerary. Never null.
        /// </summary>       
        public virtual Itinerary Itinerary
        {
            get { return DomainObjectUtils.NullSafe(itinerary, Itinerary.EMPTY_ITINERARY); }
        }

        /// <summary>
        /// The route specification.
        /// </summary>       
        public virtual RouteSpecification RouteSpecification
        {
            get { return routeSpecification; }
        }

        #endregion

        #region Pub Methods

        /// <summary>
        /// Specifies a new route for this cargo.
        /// </summary>
        /// <param name="routeSpec">route specification.</param>
        public virtual void SpecifyNewRoute(RouteSpecification routeSpec)
        {
            Validate.NotNull(routeSpec, "Route specification is required");

            routeSpecification = routeSpec;
            // Handling consistency within the Cargo aggregate synchronously
            delivery = delivery.UpdateOnRouting(routeSpecification, itinerary);
        }

        /// <summary>
        /// Attach a new itinerary to this cargo.
        /// </summary>
        /// <param name="itineraryPrm">an itinerary. May not be null.</param>
        public virtual void AssignToRoute(Itinerary itineraryPrm)
        {
            Validate.NotNull(itineraryPrm, "Itinerary is required for assignment");

            itinerary = itineraryPrm;
            // Handling consistency within the Cargo aggregate synchronously
            delivery = delivery.UpdateOnRouting(routeSpecification, itinerary);
        }

        /// <summary>
        ///  Updates all aspects of the cargo aggregate status
        /// based on the current route specification, itinerary and handling of the cargo.
        ///<p/>
        ///When either of those three changes, i.e. when a new route is specified for the cargo,
        /// the cargo is assigned to a route or when the cargo is handled, the status must be
        /// re-calculated.
        /// <p/>
        /// RouteSpecification and Itinerary are both inside the Cargo
        /// aggregate, so changes to them cause the status to be updated <b>synchronously</b>,
        /// but changes to the delivery history (when a cargo is handled) cause the status update
        /// to happen <b>asynchronously</b> since HandlingEvent is in a different aggregate.
        /// </summary>
        /// <param name="handlingHistory"> Handling History </param>
        public virtual void DeriveDeliveryProgress(HandlingHistory handlingHistory)
        {
            // TODO filter events on cargo (must be same as this cargo)

            // Delivery is a value object, so we can simply discard the old one
            // and replace it with a new
            delivery = Delivery.DerivedFrom(RouteSpecification, Itinerary, handlingHistory);
        }

        #endregion
    }
}