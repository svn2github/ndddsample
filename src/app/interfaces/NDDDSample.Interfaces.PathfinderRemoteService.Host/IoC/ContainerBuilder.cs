namespace NDDDSample.Interfaces.PathfinderRemoteService.Host.IoC
{
    #region Usings

    using System;
    using System.ServiceModel;
    using Castle.Facilities.WcfIntegration;
    using Castle.Facilities.WcfIntegration.Behaviors;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Common;
    using Wcf;

    #endregion

    public static class ContainerBuilder
    {
        public static IWindsorContainer Build()
        {
            var container = new WindsorContainer();
            //Init components and services
            RegisterComponents(container);
            
            return container;
        }

        private static void RegisterComponents(IWindsorContainer container)
        {
            container.AddComponent("graphDAO", typeof(GraphDAO), typeof(GraphDAO));
     
            container.AddFacility<WcfFacility>()
                .Register(
                Component.For<MessageLifecycleBehavior>(),               
                Component
                    .For<IGraphTraversalService>()
                    .ImplementedBy<GraphTraversalService>()
                    .Named("GraphTraversalService")
                    .LifeStyle.Transient
                    .ActAs(new DefaultServiceModel()
                               .AddEndpoints(WcfEndpoint
                                                 .BoundTo(new NetTcpBinding())
                                                 .At("net.tcp://localhost:8082/GraphTraversalService")
                                                 // adds this message action to this endpoint
                                                 .AddExtensions(new LifestyleMessageAction()
                                                 )
                               ))
                );
        }
    }
}