namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using System;
    using Locations;
    using Shared;
    using TempHelper;
    using Voyages;

    #endregion

    /// <summary>
    /// An itinerary consists of one or more legs.
    /// </summary>
    public class Leg : IValueObject<Leg>
    {
        private Voyage voyage;
        private Location loadLocation;
        private Location unloadLocation;
        private DateTime loadTime;
        private DateTime unloadTime;

        public Leg(Voyage voyage, Location loadLocation, Location unloadLocation, DateTime loadTime, DateTime unloadTime)
        {
            Validate.noNullElements(new Object[] {voyage, loadLocation, unloadLocation, loadTime, unloadTime});

            this.voyage = voyage;
            this.loadLocation = loadLocation;
            this.unloadLocation = unloadLocation;
            this.loadTime = loadTime;
            this.unloadTime = unloadTime;
        }

        public Voyage Voyage()
        {
            return voyage;
        }

        public Location LoadLocation()
        {
            return loadLocation;
        }

        public Location UnloadLocation()
        {
            return unloadLocation;
        }

        public DateTime LoadTime()
        {
            return loadTime;
        }

        public DateTime UnloadTime()
        {
            //TODO: atrosin : new Date(unloadTime.getTime());
            return unloadTime;
        }

        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        public bool SameValueAs(Leg other)
        {
            return other != null && new EqualsBuilder().
                                        Append(voyage, other.voyage).
                                        Append(loadLocation, other.loadLocation).
                                        Append(unloadLocation, other.unloadLocation).
                                        Append(loadTime, other.loadTime).
                                        Append(unloadTime, other.unloadTime).
                                        IsEquals();
        }

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

            var leg = (Leg) obj;

            return SameValueAs(leg);
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder().
                Append(voyage).
                Append(loadLocation).
                Append(unloadLocation).
                Append(loadTime).
                Append(unloadTime).
                ToHashCode();
        }

        #endregion

        protected Leg()
        {
            // Needed by Hibernate
        }

        // Auto-generated surrogate key
        protected long id;
    }
}