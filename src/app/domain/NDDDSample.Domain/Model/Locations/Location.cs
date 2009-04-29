namespace NDDDSample.Domain.Model.Locations
{
    #region Usings

    using Infrastructure.Validations;
    using Shared;

    #endregion

    /// <summary>
    ///  A location is our model is stops on a journey, such as cargo
    ///origin or destination, or carrier movement endpoints.
    /// 
    /// It is uniquely identified by a UN Locode.
    /// </summary>
    public class Location : IEntity<Location>
    {
        public static readonly Location UNKNOWN = new Location(new UnLocode("XXXXX"), "Unknown location");
        private readonly string name;
        private readonly UnLocode unLocode;
        private int id;

        #region Constr

        /// <summary>
        ///  Package-level constructor, visible for test only.
        /// </summary>
        /// <param name="unLocode"> UN Locode</param>
        /// <param name="name">location name</param>
        /// TODO: See if it is possible NOT to make it public
        public Location(UnLocode unLocode, string name)
        {
            Validate.NotNull(unLocode);
            Validate.NotNull(name);

            this.unLocode = unLocode;
            this.name = name;
        }

        protected Location()
        {
            // Needed by Hibernate
        }

        #endregion

        #region IEntity<Location> Members

        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        public virtual bool SameIdentityAs(Location other)
        {
            return unLocode.SameValueAs(other.UnLocode);
        }

        #endregion

        #region Object's Override

        /// <summary>
        /// Compare objects.
        /// </summary>
        /// <param name="obj">object to compare</param>
        /// <returns>Since this is an entiy this will be true iff UN locodes are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this == obj)
            {
                return true;
            }
            if (!(obj is Location))
            {
                return false;
            }
            var other = (Location) obj;
            return SameIdentityAs(other);
        }


        public override int GetHashCode()
        {
            return unLocode.GetHashCode();
        }

        public override string ToString()
        {
            return name + " [" + unLocode + "]";
        }

        #endregion

        #region Public Props

        /// <summary>
        /// Voyage UN Locode for this location.
        /// </summary>
        public virtual UnLocode UnLocode
        {
            get { return unLocode; }
        }

        /// <summary>
        /// Actual name of this location, e.g. "Stockholm".
        /// </summary>
        /// <returns></returns>
        public virtual string Name
        {
            get { return name; }
        }

        #endregion
    }
}