namespace NDDDSample.Persistence.NHibernate
{
    #region Usings

    using System.Collections.Generic;
    using Domain.Model.Locations;

    #endregion

    public sealed class LocationRepositoryHibernate : HibernateRepository<UnLocode>, ILocationRepository
    {
        #region ILocationRepository Members

        public Location Find(UnLocode unLocode)
        {
            return (Location) Session.
                                  CreateQuery("from Location where unLocode = ?").
                                  SetParameter(0, unLocode.IdString).
                                  UniqueResult();
        }

        public IList<Location> FindAll()
        {
            return Session.CreateQuery("from Location").List<Location>();
        }

        #endregion
    }
}