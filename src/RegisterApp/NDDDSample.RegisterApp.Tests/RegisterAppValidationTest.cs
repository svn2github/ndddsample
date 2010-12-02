namespace NDDDSample.RegisterApp.Tests
{
    #region Usings

    using HandlingReportService;

    using Moq;

    using NUnit.Framework;
    
    using ViewModels;

    using Views;

    #endregion

    /// <summary>
    /// The register app validation test.
    /// </summary>
    [TestFixture, Category("Register App")]
    public class RegisterAppValidationTest
    {
        #region Fields

        private Mock<IHandlingReportService> handlingReportServiceClientMock;
        private Mock<IMessageBoxCreator> messageBoxCreator;

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
            this.messageBoxCreator = new Mock<IMessageBoxCreator>();
            this.handlingReportViewModel = new HandlingReportViewModel(this.handlingReportServiceClientMock.Object, this.messageBoxCreator.Object);
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