namespace NDDDSample.Tests.Infrastructure.Persistence.Inmemory
{
    #region Usings

    using System.Collections.Generic;
    using NDDDSample.Domain.Model.Locations;

    #endregion

    public class LocationRepositoryInMem : ILocationRepository
    {
        #region ILocationRepository Members

        public Location Find(UnLocode unLocode)
        {
            foreach (Location location in SampleLocations.GetAll())
            {
                if (location.UnLocode.Equals(unLocode))
                {
                    return location;
                }
            }
            return null;
        }

        public IList<Location> FindAll()
        {
            return SampleLocations.GetAll();
        }

        #endregion
    }
}