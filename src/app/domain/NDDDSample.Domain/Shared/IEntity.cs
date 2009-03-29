namespace NDDDSample.Domain.Shared
{
    #region Usings

    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    /// An entity, as explained in the DDD book.
    /// </summary>
    public interface IEntity<T> : ISerializable
    {
        /// <summary>
        /// Entities compare by identity, not by attributes.
        /// </summary>
        /// <param name="other">The other entity.</param>
        /// <returns>true if the identities are the same, regardles of other attributes.</returns>
        bool sameIdentityAs(T other);
    }
}