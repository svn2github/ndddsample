namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using System.Collections.Generic;

    #endregion

    public interface ICargoRepository
    {
        /// <summary>
        /// Finds a cargo using given id.
        /// </summary>
        /// <param name="trackingId">trackingId Id</param>
        /// <returns>Cargo if found, else null</returns>
        Cargo Find(TrackingId trackingId);

        /// <summary>
        /// Finds all cargo.
        /// </summary>
        /// <returns>All cargo.</returns>
        IList<Cargo> FindAll();

        /// <summary>
        /// Saves given cargo.
        /// </summary>
        /// <param name="cargo">cargo to save</param>
        void Store(Cargo cargo);

        /// <summary>
        /// Get A unique, generated tracking Id.
        /// </summary>
        /// <returns>tracking Id</returns>
        TrackingId NextTrackingId();
    }
}