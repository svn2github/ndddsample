namespace NDDDSample.Interfaces.BookingRemoteService.Assembler
{
    using Common.Dto;
    using Domain.Model.Cargos;

    /// <summary>
    /// Assembler class for the CargoRoutingDTO.
    /// TODO:atrosin revise later use of an automaper framework
    /// </summary>
    public class CargoRoutingDTOAssembler
    {   
        /// <summary>
        ///  Assemble a Cargo Routing DTO from provided Cargo.   
        /// </summary>
        /// <param name="cargo">cargo</param>
        /// <returns>A cargo routing DTO</returns> 
        public CargoRoutingDTO ToDTO(Cargo cargo)
        {
            var dto = new CargoRoutingDTO(
                cargo.TrackingId.IdString,
                cargo.Origin.UnLocode.IdString,
                cargo.RouteSpecification.Destination.UnLocode.IdString,
                cargo.RouteSpecification.ArrivalDeadline,
                cargo.Delivery.RoutingStatus.SameValueAs(RoutingStatus.MISROUTED));

            foreach (Leg leg in cargo.Itinerary.Legs)
            {
                dto.AddLeg(
                    leg.Voyage.VoyageNumber.IdString,
                    leg.LoadLocation.UnLocode.IdString,
                    leg.UnloadLocation.UnLocode.IdString,
                    leg.LoadTime,
                    leg.UnloadTime);
            }
            return dto;
        }
    }
}