namespace NDDDSample.Domain.Model.Voyages
{
    public interface IVoyageRepository
    {
        /// <summary>
        /// Finds a voyage using voyage number.
        /// </summary>
        /// <param name="voyageNumber">voyage number</param>
        /// <returns> The voyage, or null if not found.</returns>
        Voyage Find(VoyageNumber voyageNumber);
    }
}