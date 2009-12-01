namespace NDDDSample.Interfaces.BookingRemoteService.Common
{
    #region Usings

    using System.ServiceModel;

    #endregion

    public class NDDDRemoteBookingException : FaultException
    {
        private readonly string message;

        public NDDDRemoteBookingException(string reason) : base(reason)
        {
            message = reason;
        }

        public override string Message
        {
            get { return message; }
        }
    }
}