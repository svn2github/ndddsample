#region Usings



#endregion

namespace NDDDSample.Persistence.NHibernate
{
    #region Usings

    using Rhino.Commons;

    #endregion

    /// <summary>
    /// Functionality common to all Hibernate repositories.
    /// </summary>
    public abstract class HibernateRepository<T> : NHRepository<T> {}
}