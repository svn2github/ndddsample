namespace NDDDSample.Persistence.NHibernate
{
    #region Usings

    using Domain.Model.Voyages;

    #endregion

    /// <summary>
    /// Hibernate implementation of CarrierMovementRepository.
    /// </summary>
    public sealed class VoyageRepositoryHibernate : HibernateRepository<Voyage>, IVoyageRepository
    {
        #region IVoyageRepository Members

        public Voyage Find(VoyageNumber voyageNumber)
        {
            return (Voyage) Session.CreateQuery("from Voyage as v where v.voyageNumber.number = :vn").
                                SetParameter("vn", voyageNumber.IdString).
                                UniqueResult();
        }

        #endregion
    }
}