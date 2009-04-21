#region Usings

using ISession=NHibernate.ISession;
using ISessionFactory=NHibernate.ISessionFactory;

#endregion

namespace NDDDSample.Persistence.NHibernate
{
    using Rhino.Commons;

    /// <summary>
    /// Functionality common to all Hibernate repositories.
    /// </summary>
    public abstract class HibernateRepository <T>: NHRepository<T>
    {        
    }
}