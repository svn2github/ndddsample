namespace NDDDSample.Interfaces.HandlingService.WebService
{
    #region Usings

    using System.Runtime.Serialization;

    #endregion

    [DataContract]
    public class HandlingReportException
    {
        private string Msg;

        public HandlingReportException(string msg)
        {
            Msg = msg;
        }

        [DataMember]
        public string ErrorMessage
        {
            get { return Msg; }
            set { Msg = value; }
        }
    }
}