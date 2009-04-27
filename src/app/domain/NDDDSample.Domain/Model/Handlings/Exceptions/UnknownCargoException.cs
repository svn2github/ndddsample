namespace NDDDSample.Domain.Model.Handlings.Exceptions
{
    using Cargos;

    public class UnknownCargoException : CannotCreateHandlingEventException
    {
        private readonly TrackingId trackingId;

        /// <summary>
        ///  Constructor.
        /// </summary>
        /// <param name="trackingId">trackingId cargo tracking id</param>
        public UnknownCargoException(TrackingId trackingId)
        {
            this.trackingId = trackingId;
        }

        public override string Message
        {
            get { return "No cargo with tracking id " + trackingId.IdString + " exists in the system"; }
        }
    }
}