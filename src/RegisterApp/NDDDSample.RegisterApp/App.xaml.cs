namespace NDDDSample.RegisterApp
{
    #region Usings

    using System;
    using System.ServiceModel;
    using System.Windows;

    using Castle.Facilities.WcfIntegration;
    using Castle.MicroKernel.Registration;

    using HandlingReportService;

    using ViewModels;

    using Views;

    #endregion

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        #region Methods

        /// <summary>
        /// The on startup.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = DynamicContainer.Instance;

            container.AddFacility<WcfFacility>();

            container.Register(
                Component.For<IHandlingReportService>()                
                    .Named("handlingReportServiceClient")
                    .LifeStyle.Transient
                    .ActAs(DefaultClientModel
                        .On(WcfEndpoint.BoundTo(new BasicHttpBinding())
                            .At(new Uri("http://127.0.0.1:8089/HandlingReportServiceFacade"))
                        ))
                    .LifeStyle.Transient);

            var viewModel = container.Resolve<HandlingReportViewModel>();

            var registerAppWindow = new RegisterAppWindow { DataContext = viewModel };

            registerAppWindow.Show();
        }

        #endregion
    }
}