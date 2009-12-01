namespace NDDDSample.Web
{
    #region Usings

    using System;
    using System.Web.Mvc;
    using System.Web.Routing;  
    using Controllers;
    using Initializers;    
    using MvcContrib.Castle;

    #endregion

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : Rhino.Commons.HttpModules.UnitOfWorkApplication
    {
        public override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);

            log4net.Config.XmlConfigurator.Configure();
            InitializeServiceLocator();
            RegisterRoutes(RouteTable.Routes);
            NhInitializer.Init();
        }

        /// <summary>
        /// Instantiate the container and add all Controllers that derive from 
        /// WindsorController to the container.  Also associate the Controller 
        /// with the WindsorContainer ControllerFactory.
        /// </summary>
        protected virtual void InitializeServiceLocator()
        {
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(Container));
            Container.RegisterControllers(typeof (HomeController).Assembly);
            ComponentRegistrar.AddComponentsTo(Container);
            //TODO: Register repositories and services for controllers
           // ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(Container));
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = ""} // Parameter defaults
                );
        }
    }
}