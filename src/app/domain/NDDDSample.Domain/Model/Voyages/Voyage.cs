namespace NDDDSample.Domain.Model.Voyages
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Locations;
    using Shared;
    using TempHelper;

    #endregion

    /// <summary>
    /// A GetVoyage.
    /// </summary>
    public class Voyage : IEntity<Voyage>
    {
        // Null object pattern
        public static readonly Voyage NONE = new Voyage(new VoyageNumber(""), Schedule.EMPTY);
        private readonly Schedule schedule;
        private readonly VoyageNumber voyageNumber;
        private long id;

        #region Neted GetVoyage Builder 

        /// <summary>
        ///  Builder pattern is used for incremental construction
        ///  of a GetVoyage aggregate. This serves as an aggregate factory.
        /// TODO:atrosin revise the java declaration if is port correctly:  static final class Builder {
        /// </summary>
        public class Builder
        {
            private readonly List<CarrierMovement> carrierMovements = new List<CarrierMovement>();
            private readonly VoyageNumber voyageNumber;
            private Location departureLocation;

            public Builder(VoyageNumber voyageNumber, Location departureLocation)
            {
                Validate.notNull(voyageNumber, "GetVoyage number is required");
                Validate.notNull(departureLocation, "Departure location is required");

                this.voyageNumber = voyageNumber;
                this.departureLocation = departureLocation;
            }

            public Builder AddMovement(Location arrivalLocation, DateTime departureTime, DateTime arrivalTime)
            {
                carrierMovements.Add(new CarrierMovement(departureLocation, arrivalLocation, departureTime, arrivalTime));
                // Next departure location is the same as this arrival location
                departureLocation = arrivalLocation;
                return this;
            }

            public Voyage Build()
            {
                return new Voyage(voyageNumber, new Schedule(carrierMovements));
            }
        }

        #endregion

        public Voyage(VoyageNumber voyageNumber, Schedule schedule)
        {
            Validate.notNull(voyageNumber, "GetVoyage number is required");
            Validate.notNull(schedule, "GetSchedule is required");

            this.voyageNumber = voyageNumber;
            this.schedule = schedule;
        }

        protected Voyage()
        {
            // Needed by Hibernate
        }

        #region IEntity<Voyage> Members

        public bool SameIdentityAs(Voyage other)
        {
            return other != null && VoyageNumber().SameValueAs(other.VoyageNumber());
        }

        #endregion

        /// <summary>
        /// GetVoyage number.
        /// </summary>
        /// <returns></returns>
        public VoyageNumber VoyageNumber()
        {
            return voyageNumber;
        }

        /// <summary>
        /// GetSchedule
        /// </summary>
        /// <returns></returns>
        public Schedule GetSchedule()
        {
            return schedule;
        }

        public override int GetHashCode()
        {
            return voyageNumber.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (!(obj.GetType().IsInstanceOfType(typeof (Voyage))))
            {
                return false;
            }

            var that = (Voyage) obj;

            return SameIdentityAs(that);
        }


        public override String ToString()
        {
            return "GetVoyage " + voyageNumber;
        }
    }
}