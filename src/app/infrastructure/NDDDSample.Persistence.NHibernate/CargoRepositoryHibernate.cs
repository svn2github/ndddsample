namespace NDDDSample.Persistence.NHibernate
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Domain.Model.Cargos;

    #endregion

    /// <summary>
    /// Hibernate implementation of CargoRepository.
    /// </summary>
    public sealed class CargoRepositoryHibernate : HibernateRepository, ICargoRepository
    {
        #region ICargoRepository Members

        public Cargo Find(TrackingId tid)
        {
            return (Cargo) getSession().
                               CreateQuery("from Cargo where trackingId = :tid").
                               SetParameter("tid", tid).
                               UniqueResult();
        }

        public void Store(Cargo cargo)
        {
            getSession().SaveOrUpdate(cargo);
            // Delete-orphan does not seem to work correctly when the parent is a component
            getSession().CreateSQLQuery("delete from Leg where cargo_id = null").ExecuteUpdate();
        }

        public TrackingId NextTrackingId()
        {
            // TODO use an actual DB sequence here, UUID is for in-mem
            string random = Guid.NewGuid().ToString().ToUpper();
            return new TrackingId(
                random.Substring(0, random.IndexOf("-"))
                );
        }

        public IList<Cargo> FindAll()
        {
            return getSession().CreateQuery("from Cargo").List<Cargo>();
        }

        #endregion
    }
}