namespace NDDDSample.Interfaces.BookingRemoteService.Common.Dto
{
    #region Usings

    using System;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    /// DTO for a leg in an itinerary.
    /// </summary>    
    [DataContract]
    public class LegDTO
    {
        [DataMember] private string fromLocation;
        [DataMember] private DateTime loadTime;
        [DataMember] private string toLocation;
        [DataMember] private DateTime unloadTime;
        [DataMember] private string voyageNumber;

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
            fromLocation = from;
            toLocation = to;
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