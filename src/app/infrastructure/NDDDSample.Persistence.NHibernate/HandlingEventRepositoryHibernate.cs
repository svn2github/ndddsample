namespace NDDDSample.Persistence.NHibernate
{
    #region Usings

    using Domain.Model.Cargos;
    using Domain.Model.Handlings;

    #endregion

    /// <summary>
    /// Hibernate implementation of HandlingEventRepository.
    /// </summary>
    public sealed class HandlingEventRepositoryHibernate : HibernateRepository, IHandlingEventRepository
    {
        #region IHandlingEventRepository Members

        public void Store(HandlingEvent evnt)
        {
            getSession().Save(evnt);
        }


        public HandlingHistory LookupHandlingHistoryOfCargo(TrackingId trackingId)
        {
            return new HandlingHistory(getSession().CreateQuery(
                                           "from HandlingEvent where cargo.trackingId = :tid").
                                           SetParameter("tid", trackingId).
                                           List<HandlingEvent>());
        }

        #endregion
    }
}