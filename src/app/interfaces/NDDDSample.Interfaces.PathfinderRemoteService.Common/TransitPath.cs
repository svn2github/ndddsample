namespace NDDDSample.Interfaces.PathfinderRemoteService.Common
{
    #region Usings

    using System.Collections.Generic;
    using System.Runtime.Serialization;

    #endregion

    [DataContract]
    public class TransitPath
    {
        [DataMember] private IList<TransitEdge> transitEdges;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transitEdges">The legs for this itinerary.</param>
        public TransitPath(IList<TransitEdge> transitEdges)
        {
            this.transitEdges = transitEdges;
        }

        /// <summary>
        /// An unmodifiable list DTOs.
        /// </summary>
        public IList<TransitEdge> TransitEdges
        {
            get { return transitEdges; }
        }
    }
}