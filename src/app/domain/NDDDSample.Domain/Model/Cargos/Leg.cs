namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using System;
    using JavaRelated;
    using Locations;
    using Shared;
    using Voyages;

    #endregion

    /// <summary>
    /// An itinerary consists of one or more legs.
    /// </summary>
    public class Leg : IValueObject<Leg>
    {
        private readonly Location loadLocation;
        private readonly DateTime loadTime;
        private readonly Location unloadLocation;
        private readonly DateTime unloadTime;
        private readonly Voyage voyage;
        protected long id;

        #region Constr

        public Leg(Voyage voyage, Location loadLocation, Location unloadLocation, DateTime loadTime, DateTime unloadTime)
        {
            Validate.NoNullElements(new object[] {voyage, loadLocation, unloadLocation, loadTime, unloadTime});

            this.voyage = voyage;
            this.loadLocation = loadLocation;
            this.unloadLocation = unloadLocation;
            this.loadTime = loadTime;
            this.unloadTime = unloadTime;
        }

        protected Leg()
        {
            // Needed by Hibernate
        }

        #endregion

        #region IValueObject<Leg> Members

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

        #region Public props

        public Voyage Voyage
        {
            get { return voyage; }
        }

        public Location LoadLocation
        {
            get { return loadLocation; }
        }

        public Location UnloadLocation
        {
            get { return unloadLocation; }
        }

        public DateTime LoadTime
        {
            get { return loadTime; }
        }

        public DateTime UnloadTime
        {
            get
            {
                //TODO: atrosin : new Date(unloadTime.getTime());
                return unloadTime;
            }
        }

        #endregion
    }
}