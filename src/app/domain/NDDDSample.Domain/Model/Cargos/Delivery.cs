namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Handlings;
    using Locations;
    using Shared;
    using TempHelper;
    using Voyages;

    #endregion

    /// <summary>
    /// The actual transportation of the cargo, as opposed to
    /// the customer requirement (RouteSpecification) and the plan (Itinerary). 
    /// </summary>
    public class Delivery : IValueObject<Delivery>
    {
        private static readonly DateTime ETA_UNKOWN = DateTime.MinValue;
        private static HandlingActivity NO_ACTIVITY;
        private readonly DateTime calculatedAt;
        private readonly Voyage currentVoyage;
        private readonly DateTime eta;
        private readonly bool isUnloadedAtDestination;
        private readonly HandlingEvent lastEvent;
        private readonly Location lastKnownLocation;
        private readonly bool misdirected;
        private readonly HandlingActivity nextExpectedActivity;
        private readonly RoutingStatus routingStatus;
        private readonly TransportStatus transportStatus;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="lastEvent">last event</param>
        /// <param name="itinerary">itinerary</param>
        /// <param name="routeSpecification">route specification</param>
        private Delivery(HandlingEvent lastEvent, Itinerary itinerary, RouteSpecification routeSpecification)
        {
            calculatedAt = new DateTime();
            this.lastEvent = lastEvent;

            misdirected = CalculateMisdirectionStatus(itinerary);
            routingStatus = CalculateRoutingStatus(itinerary, routeSpecification);
            transportStatus = CalculateTransportStatus();
            lastKnownLocation = CalculateLastKnownLocation();
            currentVoyage = CalculateCurrentVoyage();
            eta = CalculateEta(itinerary);
            nextExpectedActivity = CalculateNextExpectedActivity(routeSpecification, itinerary);
            isUnloadedAtDestination = CalculateUnloadedAtDestination(routeSpecification);
        }

        private Delivery()
        {
            // Needed by Hibernate
        }

        #region IValueObject<Delivery> Members

        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        public bool SameValueAs(Delivery other)
        {
            return other != null && new EqualsBuilder().
                                        Append(transportStatus, other.transportStatus).
                                        Append(lastKnownLocation, other.lastKnownLocation).
                                        Append(currentVoyage, other.currentVoyage).
                                        Append(misdirected, other.misdirected).
                                        Append(eta, other.eta).
                                        Append(nextExpectedActivity, other.nextExpectedActivity).
                                        Append(isUnloadedAtDestination, other.isUnloadedAtDestination).
                                        Append(routingStatus, other.routingStatus).
                                        Append(calculatedAt, other.calculatedAt).
                                        Append(lastEvent, other.lastEvent).
                                        IsEquals();
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

            Delivery other = (Delivery) obj;

            return SameValueAs(other);
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder().
                Append(transportStatus).
                Append(lastKnownLocation).
                Append(currentVoyage).
                Append(misdirected).
                Append(eta).
                Append(nextExpectedActivity).
                Append(isUnloadedAtDestination).
                Append(routingStatus).
                Append(calculatedAt).
                Append(lastEvent).
                ToHashCode();
        }

        #endregion

        //TODO: atrosin revise ETA_UNKOWN = null

        /// <summary>
        /// Creates a new delivery snapshot to reflect changes in routing, i.e.
        /// when the route specification or the itinerary has changed
        /// but no additional handling of the cargo has been performed.
        /// </summary>
        /// <param name="routeSpecification">route specification</param>
        /// <param name="itinerary">itinerary itinerary</param>
        /// <returns>An up to date delivery</returns>
        internal Delivery UpdateOnRouting(RouteSpecification routeSpecification, Itinerary itinerary)
        {
            Validate.notNull(routeSpecification, "Route specification is required");

            return new Delivery(lastEvent, itinerary, routeSpecification);
        }

        internal static Delivery DerivedFrom(RouteSpecification routeSpecification, Itinerary itinerary,
                                             HandlingHistory handlingHistory)
        {
            Validate.notNull(routeSpecification, "Route specification is required");
            Validate.notNull(handlingHistory, "Delivery history is required");

            HandlingEvent lastEvent = handlingHistory.MostRecentlyCompletedEvent();

            return new Delivery(lastEvent, itinerary, routeSpecification);
        }

        /// <summary>
        /// Transport status
        /// </summary>
        /// <returns></returns>
        public TransportStatus GetTransportStatus()
        {
            return transportStatus;
        }

        /// <summary>
        /// Last known location of the cargo, or Location.UNKNOWN if the delivery history is empty.
        /// </summary>
        /// <returns></returns>
        public Location LastKnownLocation()
        {
            return DomainObjectUtils.nullSafe(lastKnownLocation, Location.UNKNOWN);
        }

        /// <summary>
        /// Current voyage.
        /// </summary>
        /// <returns></returns>
        public Voyage CurrentVoyage()
        {
            return DomainObjectUtils.nullSafe(currentVoyage, Voyage.NONE);
        }

        /// <summary>
        /// Estimated time of arrival
        /// </summary>
        /// <returns></returns>
        public bool IsMisdirected()
        {
            return misdirected;
        }

        /// <summary>
        /// Estimated time of arrival
        /// </summary>
        /// <returns></returns>
        public DateTime EstimatedTimeOfArrival()
        {
            if (eta != ETA_UNKOWN)
            {
                //TODO: atrosin revise new Date(eta.getTime());
                return eta;
            }

            return ETA_UNKOWN;
        }

        /// <summary>
        /// The next expected handling activity.
        /// </summary>
        /// <returns></returns>
        public HandlingActivity NextExpectedActivity()
        {
            return nextExpectedActivity;
        }

        /// <summary>
        /// True if the cargo has been unloaded at the final destination.
        /// </summary>
        /// <returns></returns>
        public bool IsUnloadedAtDestination()
        {
            return isUnloadedAtDestination;
        }

        /// <summary>
        /// Routing status.
        /// </summary>
        /// <returns></returns>
        public RoutingStatus GetRoutingStatus()
        {
            return routingStatus;
        }

        /// <summary>
        /// When this delivery was calculated.
        /// </summary>
        /// <returns></returns>
        public DateTime CalculatedAt()
        {
            //TODO: atrosin new DateTime(calculatedAt.getTime());
            return calculatedAt;
        }

        // TODO add currentCarrierMovement (?)


        // --- Internal calculations below ---

        private TransportStatus CalculateTransportStatus()
        {
            //TODO: atrosin revise the if pattern spagetti code

            if (lastEvent == null)
            {
                return TransportStatus.NOT_RECEIVED;
            }

            if (lastEvent.Type() == HandlingEvent.HandlingType.LOAD)
            {
                return TransportStatus.ONBOARD_CARRIER;
            }

            bool isInPort = lastEvent.Type() == HandlingEvent.HandlingType.UNLOAD
                            || lastEvent.Type() == HandlingEvent.HandlingType.RECEIVE
                            || lastEvent.Type() == HandlingEvent.HandlingType.CUSTOMS;

            if (isInPort)
            {
                return TransportStatus.IN_PORT;
            }

            if (lastEvent.Type() == HandlingEvent.HandlingType.CLAIM)
            {
                return TransportStatus.CLAIMED;
            }

            return TransportStatus.UNKNOWN;
        }

        private Location CalculateLastKnownLocation()
        {
            if (lastEvent != null)
            {
                return lastEvent.Location();
            }
            return null;
        }

        private Voyage CalculateCurrentVoyage()
        {
            if (transportStatus.Equals(TransportStatus.ONBOARD_CARRIER) && lastEvent != null)
            {
                return lastEvent.GetVoyage();
            }
            return null;
        }

        private bool CalculateMisdirectionStatus(Itinerary itinerary)
        {
            if (lastEvent == null)
            {
                return false;
            }
            return !itinerary.IsExpected(lastEvent);
        }

        private DateTime CalculateEta(Itinerary itinerary)
        {
            if (OnTrack())
            {
                return itinerary.FinalArrivalDate();
            }

            return ETA_UNKOWN;
        }

        private HandlingActivity CalculateNextExpectedActivity(RouteSpecification routeSpecification,
                                                               Itinerary itinerary)
        {
            if (!OnTrack())
            {
                return NO_ACTIVITY;
            }

            if (lastEvent == null)
            {
                return new HandlingActivity(HandlingEvent.HandlingType.RECEIVE, routeSpecification.Origin());
            }

            if (lastEvent.Type() == HandlingEvent.HandlingType.LOAD)
            {
                foreach (Leg leg in itinerary.Legs())
                {
                    if (leg.LoadLocation().SameIdentityAs(lastEvent.Location()))
                    {
                        return new HandlingActivity(HandlingEvent.HandlingType.UNLOAD, leg.UnloadLocation(),
                                                    leg.Voyage());
                    }
                }

                return NO_ACTIVITY;
            }

            if (lastEvent.Type() == HandlingEvent.HandlingType.UNLOAD)
            {
                for (IEnumerator<Leg> it = itinerary.Legs().GetEnumerator(); it.MoveNext();)
                {
                    Leg leg = it.Current;
                    if (leg.UnloadLocation().SameIdentityAs(lastEvent.Location()))
                    {
                        if (it.MoveNext())
                        {
                            Leg nextLeg = it.Current;
                            return new HandlingActivity(HandlingEvent.HandlingType.LOAD, nextLeg.LoadLocation(),
                                                        nextLeg.Voyage());
                        }
                        return new HandlingActivity(HandlingEvent.HandlingType.CLAIM, leg.UnloadLocation());
                    }
                }
                return NO_ACTIVITY;
            }

            if (lastEvent.Type() == HandlingEvent.HandlingType.RECEIVE)
            {
                IEnumerator<Leg> enumerator = itinerary.Legs().GetEnumerator();
                enumerator.MoveNext();
                var firstLeg = enumerator.Current;
                return new HandlingActivity(HandlingEvent.HandlingType.LOAD, firstLeg.LoadLocation(), firstLeg.Voyage());
            }

            if (lastEvent.Type() == HandlingEvent.HandlingType.CLAIM)
            {
                //DO nothing
            }

            return NO_ACTIVITY;
        }


        private RoutingStatus CalculateRoutingStatus(Itinerary itinerary, RouteSpecification routeSpecification)
        {
            if (itinerary == null)
            {
                return RoutingStatus.NOT_ROUTED;
            }

            if (routeSpecification.IsSatisfiedBy(itinerary))
            {
                return RoutingStatus.ROUTED;
            }

            return RoutingStatus.MISROUTED;
        }

        private bool CalculateUnloadedAtDestination(RouteSpecification routeSpecification)
        {
            return lastEvent != null &&
                   HandlingEvent.HandlingType.UNLOAD.SameValueAs(lastEvent.Type()) &&
                   routeSpecification.Destination().SameIdentityAs(lastEvent.Location());
        }

        private bool OnTrack()
        {
            return routingStatus.Equals(RoutingStatus.ROUTED) && !misdirected;
        }
    }
}