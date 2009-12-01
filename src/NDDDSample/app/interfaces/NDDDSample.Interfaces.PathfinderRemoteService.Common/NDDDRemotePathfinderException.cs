namespace NDDDSample.Interfaces.PathfinderRemoteService.Common
{
    #region Usings

    using System.ServiceModel;

    #endregion

    public class NDDDRemotePathfinderException : FaultException
    {
        private readonly string message;

        public NDDDRemotePathfinderException(string reason)
            : base(reason)
        {
            message = reason;
        }

        public override string Message
        {
            get { return message; }
        }
    }
}