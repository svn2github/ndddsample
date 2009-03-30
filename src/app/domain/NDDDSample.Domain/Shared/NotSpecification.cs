namespace NDDDSample.Domain.Shared
{
    /// <summary>
    /// NOT decorator, used to create a new specifcation that is the inverse (NOT) of the given spec.
    /// </summary>
    /// <typeparam name="T">Object Type to test</typeparam>
    public class NotSpecification<T> : AbstractSpecification<T>
    {
        private readonly ISpecification<T> spec1;

        /// <summary>
        /// Create a new NOT specification based on another spec.
        /// </summary>
        /// <param name="spec1">Specification instance to not.</param>
        public NotSpecification(ISpecification<T> spec1)
        {
            this.spec1 = spec1;
        }

        #region Overrides of AbstractSpecification<T>

        /// <summary>
        /// Check if t is satisfied by the specification.
        /// </summary>
        /// <param name="t">Object to test.</param>
        /// <returns>true if t satisfies the specification.</returns>
        public override bool IsSatisfiedBy(T t)
        {
            return !spec1.IsSatisfiedBy(t);
        }

        #endregion
    }
}