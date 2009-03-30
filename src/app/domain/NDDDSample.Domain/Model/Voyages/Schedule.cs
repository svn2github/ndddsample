namespace NDDDSample.Domain.Model.Voyages
{
    #region Usings

    using System.Collections.Generic;
    using Shared;
    using TempHelper;

    #endregion

    /// <summary>
    /// A voyage schedule.
    /// </summary>
    public class Schedule : IValueObject<Schedule>
    {
        private readonly List<CarrierMovement> carrierMovements = new List<CarrierMovement>();

        public static readonly Schedule EMPTY = new Schedule();


        internal Schedule(List<CarrierMovement> carrierMovements)
        {
            Validate.notNull(carrierMovements);
            Validate.noNullElements(carrierMovements);
            Validate.notEmpty(carrierMovements);

            this.carrierMovements = carrierMovements;
        }

        /// <summary>
        /// Carrier movements.
        /// </summary>
        /// <returns></returns>
        public IList<CarrierMovement> CarrierMovements()
        {
            return new List<CarrierMovement>(carrierMovements).AsReadOnly();
        }

        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        public bool SameValueAs(Schedule other)
        {
            return other != null && carrierMovements.Equals(other.carrierMovements);
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (o == null || GetType() != o.GetType())
            {
                return false;
            }

            var that = (Schedule) o;

            return SameValueAs(that);
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder().Append(carrierMovements).ToHashCode();
        }

        private Schedule()
        {
            // Needed by Hibernate
        }
    }
}