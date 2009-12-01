namespace NDDDSample.Web.Controllers.CargoAdmin
{
    /// <summary>
    /// The command represents a request that is used to pass data
    /// from RegistrationForm to Register action in order to register new 
    /// Cargo. 
    /// </summary>
    public class RegistrationCommand
    {
        private string originUnlocode;
        private string destinationUnlocode;
        private string arrivalDeadline;

        public RegistrationCommand(string originUnlocode, string destinationUnlocode, string arrivalDeadline)
        {
            this.originUnlocode = originUnlocode;
            this.destinationUnlocode = destinationUnlocode;
            this.arrivalDeadline = arrivalDeadline;
        }

        public string OriginUnlocode
        {
            get { return originUnlocode; }
        }

        public string DestinationUnlocode
        {
            get { return destinationUnlocode; }
        }

        public string ArrivalDeadline
        {
            get { return arrivalDeadline; }
        }
    }
}