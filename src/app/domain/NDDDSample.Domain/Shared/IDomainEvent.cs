namespace NDDDSample.Domain.Shared
{
    /// <summary>
    ///A domain event is something that is unique, but does not have a lifecycle.
    ///The identity may be explicit, for example the sequence number of a payment,
    /// or it could be derived from various aspects of the event such as where, when and what
    /// has happened.
    /// </summary>
    /// <typeparam name="T">Event type</typeparam>
    public interface IDomainEvent<T>
    {
        /// <summary>
        /// Compare two events.
        /// </summary>
        /// <param name="other">The other domain event.</param>
        /// <returns>true if the given domain event and this event are regarded as being the same event.</returns>
        bool SameEventAs(T other);
    }
}