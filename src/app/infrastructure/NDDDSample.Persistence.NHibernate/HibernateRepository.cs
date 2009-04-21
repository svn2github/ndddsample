#region Usings

using ISession=NHibernate.ISession;
using ISessionFactory=NHibernate.ISessionFactory;

#endregion

namespace NDDDSample.Persistence.NHibernate
{
    /// <summary>
    /// Functionality common to all Hibernate repositories.
    /// </summary>
    public abstract class HibernateRepository
    {
        private ISessionFactory sessionFactory;

        public void setSessionFactory(ISessionFactory sessFactory)
        {
            sessionFactory = sessFactory;
        }

        protected ISession getSession()
        {
            return sessionFactory.GetCurrentSession();
        }
    }
}