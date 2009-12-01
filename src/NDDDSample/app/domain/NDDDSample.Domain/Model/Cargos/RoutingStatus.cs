namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using Shared;

    #endregion

    /// <summary>
    /// Routing status. 
    /// </summary>
    public class RoutingStatus : Enumeration, IValueObject<RoutingStatus>
    {
        public static readonly RoutingStatus MISROUTED = new RoutingStatus("MISROUTED");
        public static readonly RoutingStatus NOT_ROUTED = new RoutingStatus("NOT_ROUTED");
        public static readonly RoutingStatus ROUTED = new RoutingStatus("ROUTED");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Enum string name</param>
        private RoutingStatus(string name)
            : base(name) {}

        #region IValueObject<RoutingStatus> Members

        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        public bool SameValueAs(RoutingStatus other)
        {
            return Equals(other);
        }

        #endregion
    }
}