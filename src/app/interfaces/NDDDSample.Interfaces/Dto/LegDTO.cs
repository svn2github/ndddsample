namespace NDDDSample.Interfaces.BookingRemoteService.Common.Dto
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// DTO for a leg in an itinerary.
    /// </summary>
    [Serializable]
    [DataContract]
    public class LegDTO
    {
        private readonly string fromLocation;
        private readonly DateTime loadTime;
        private readonly string toLocation;
        private readonly DateTime unloadTime;
        private readonly string voyageNumber;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="voyageNumber">voyage Number</param>
        /// <param name="from">from Location</param>
        /// <param name="to">toLocation</param>
        /// <param name="loadTime">load Time</param>
        /// <param name="unloadTime">unload Time</param>
        public LegDTO(string voyageNumber, string from, string to, DateTime loadTime, DateTime unloadTime)
        {
            this.voyageNumber = voyageNumber;
            this.fromLocation = from;
            this.toLocation = to;
            this.loadTime = loadTime;
            this.unloadTime = unloadTime;
        }

        public string VoyageNumber
        {
            get { return voyageNumber; }
        }

        public string FromLocation
        {
            get { return fromLocation; }
        }

        public string ToLocation
        {
            get { return toLocation; }
        }

        public DateTime LoadTime
        {
            get { return loadTime; }
        }

        public DateTime UnloadTime
        {
            get { return unloadTime; }
        }
    }
}
