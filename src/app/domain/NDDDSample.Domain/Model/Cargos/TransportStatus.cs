namespace NDDDSample.Domain.Model.Cargos
{
    using Shared;

    /// <summary>
    /// Represents the different transport statuses for a cargo.
    /// </summary>
    public class TransportStatus : IValueObject<TransportStatus>
    {
        public static readonly TransportStatus NOT_RECEIVED = new TransportStatus();
        public static readonly TransportStatus IN_PORT = new TransportStatus();
        public static readonly TransportStatus ONBOARD_CARRIER = new TransportStatus();
        public static readonly TransportStatus CLAIMED = new TransportStatus();
        public static readonly TransportStatus UNKNOWN = new TransportStatus();

        public bool SameValueAs(TransportStatus other)
        {
            return this.Equals(other);
        }
    }
}
