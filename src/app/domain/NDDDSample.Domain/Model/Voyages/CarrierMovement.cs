namespace NDDDSample.Domain.Model.Voyages
{
    #region Usings

    using System;   
    using Locations;
    using Shared;
    using TempHelper;

    #endregion

    /// <summary>
    /// A carrier movement is a vessel voyage from one location to another.
    /// </summary>
    public class CarrierMovement : IValueObject<CarrierMovement>
    {
        private readonly Location departureLocation;
        private readonly Location arrivalLocation;
        private DateTime departureTime;
        private DateTime arrivalTime;

        // Null object pattern 
        public static CarrierMovement NONE = new CarrierMovement(
            Location.UNKNOWN, Location.UNKNOWN, new DateTime(0), new DateTime(0));


        /// <summary>
        /// Constructor.
        /// TODO make package local
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
            Validate.noNullElements(new object[] {departureLocation, arrivalLocation, departureTime, arrivalTime});
            this.departureTime = departureTime;
            this.arrivalTime = arrivalTime;
            this.departureLocation = departureLocation;
            this.arrivalLocation = arrivalLocation;
        }


        /// <summary>
        /// Departure location.
        /// </summary>
        /// <returns></returns>
        public Location DepartureLocation()
        {
            return departureLocation;
        }

        /// <summary>
        /// Arrival location.
        /// </summary>
        /// <returns></returns>
        public Location ArrivalLocation()
        {
            return arrivalLocation;
        }

        /// <summary>
        /// Time of departure.
        /// </summary>
        /// <returns></returns>
        public DateTime DepartureTime()
        {
            //TODO: atrosin : new Date(arrivalTime.getTime());
            return departureTime;
        }

        /// <summary>
        ///  Time of arrival.
        /// </summary>
        /// <returns></returns>
        public DateTime ArrivalTime()
        {
            //TODO: atrosin : new Date(arrivalTime.getTime());
            return arrivalTime;
        }

        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        public bool SameValueAs(CarrierMovement other)
        {
            return other != null && new EqualsBuilder().
              Append(this.departureLocation, other.departureLocation).
              Append(this.departureTime, other.departureTime).
              Append(this.arrivalLocation, other.arrivalLocation).
              Append(this.arrivalTime, other.arrivalTime).
              IsEquals();
        }    

        #region Object's overrides

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            var that = (CarrierMovement)obj;

            return SameValueAs(that);

        }           

        public override int GetHashCode()
        {
            return new HashCodeBuilder().
              Append(this.departureLocation).
              Append(this.departureTime).
              Append(this.arrivalLocation).
              Append(this.arrivalTime).
              ToHashCode();
        }

        #endregion

        protected CarrierMovement()
        {
            // Needed by Hibernate
        }

        // Auto-generated surrogate key
        protected long id;
    }
}