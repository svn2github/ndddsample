namespace NDDDSample.Persistence.NHibernate
{
    #region Usings

    using System.Collections.Generic;
    using Domain.Model.Locations;

    #endregion

    public sealed class LocationRepositoryHibernate : HibernateRepository, ILocationRepository
    {
        #region ILocationRepository Members

        public Location Find(UnLocode unLocode)
        {
            return (Location) getSession().
                                  CreateQuery("from Location where unLocode = ?").
                                  SetParameter(0, unLocode).
                                  UniqueResult();
        }

        public IList<Location> FindAll()
        {
            return getSession().CreateQuery("from Location").List<Location>();
        }

        #endregion
    }
}