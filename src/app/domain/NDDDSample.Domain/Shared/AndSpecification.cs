namespace NDDDSample.Domain.Shared
{
    /// <summary>
    /// AND specification, used to create a new specifcation that is the AND of two other specifications.
    /// </summary>
    /// <typeparam name="T">Object Type to test</typeparam>
    public class AndSpecification<T> : AbstractSpecification<T>
    {
        private readonly ISpecification<T> spec1;
        private readonly ISpecification<T> spec2;

        /// <summary>
        /// Create a new AND specification based on two other spec.
        /// </summary>
        /// <param name="spec1">Specification one.</param>
        /// <param name="spec2">Specification two.</param>
        public AndSpecification(ISpecification<T> spec1, ISpecification<T> spec2)
        {
            this.spec1 = spec1;
            this.spec2 = spec2;
        }

        #region Overrides of AbstractSpecification<T>

        /// <summary>
        /// Check if t is satisfied by the specification.
        /// </summary>
        /// <param name="t">Object to test.</param>
        /// <returns>true if t satisfies the specification.</returns>
        public override bool IsSatisfiedBy(T t)
        {
            return spec1.IsSatisfiedBy(t) && spec2.IsSatisfiedBy(t);
        }

        #endregion
    }
}