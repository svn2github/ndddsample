namespace NDDDSample.RegisterApp
{
    #region Usings

    using System.Windows;

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

            //RegisterServices();

            var viewModel = DynamicContainer.Instance.Resolve<HandlingReportViewModel>();

            var registerAppWindow = new RegisterAppWindow { DataContext = viewModel };

            registerAppWindow.Show();
        }

        /// <summary>
        /// The register services.
        /// </summary>
        private static void RegisterServices()
        {
            DynamicContainer.Instance.AddComponent(
                "handlingReportService", typeof(IHandlingReportService), typeof(HandlingReportServiceClient));
            DynamicContainer.Instance.AddComponent(
                "handlingReportViewModel", typeof(HandlingReportViewModel));
        }

        #endregion
    }
}