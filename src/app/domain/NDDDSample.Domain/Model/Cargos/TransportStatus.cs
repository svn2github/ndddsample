namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using Shared;

    #endregion

    /// <summary>
    /// Represents the different transport statuses for a cargo.
    /// </summary>
    public class TransportStatus : IValueObject<TransportStatus>
    {
        public static readonly TransportStatus CLAIMED = new TransportStatus();
        public static readonly TransportStatus IN_PORT = new TransportStatus();
        public static readonly TransportStatus NOT_RECEIVED = new TransportStatus();
        public static readonly TransportStatus ONBOARD_CARRIER = new TransportStatus();
        public static readonly TransportStatus UNKNOWN = new TransportStatus();

        #region IValueObject<TransportStatus> Members

        public bool SameValueAs(TransportStatus other)
        {
            return Equals(other);
        }

        #endregion
    }
}