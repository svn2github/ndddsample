namespace NDDDSample.RegisterApp.Tests
{
    #region Usings

    using System;
    using System.ServiceModel;

    using HandlingReportService;

    using ViewModels;

    using Views;

    using NUnit.Framework;

    #endregion

    /// <summary>
    /// The register app validation test.
    /// </summary>
    [TestFixture, Category("Register App: Remote Integration")]
    public class RegisterAppIntegrationTest
    {
        #region Constants and Fields

        /// <summary>
        /// The handling report service client.
        /// </summary>
        private IHandlingReportService handlingReportServiceClient;

        /// <summary>
        /// The handling report view model.
        /// </summary>
        private HandlingReportViewModel handlingReportViewModel;

        /// <summary>
        /// The message box creator.
        /// </summary>
        private IMessageBoxCreator messageBoxCreator;

        #endregion

        #region Public Methods

        /// <summary>
        /// The set up.
        /// </summary>
        [SetUp]
        [Ignore]
        public void SetUp()
        {
            var basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.SendTimeout = TimeSpan.FromMinutes(5);
            var endpointAddress = new EndpointAddress("http://localhost:8089/HandlingReportServiceFacade/");
            this.handlingReportServiceClient = new HandlingReportServiceClient(basicHttpBinding, endpointAddress);
            this.messageBoxCreator = new MessageBoxCreator();
            this.handlingReportViewModel = new HandlingReportViewModel(
                this.handlingReportServiceClient, this.messageBoxCreator);
        }

        /// <summary>
        /// The test handling report view model validation.
        /// </summary>
        [Test]
        [Ignore]
        [ExpectedException(typeof(FaultException<HandlingReportException>))]
        public void TestHandlingReportViewModelValidation()
        {
            var handlingReport = new HandlingReport();
            handlingReport.TrackingIds = new[] { "5" };
            handlingReport.Type = HandlingReportViewModel.HandlingType.Load.ToString();
            handlingReport.UnLocode = "NYC";
            handlingReport.VoyageNumber = "123qwe";
            handlingReport.CompletionTime = DateTime.Now;
            this.handlingReportServiceClient.SubmitReport(handlingReport);
        }

        #endregion
    }
}