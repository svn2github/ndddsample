namespace NDDDSample.Web.Controllers.Tracking
{
    #region Usings

    using Infrastructure.Builders;

    #endregion

    public class TrackCommand
    {
        private string trackingId;

        public TrackCommand()
        {
            trackingId = string.Empty;
        }

        /// <summary>
        /// The tracking id.
        /// </summary>
        public string TrackingId
        {
            get { return trackingId; }
            set { trackingId = value; }
        }


        public override string ToString()
        {
            return ToStringBuilder.ReflectionToString(this);
        }
    }
}