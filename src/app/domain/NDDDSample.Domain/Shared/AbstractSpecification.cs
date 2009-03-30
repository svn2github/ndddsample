namespace NDDDSample.Domain.Shared
{
    /// <summary>
    ///Abstract base implementation of composite Specification with default
    ///implementations for and, or and not Specification operations.
    /// </summary>
    /// <typeparam name="T">Object Type to test</typeparam>
    public abstract class AbstractSpecification<T> : ISpecification<T>
    {
        #region ISpecification<T> Members

        /// <summary>
        /// Check if t is satisfied by the specification.
        /// </summary>
        /// <param name="t">Object to test.</param>
        /// <returns>true if t satisfies the specification.</returns>
        public abstract bool IsSatisfiedBy(T t);

        /// <summary>
        /// Create a new specification that is the AND operation of this specification and another specification.
        /// </summary>
        /// <param name="specification">Specification to AND.</param>
        /// <returns>A new specification.</returns>
        public ISpecification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        /// <summary>
        /// Create a new specification that is the OR operation of this specification and another specification.
        /// </summary>
        /// <param name="specification">Specification to OR.</param>
        /// <returns>A new specification.</returns>
        public ISpecification<T> Or(ISpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        /// <summary>
        /// Create a new specification that is the NOT operation of this specification.
        /// </summary>
        /// <param name="specification">Specification to NOT.</param>
        /// <returns>A new specification.</returns>
        public ISpecification<T> Not(ISpecification<T> specification)
        {
            return new NotSpecification<T>(specification);
        }

        #endregion
    }
}