namespace NDDDSample.Interfaces.BookingRemoteService.Common
{
    #region Usings

    using System.ServiceModel;

    #endregion

    public class NDDDRemoteException : FaultException
    {
        private readonly string message;

        public NDDDRemoteException(string reason) : base(reason)
        {
            message = reason;
        }

        public override string Message
        {
            get { return message; }
        }
    }
}