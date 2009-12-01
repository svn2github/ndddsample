namespace NDDDSample.Interfaces.BookingRemoteService.Assembler
{
    using System.Collections.Generic;
    using Common.Dto;
    using Domain.Model.Cargos;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;

    /// <summary>
    /// Assembler class for the ItineraryCandidateDTOAssembler.
    /// TODO:atrosin revise later use of an automaper framework
    /// </summary>
    public class ItineraryCandidateDTOAssembler
    {
        /// <summary>
        /// Assemble a Route Candidate DTO from provided Itinerary. 
        /// </summary>
        /// <param name="itinerary">itinerary</param>
        /// <returns>A route candidate DTO</returns>
        public RouteCandidateDTO ToDTO(Itinerary itinerary)
        {
            IList<LegDTO> legDTOs = new List<LegDTO>(itinerary.Legs.Count);
            foreach (Leg leg in itinerary.Legs)
            {
                legDTOs.Add(ToLegDTO(leg));
            }
            return new RouteCandidateDTO(legDTOs);
        }

        /// <summary>
        /// Assemble a Itinerary from provided RouteCandidateDTO. 
        /// </summary>
        /// <param name="routeCandidateDTO">route candidate DTO</param>
        /// <param name="voyageRepository">voyage repository</param>
        /// <param name="locationRepository">location repository</param>
        /// <returns>An itinerary</returns>
        public Itinerary FromDTO(RouteCandidateDTO routeCandidateDTO,
                                 IVoyageRepository voyageRepository,
                                 ILocationRepository locationRepository)
        {
            var legs = new List<Leg>(routeCandidateDTO.Legs.Count);

            foreach (LegDTO legDTO in routeCandidateDTO.Legs)
            {
                VoyageNumber voyageNumber = new VoyageNumber(legDTO.VoyageNumber);
                Voyage voyage = voyageRepository.Find(voyageNumber);
                Location from = locationRepository.Find(new UnLocode(legDTO.FromLocation));
                Location to = locationRepository.Find(new UnLocode(legDTO.ToLocation));
                legs.Add(new Leg(voyage, from, to, legDTO.LoadTime, legDTO.UnloadTime));
            }

            return new Itinerary(legs);
        }

        protected static LegDTO ToLegDTO(Leg leg)
        {
            VoyageNumber voyageNumber = leg.Voyage.VoyageNumber;
            UnLocode from = leg.LoadLocation.UnLocode;
            UnLocode to = leg.UnloadLocation.UnLocode;
            return new LegDTO(voyageNumber.IdString, from.IdString, to.IdString, leg.LoadTime, leg.UnloadTime);
        }
    }
}