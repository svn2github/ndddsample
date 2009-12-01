namespace NDDDSample.Tests.Infrastructure.Persistence.Inmemory
{
    #region Usings

    using NDDDSample.Domain.Model.Voyages;

    #endregion

    public class VoyageRepositoryInMem : IVoyageRepository
    {
        #region IVoyageRepository Members

        public Voyage Find(VoyageNumber voyageNumber)
        {
            return SampleVoyages.Lookup(voyageNumber);
        }

        #endregion
    }
}