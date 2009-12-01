namespace NDDDSample.Domain.Shared
{
    /// <summary>
    /// Specificaiton interface.     
    /// Use AbstractSpecification as base for creating specifications, and
    /// only the method IsSatisfiedBy(T t) must be implemented.
    /// </summary>
    /// <typeparam name="T">Object Type to test</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Check if t is satisfied by the specification.
        /// </summary>
        /// <param name="t">Object to test.</param>
        /// <returns>true if t satisfies the specification.</returns>
        bool IsSatisfiedBy(T t);

        /// <summary>
        /// Create a new specification that is the AND operation of this specification and another specification.
        /// </summary>
        /// <param name="specification">Specification to AND.</param>
        /// <returns>A new specification.</returns>
        ISpecification<T> And(ISpecification<T> specification);

        /// <summary>
        /// Create a new specification that is the OR operation of this specification and another specification.
        /// </summary>
        /// <param name="specification">Specification to OR.</param>
        /// <returns>A new specification.</returns>
        ISpecification<T> Or(ISpecification<T> specification);

        /// <summary>
        /// Create a new specification that is the NOT operation of this specification.
        /// </summary>
        /// <param name="specification">Specification to NOT.</param>
        /// <returns>A new specification.</returns>
        ISpecification<T> Not(ISpecification<T> specification);
    }
}