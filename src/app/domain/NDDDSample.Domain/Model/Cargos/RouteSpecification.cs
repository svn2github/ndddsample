namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using System;
    using JavaRelated;
    using Locations;
    using Shared;

    #endregion

    /// <summary>
    ///  Route specification. Describes where a cargo orign and destination is,
    /// and the arrival deadline.
    /// </summary>
    public class RouteSpecification : AbstractSpecification<Itinerary>, IValueObject<RouteSpecification>
    {
        private readonly DateTime arrivalDeadline;
        private readonly Location destination;
        private readonly Location origin;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="origin">origin location - can't be the same as the destination</param>
        /// <param name="destination">destination location - can't be the same as the origin</param>
        /// <param name="arrivalDeadline">arrival deadline</param>
        public RouteSpecification(Location origin, Location destination, DateTime arrivalDeadline)
        {
            Validate.notNull(origin, "Origin is required");
            Validate.notNull(destination, "Destination is required");
            Validate.notNull(arrivalDeadline, "Arrival deadline is required");
            Validate.isTrue(!origin.SameIdentityAs(destination), "Origin and destination can't be the same: " + origin);

            this.origin = origin;
            this.destination = destination;
            //TODO: atrosin revise (DateTime)arrivalDeadline.clone();
            this.arrivalDeadline = arrivalDeadline;
        }

        private RouteSpecification()
        {
            // Needed by Hibernate
        }

        #region IValueObject<RouteSpecification> Members

        public bool SameValueAs(RouteSpecification other)
        {
            return other != null && new EqualsBuilder().
                                        Append(origin, other.origin).
                                        Append(destination, other.destination).
                                        Append(arrivalDeadline, other.arrivalDeadline).
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

            var that = (RouteSpecification) obj;

            return SameValueAs(that);
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder().
                Append(origin).
                Append(destination).
                Append(arrivalDeadline).
                ToHashCode();
        }

        #endregion

        /// <summary>
        /// Specified origin location.
        /// </summary>
        /// <returns></returns>
        public Location Origin()
        {
            return origin;
        }

        /// <summary>
        /// Specfied destination location.
        /// </summary>
        /// <returns></returns>
        public Location Destination()
        {
            return destination;
        }

        public DateTime ArrivalDeadline()
        {
            //TODO: atrosin revise new Date(arrivalDeadline.getTime());
            return arrivalDeadline;
        }

        public override bool IsSatisfiedBy(Itinerary itinerary)
        {
            return itinerary != null &&
                   Origin().SameIdentityAs(itinerary.InitialDepartureLocation()) &&
                   Destination().SameIdentityAs(itinerary.FinalArrivalLocation()) &&
                   ArrivalDeadline().After(itinerary.FinalArrivalDate());
        }
    }
}