namespace NDDDSample.Tests.Infrastructure.Persistence.NHibernate
{
    #region Usings

    using System.Reflection;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Persistence.NHibernate;
    using NDDDSample.Persistence.NHibernate.Utils;
    using NUnit.Framework;
    using Rhino.Commons;
    using Rhino.Commons.ForTesting;

    #endregion

    [TestFixture, Category(UnitTestCategories.Infrastructure)]
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

        private static void LoadData()
        {
            // TODO store Sample* and object instances here instead of handwritten SQL
            SampleDataGenerator.LoadTestSampleData();
        }

        protected static void Flush()
        {
            UnitOfWork.CurrentSession.Flush();
        }

        // Instead of exposing a getId() on persistent classes
        protected static int GetIntId(object o)
        {
            if (UnitOfWork.CurrentSession.Contains(o))
            {
                return (int) UnitOfWork.CurrentSession.GetIdentifier(o);
            }

            FieldInfo id = o.GetType().GetField("id");
            return (int) id.GetValue(o);
        }
    }
}