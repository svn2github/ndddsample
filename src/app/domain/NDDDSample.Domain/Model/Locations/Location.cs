namespace NDDDSample.Domain.Model.Locations
{
    #region Usings

    using JavaRelated;
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
        protected long id;

        #region Constr

        /// <summary>
        ///  Package-level constructor, visible for test only.
        /// </summary>
        /// <param name="unLocode"> UN Locode</param>
        /// <param name="name">location name</param>
        internal Location(UnLocode unLocode, string name)
        {
            Validate.notNull(unLocode);
            Validate.notNull(name);

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
        public bool SameIdentityAs(Location other)
        {
            return unLocode.SameValueAs(other.unLocode);
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
            if (!(obj.GetType().IsInstanceOfType(typeof (Location))))
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
        public UnLocode UnLocode
        {
            get { return unLocode; }
        }

        /// <summary>
        /// Actual name of this location, e.g. "Stockholm".
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get { return name; }
        }

        #endregion
    }
}