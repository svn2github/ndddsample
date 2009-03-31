namespace NDDDSample.Domain.Model.Locations
{
    #region Usings

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Location repository interface
    /// </summary>
    public interface ILocationRepository
    {
        /// <summary>
        /// Finds a location using given unlocode.
        /// </summary>
        /// <param name="unLocode">UNLocode</param>
        /// <returns>Location</returns>
        Location Find(UnLocode unLocode);

        /// <summary>
        /// Finds all locations.
        /// </summary>
        /// <returns>All locations.</returns>
        IList<Location> FindAll();
    }
}