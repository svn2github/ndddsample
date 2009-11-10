namespace NDDDSample.Interfaces.PathfinderRemoteService.Common
{
    #region Usings

    using System;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    /// Represents an edge in a path through a graph,
    /// describing the route of a cargo.
    /// </summary>
    [DataContract]
    public class TransitEdge
    {
        [DataMember] private string voyageNumber;
        [DataMember] private string fromUnLocode;
        [DataMember] private string toUnLocode;
        [DataMember] private DateTime fromDate;
        [DataMember] private DateTime toDate;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="voyageNumber">voyageNumber</param>
        /// <param name="fromUnLocode">fromUnLocode</param>
        /// <param name="toUnLocode">toUnLocode</param>
        /// <param name="fromDate">fromDate</param>
        /// <param name="toDate">toDate</param>
        public TransitEdge(string voyageNumber, string fromUnLocode, string toUnLocode, DateTime fromDate,
                           DateTime toDate)
        {
            this.voyageNumber = voyageNumber;
            this.fromUnLocode = fromUnLocode;
            this.toUnLocode = toUnLocode;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }


        public string VoyageNumber
        {
            get { return voyageNumber; }
        }

        public string FromUnLocode
        {
            get { return fromUnLocode; }
        }

        public string ToUnLocode
        {
            get { return toUnLocode; }
        }

        public DateTime FromDate
        {
            get { return fromDate; }
        }

        public DateTime ToDate
        {
            get { return toDate; }
        }
    }
}