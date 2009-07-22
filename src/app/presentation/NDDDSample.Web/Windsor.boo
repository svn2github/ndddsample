import NDDDSample.Web
#import NDDDSample.Web.Initialize

# Rhino Commons
Component("nhibernate.repository", IRepository, NHRepository)
Component("nhibernate.unit-of-work.factory",
          IUnitOfWorkFactory,
          NHibernateUnitOfWorkFactory,
          configurationFileName: "hibernate.cfg.xml")
          
#component INHibernateInitializationAware, NHibernateDbSchemaGeneration          