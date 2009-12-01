namespace NDDDSample.Domain.Model.Handlings
{
    #region Usings

    using Cargos;

    #endregion

    public interface IHandlingEventRepository
    {
        /// <summary>
        /// Handling event repository.
        /// </summary>
        /// <param name="handlingEvent">Stores a (new) handling event.</param>
        void Store(HandlingEvent handlingEvent);

        /// <summary>
        /// Lookup Handling History Of Cargo
        /// </summary>
        /// <param name="trackingId"> trackingId cargo tracking id</param>
        /// <returns>The handling history of this cargo</returns>
        HandlingHistory LookupHandlingHistoryOfCargo(TrackingId trackingId);
    }
}