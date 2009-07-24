namespace NDDDSample.Web.Initializers
{
    #region Usings

    using System;
    using Castle.Facilities.WcfIntegration;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using Infrastructure.Utils;
    using Interfaces.BookingRemoteService.Common;

    #endregion

    public static class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddCustomRepositoriesTo(container);
        }

        private static void AddCustomRepositoriesTo(IWindsorContainer container)
        {
            container.Register(
                AllTypes.Pick()
                    //Scan repository assembly for domain model interfaces implementation
                    .FromAssemblyNamed("NDDDSample.Persistence.NHibernate")
                    .WithService.FirstNonGenericCoreInterface("NDDDSample.Domain.Model"));
        }
    }
}