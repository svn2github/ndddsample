namespace NDDDSample.Tests.Presentation.Tracking
{
    #region Usings

    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Moq;
    using NUnit.Framework;
    using Web.Controllers.Tracking;

    #endregion

    [TestFixture, Category(UnitTestCategories.Controllers)]
    public class TrackCommandTest
    {
        [Test]
        public void CanCreateTrackCommandTest()
        {
            var instance = new TrackCommand() {TrackingId = "ZZZ"};

            Assert.IsNotNull(instance);
            Assert.AreEqual(instance.TrackingId, "ZZZ");
        }

        [Test]
        public void TrackCommandBinderTest()
        {
            //Arrange
            var mock = new Mock<ControllerContext>();
            var httpGet = new NameValueCollection {{"trackingId", "ZWY"}};
            mock.Setup(p => p.HttpContext.Request.Form).Returns(httpGet);

            //Act
            var commandBinder = new TrackCommandBinder();
            var bindModel = commandBinder.BindModel(mock.Object, null) as TrackCommand;

            //Assert
            Assert.IsNotNull(bindModel);
            Assert.AreEqual(bindModel.TrackingId, "ZWY");
        }
    }
}