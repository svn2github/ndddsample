namespace NDDDSample.Domain.Model.Voyages
{
    #region Usings

    using System;
    using Infrastructure.Builders;
    using Infrastructure.Validations;
    using Locations;
    using Shared;

    #endregion

    /// <summary>
    /// A carrier movement is a vessel voyage from one location to another.
    /// </summary>
    public class CarrierMovement : IValueObject<CarrierMovement>
    {
        #region Private fields

        // Null object pattern 
        public static CarrierMovement NONE = new CarrierMovement(
            Location.UNKNOWN, Location.UNKNOWN, new DateTime(0), new DateTime(0));

        private readonly Location arrivalLocation;
        private readonly DateTime arrivalTime;
        private readonly Location departureLocation;
        private readonly DateTime departureTime;
        private int id;

        #endregion

        #region Constr

        /// <summary>
        /// Constructor.
        /// TODO make assembly local
        /// </summary>
        /// <param name="departureLocation">location of departure</param>
        /// <param name="arrivalLocation">location of arrival</param>
        /// <param name="departureTime">time of departure</param>
        /// <param name="arrivalTime">time of arrival</param>
        public CarrierMovement(Location departureLocation,
                               Location arrivalLocation,
                               DateTime departureTime,
                               DateTime arrivalTime)
        {
            Validate.NoNullElements(new object[] {departureLocation, arrivalLocation, departureTime, arrivalTime});
            this.departureTime = departureTime;
            this.arrivalTime = arrivalTime;
            this.departureLocation = departureLocation;
            this.arrivalLocation = arrivalLocation;
        }

        protected CarrierMovement()
        {
            // Needed by Hibernate
        }

        #endregion

        #region IValueObject<CarrierMovement> Members

        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        public virtual bool SameValueAs(CarrierMovement other)
        {
            return other != null && new EqualsBuilder().
                                        Append(departureLocation, other.departureLocation).
                                        Append(departureTime, other.departureTime).
                                        Append(arrivalLocation, other.arrivalLocation).
                                        Append(arrivalTime, other.arrivalTime).
                                        IsEquals();
        }

        #endregion

        #region Object's overrides

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

            var that = (CarrierMovement) obj;

            return SameValueAs(that);
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder().
                Append(departureLocation).
                Append(departureTime).
                Append(arrivalLocation).
                Append(arrivalTime).
                ToHashCode();
        }

        #endregion

        #region Public Props

        /// <summary>
        /// Departure location.
        /// </summary>
        public virtual Location DepartureLocation
        {
            get { return departureLocation; }
        }

        /// <summary>
        /// Arrival location.
        /// </summary>
        public virtual Location ArrivalLocation
        {
            get { return arrivalLocation; }
        }

        /// <summary>
        /// Time of departure.
        /// </summary>
        public virtual DateTime DepartureTime
        {
            get { return departureTime; }
        }

        /// <summary>
        ///  Time of arrival.
        /// </summary>
        public virtual DateTime ArrivalTime
        {
            get { return arrivalTime; }
        }

        #endregion
    }
}