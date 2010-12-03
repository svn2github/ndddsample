namespace NDDDSample.Web.Initializers
{
    #region Usings

    using System;
    using System.ServiceModel;

    using Castle.Facilities.WcfIntegration;
    using Castle.Facilities.WcfIntegration.Behaviors;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NDDDSample.Infrastructure.Utils;
    using NDDDSample.Interfaces.BookingRemoteService.Common;

    #endregion

    public static class ComponentRegistrar
    {
        private static string bookingRemoteServiceWorkerRoleEndpoint = "localhost:8081";

        public static void AddComponentsTo(IWindsorContainer container, string bookingEndpoint)
        {
            bookingRemoteServiceWorkerRoleEndpoint = bookingEndpoint;            
            AddComponentsTo(container);
        }

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

            container.AddFacility<WcfFacility>();

            container.Register(
                Component.For<MessageLifecycleBehavior>(),                
                Component.For<IBookingServiceFacade>()                
                    .Named("bookingServiceFacade")
                    .LifeStyle.Transient
                .ActAs(DefaultClientModel
                    .On(WcfEndpoint.BoundTo(new NetTcpBinding())
                        .At(String.Format("net.tcp://{0}/BookingServiceFacade", bookingRemoteServiceWorkerRoleEndpoint))
                        )));            
        }
    }
}