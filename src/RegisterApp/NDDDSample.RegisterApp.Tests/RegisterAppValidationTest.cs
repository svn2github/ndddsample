namespace NDDDSample.RegisterApp.Tests
{
    #region Usings

    using HandlingReportService;

    using Moq;

    using NUnit.Framework;
    
    using ViewModels;

    #endregion

    /// <summary>
    /// The register app validation test.
    /// </summary>
    [TestFixture]
    public class RegisterAppValidationTest
    {
        #region Fields

        private Mock<IHandlingReportService> handlingReportServiceClientMock;

        private HandlingReportViewModel handlingReportViewModel;

        #endregion

        #region Public Methods

        /// <summary>
        /// The set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {            
            this.handlingReportServiceClientMock = new Mock<IHandlingReportService>();
            this.handlingReportViewModel = new HandlingReportViewModel(this.handlingReportServiceClientMock.Object);
        }
        
        [Test]
        public void TestHandlingReportViewModelValidation()
        {
            this.handlingReportViewModel.TrackingId = "5";
            this.handlingReportViewModel.Validate();
            Assert.IsTrue(this.handlingReportViewModel.ValidationErrors.Count > 0);
        }

        #endregion
    }
}