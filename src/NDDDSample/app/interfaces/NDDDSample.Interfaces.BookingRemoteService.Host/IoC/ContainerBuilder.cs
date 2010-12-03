namespace NDDDSample.Interfaces.BookingRemoteService.Host.IoC
{
    #region Usings

    using System;
    using System.ServiceModel;
    using Castle.Facilities.WcfIntegration;
    using Castle.Facilities.WcfIntegration.Behaviors;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Configuration.Interpreters;
    using Common;
    using Infrastructure.Utils;

    using NDDDSample.Interfaces.PathfinderRemoteService.Common;

    using Rhino.Commons;
    using Wcf;

    #endregion

    public static class ContainerBuilder
    {
        private static string bookingRemoteServiceWorkerRoleEndpoint = "localhost:8081";        
        private static string pathfinderRemoteServiceWorkerRoleEndpoint = "localhost:8082";

        public static IWindsorContainer Build(string bookingEndpoint, string pathfinderEndpoint)
        {
            bookingRemoteServiceWorkerRoleEndpoint = bookingEndpoint;
            pathfinderRemoteServiceWorkerRoleEndpoint = pathfinderEndpoint;
            return Build();
        }

        public static IWindsorContainer Build()
        {
            var container = new WindsorContainer(new XmlInterpreter("Windsor.config"));
            //Init components and services
            RegisterComponents(container);
            // For Rhino Commons
            IoC.Initialize(container);

            return container;
        }

        private static void RegisterComponents(IWindsorContainer container)
        {
            container.Register(
                AllTypes.Pick()
                    //Scan repository assembly for domain model interfaces implementation
                    .FromAssemblyNamed("NDDDSample.Persistence.NHibernate")
                    .WithService.FirstNonGenericCoreInterface("NDDDSample.Domain.Model"));

            container.AddComponent("bookingInterface",
                                   Type.GetType("NDDDSample.Application.IBookingService, NDDDSample.Application"),
                                   Type.GetType("NDDDSample.Application.Impl.BookingService, NDDDSample.Application"));
           
            container.AddComponent("routingService",                                   
                                   Type.GetType("NDDDSample.Domain.Service.IRoutingService, NDDDSample.Domain"),
                                   Type.GetType("NDDDSample.Infrastructure.ExternalRouting.ExternalRoutingService, NDDDSample.Infrastructure.ExternalRouting"));

            container.AddFacility<WcfFacility>();

            container.Register(
                Component.For<MessageLifecycleBehavior>(),
                Component.For<UnitOfWorkBehavior>(),
                Component
                    .For<IBookingServiceFacade>()
                    .ImplementedBy<BookingServiceFacade>()
                    .Named("BookingService")
                    .LifeStyle.Transient
                    .ActAs(new DefaultServiceModel()
                               .AddEndpoints(WcfEndpoint
                                                 .BoundTo(new NetTcpBinding())
                                                 //.At("net.tcp://localhost:8081/BookingServiceFacade")
                                                 .At(String.Format("net.tcp://{0}/BookingServiceFacade", bookingRemoteServiceWorkerRoleEndpoint))
                                                 // adds this message action to this endpoint
                                                 .AddExtensions(new LifestyleMessageAction()
                                                 )
                               ))
                );

            container.Register(
                Component
                    .For<IGraphTraversalService>()                
                    .Named("pathfinderRemoteFacade")
                    .LifeStyle.Transient
                    .ActAs(DefaultClientModel
                    .On(WcfEndpoint.BoundTo(new NetTcpBinding())
                        .At(String.Format("net.tcp://{0}/GraphTraversalService", pathfinderRemoteServiceWorkerRoleEndpoint))
                        ))
                        .LifeStyle.Transient);
        }
    }
}