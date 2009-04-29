namespace NDDDSample.Interfaces.Bookings.Facade.Dto
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Infrastructure.Utils;

    #endregion

    /// <summary>
    /// DTO for registering and routing a cargo.
    /// </summary>
    [Serializable]
    public class CargoRoutingDTO
    {
        private readonly DateTime arrivalDeadline;
        private readonly string finalDestination;
        private readonly IList<LegDTO> legs;
        private readonly bool misrouted;
        private readonly string origin;
        private readonly string trackingId;


        /// <summary>
        /// Constructor.   
        /// </summary>
        /// <param name="trackingId">trackingId</param>
        /// <param name="origin">origin</param>
        /// <param name="finalDestination">finalDestination</param>
        /// <param name="arrivalDeadline">arrivalDeadline</param>
        /// <param name="misrouted">misrouted</param>
        public CargoRoutingDTO(string trackingId, string origin, string finalDestination, DateTime arrivalDeadline,
                               bool misrouted)
        {
            this.trackingId = trackingId;
            this.origin = origin;
            this.finalDestination = finalDestination;
            this.arrivalDeadline = arrivalDeadline;
            this.misrouted = misrouted;
            legs = new List<LegDTO>();
        }

        public string TrackingId
        {
            get { return trackingId; }
        }

        public string Origin
        {
            get { return origin; }
        }

        public string FinalDestination
        {
            get { return finalDestination; }
        }

        public void AddLeg(string voyageNumber, string from, string to, DateTime loadTime, DateTime unloadTime)
        {
            legs.Add(new LegDTO(voyageNumber, from, to, loadTime, unloadTime));
        }


        public IList<LegDTO> Legs
        {
            get { return new List<LegDTO>(legs).AsReadOnly(); }
        }

        public bool IsMisrouted
        {
            get { return misrouted; }
        }

        public bool IsRouted
        {
            get { return !legs.IsEmpty(); }
        }

        public DateTime ArrivalDeadline
        {
            get { return arrivalDeadline; }
        }
    }   
}