namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using Shared;

    #endregion

    /// <summary>
    /// Routing status. 
    /// </summary>
    public class RoutingStatus : IValueObject<RoutingStatus>
    {
        public static readonly RoutingStatus MISROUTED = new RoutingStatus();
        public static readonly RoutingStatus NOT_ROUTED = new RoutingStatus();
        public static readonly RoutingStatus ROUTED = new RoutingStatus();

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