namespace NDDDSample.Persistence.NHibernate
{
    #region Usings

    using Domain.Model.Cargos;
    using Domain.Model.Handlings;

    #endregion

    /// <summary>
    /// Hibernate implementation of HandlingEventRepository.
    /// </summary>
    public sealed class HandlingEventRepositoryHibernate : HibernateRepository<HandlingEvent>, IHandlingEventRepository
    {
        #region IHandlingEventRepository Members

        public void Store(HandlingEvent evnt)
        {
            Session.Save(evnt);
        }


        public HandlingHistory LookupHandlingHistoryOfCargo(TrackingId trackingId)
        {
            return new HandlingHistory(Session.CreateQuery(
                                           "from HandlingEvent as h where h.cargo.trackingId.id = :tid").
                                           SetParameter("tid", trackingId.IdString).
                                           List<HandlingEvent>());
        }

        #endregion
    }
}