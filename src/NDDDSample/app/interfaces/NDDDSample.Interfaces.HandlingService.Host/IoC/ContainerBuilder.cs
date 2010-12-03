namespace NDDDSample.Interfaces.HandlingService.Host.IoC
{
    #region Usings

    using System;
    using System.ServiceModel;
    using Application;
    using Application.Impl;
    using Castle.Facilities.WcfIntegration;
    using Castle.Facilities.WcfIntegration.Behaviors;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Configuration.Interpreters;
    using Domain.Model.Handlings;
    using Infrastructure.Messaging;
    using Infrastructure.Utils;
    using Messaging;
    using Wcf;
    using WebService;

    #endregion

    public static class ContainerBuilder
    {
        private static string handlingServiceWorkerRoleEndpoint = "localhost:8089";

        public static IWindsorContainer Build(string endPoint)
        {
            handlingServiceWorkerRoleEndpoint = endPoint;
            return Build();
        }

        public static IWindsorContainer Build()
        {
            var container = new WindsorContainer(new XmlInterpreter("Windsor.config"));
            //Init components and services
            RegisterComponents(container);
            // For Rhino Commons
            Rhino.Commons.IoC.Initialize(container);

            //Register messages and handlers
            new MessageHandlerRegister(container, typeof (CargoHandledMessage).Assembly,
                                       typeof (CargoHandledHandler).Assembly);

            return container;
        }

        private static void RegisterComponents(IWindsorContainer container)
        {
            container.Register(
                AllTypes.Pick()
                    //Scan repository assembly for domain model interfaces implementation
                    .FromAssemblyNamed("NDDDSample.Persistence.NHibernate")
                    .WithService.FirstNonGenericCoreInterface("NDDDSample.Domain.Model"));

            container.Register(Component.For<IMessageBus>()
                                   .ImplementedBy<MessageBus>()
                                   .LifeStyle.Singleton);

            container.Register(Component.For<IWindsorContainer>()
                                   .Instance(container));

            container.Register(Component.For<IApplicationEvents>()
                                   .ImplementedBy<EsbApplicationEventsImpl>());

            container.Register(Component.For<IHandlingEventService>()
                                   .ImplementedBy<HandlingEventService>());

            container.Register(Component.For<HandlingEventFactory>()
                                   .ImplementedBy<HandlingEventFactory>());

            container.Register(Component.For<ICargoInspectionService>()
                                   .ImplementedBy<CargoInspectionService>());

            container.AddFacility<WcfFacility>();

            container.Register(
                Component.For<MessageLifecycleBehavior>(),
                Component.For<UnitOfWorkBehavior>(),
                Component
                    .For<IHandlingReportService>()
                    .ImplementedBy<HandlingReportService>()
                    .Named("HandlingReportService")
                    .LifeStyle.Transient
                    .ActAs(new DefaultServiceModel()
                               .AddEndpoints(WcfEndpoint
                                                 .BoundTo(new BasicHttpBinding())
                                                 //.At("http://localhost:8089/HandlingReportServiceFacade")
                                                 .At(new Uri(String.Format("http://{0}/HandlingReportServiceFacade", handlingServiceWorkerRoleEndpoint)))
                                                 // adds this message action to this endpoint
                                                 .AddExtensions(new LifestyleMessageAction()
                                                 )
                               ))
                );
        }
    }
}