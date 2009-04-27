namespace NDDDSample.Interfaces.Bookings.Facade.Internal.Assembler
{
    #region Usings

    using System.Collections.Generic;
    using Domain.Model.Locations;
    using Dto;

    #endregion

    /// <summary>
    /// Assembler class for the LocationDTOAssembler.
    /// TODO:atrosin revise later use of an automaper framework
    /// </summary>
    public class LocationDTOAssembler
    {
        public LocationDTO ToDTO(Location location)
        {
            return new LocationDTO(location.UnLocode.IdString, location.Name);
        }

        public IList<LocationDTO> ToDTOList(IList<Location> allLocations)
        {
            IList<LocationDTO> dtoList = new List<LocationDTO>(allLocations.Count);
            foreach (Location location in allLocations)
            {
                dtoList.Add(ToDTO(location));
            }
            return dtoList;
        }
    }
}