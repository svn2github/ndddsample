namespace NDDDSample.Interfaces.HandlingService.Host.IoC
{
    #region Usings

    using System;
    using System.ServiceModel;
    using Castle.Facilities.WcfIntegration;
    using Castle.Facilities.WcfIntegration.Behaviors;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Configuration.Interpreters;
    using Infrastructure.Messaging;
    using Infrastructure.Utils;
    using Messaging;
    using Wcf;
    using WebService;

    #endregion

    public static class ContainerBuilder
    {
        public static IWindsorContainer Build()
        {
            var container = new WindsorContainer(new XmlInterpreter("Windsor.config"));
            //Init components and services
            RegisterComponents(container);
            // For Rhino Commons
            Rhino.Commons.IoC.Initialize(container);

            //Register messages and handlers
            new MessageHandlerRegister(container, typeof(CargoHandledMessage).Assembly, typeof(CargoHandledHandler).Assembly);

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


            container //.AddFacility<WcfFacility>() Note: commented because it
                //Note: is registered in windsor config already
                .Register(
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
                                                 .At("http://localhost:8088/HandlingReportServiceFacade")                                                 
                                                 // adds this message action to this endpoint
                                                 .AddExtensions(new LifestyleMessageAction()
                                                 )
                               ))
                );
        }
    }
}