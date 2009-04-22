namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using Shared;

    #endregion

    /// <summary>
    /// Represents the different transport statuses for a cargo.
    /// </summary>
    public class TransportStatus : Enumeration, IValueObject<TransportStatus>
    {
        public static readonly TransportStatus CLAIMED = new TransportStatus("CLAIMED");
        public static readonly TransportStatus IN_PORT = new TransportStatus("IN_PORT");
        public static readonly TransportStatus NOT_RECEIVED = new TransportStatus("NOT_RECEIVED");
        public static readonly TransportStatus ONBOARD_CARRIER = new TransportStatus("ONBOARD_CARRIER");
        public static readonly TransportStatus UNKNOWN = new TransportStatus("UNKNOWN");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Enum string name</param>
        private TransportStatus(string name)
            : base(name) {}

        #region IValueObject<TransportStatus> Members

        public bool SameValueAs(TransportStatus other)
        {
            return Equals(other);
        }

        #endregion
    }
}