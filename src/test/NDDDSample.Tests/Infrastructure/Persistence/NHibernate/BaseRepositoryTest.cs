namespace NDDDSample.Tests.Infrastructure.Persistence.NHibernate
{
    #region Usings

    using Application.Utils;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Persistence.NHibernate;
    using NUnit.Framework;
    using Rhino.Commons.ForTesting;

    #endregion

    public class BaseRepositoryTest : DatabaseTestFixtureBase
    {
        [SetUp]
        public virtual void SetUp()
        {
            MappingInfo from = MappingInfo.From(typeof (Cargo).Assembly, typeof (HibernateRepository<>).Assembly);
            IntializeNHibernateAndIoC(PersistenceFramwork, RhinoContainerConfig, DatabaseEngine.SQLite, from);
           
            CurrentContext.CreateUnitOfWork();
            LoadData();
        }

        private static string RhinoContainerConfig
        {
            get { return "nh-windsor.boo"; }
        }

        private static PersistenceFramework PersistenceFramwork
        {
            get { return PersistenceFramework.NHibernate; }
        }

        [TearDown]
        public void TearDown()
        {
            CurrentContext.DisposeUnitOfWork();
        }

        protected static void LoadData()
        {
            // TODO store Sample* and object instances here instead of handwritten SQL
            SampleDataGenerator.LoadSampleData();          
        }

    }
}