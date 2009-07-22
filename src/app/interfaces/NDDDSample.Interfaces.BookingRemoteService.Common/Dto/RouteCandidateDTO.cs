namespace NDDDSample.Interfaces.BookingRemoteService.Common.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// DTO for presenting and selecting an itinerary from a collection of candidates.
    /// </summary>
    [Serializable]
    [DataContract]
    public class RouteCandidateDTO
    {
        private readonly IList<LegDTO> legs;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="legs">The legs for this itinerary.</param>
        public RouteCandidateDTO(IList<LegDTO> legs)
        {
            this.legs = legs;
        }

        /// <summary>
        /// An unmodifiable list DTOs.
        /// </summary>
        /// <returns></returns>
        public IList<LegDTO> Legs
        {
            get { return new List<LegDTO>(legs).AsReadOnly(); }
        }
    }
}