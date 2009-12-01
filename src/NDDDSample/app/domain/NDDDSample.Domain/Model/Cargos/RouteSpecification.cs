namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using System;
    using Infrastructure.Builders;
    using Infrastructure.Utils;
    using Infrastructure.Validations;
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

        #region Constr

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="origin">origin location - can't be the same as the destination</param>
        /// <param name="destination">destination location - can't be the same as the origin</param>
        /// <param name="arrivalDeadline">arrival deadline</param>
        public RouteSpecification(Location origin, Location destination, DateTime arrivalDeadline)
        {
            Validate.NotNull(origin, "Origin is required");
            Validate.NotNull(destination, "Destination is required");
            Validate.NotNull(arrivalDeadline, "Arrival deadline is required");
            Validate.IsTrue(!origin.SameIdentityAs(destination), "Origin and destination can't be the same: " + origin);

            this.origin = origin;
            this.destination = destination;
            this.arrivalDeadline = arrivalDeadline;
        }

        protected RouteSpecification()
        {
            // Needed by Hibernate
        }

        #endregion

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

        #region Props

        /// <summary>
        /// Specified origin location.
        /// </summary>
        public Location Origin
        {
            get { return origin; }
        }

        /// <summary>
        /// Specfied destination location.
        /// </summary>
        public Location Destination
        {
            get { return destination; }
        }

        public DateTime ArrivalDeadline
        {
            get { return arrivalDeadline; }
        }

        #endregion

        public override bool IsSatisfiedBy(Itinerary itinerary)
        {
            return itinerary != null &&
                   Origin.SameIdentityAs(itinerary.InitialDepartureLocation) &&
                   Destination.SameIdentityAs(itinerary.FinalArrivalLocation) &&
                   ArrivalDeadline.After(itinerary.FinalArrivalDate);
        }
    }
}