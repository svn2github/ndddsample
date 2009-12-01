namespace NDDDSample.Web.Initializers
{
    #region Usings

    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;
    using Persistence.NHibernate.Utils;
    using Rhino.Commons;

    #endregion

    public static class NhInitializer
    {
        public static void Init()
        {
            Configuration configuration = new Configuration().Configure();

            using (UnitOfWork.Start())
            {
                new SchemaExport(configuration)
                    .Execute(false, true, false, true, UnitOfWork.CurrentSession.Connection, null);
                SampleDataGenerator.LoadSampleData();
            }
        }
    }
}