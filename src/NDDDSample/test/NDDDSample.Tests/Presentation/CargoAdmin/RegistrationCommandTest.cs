namespace NDDDSample.Tests.Presentation.CargoAdmin
{
    #region Usings

    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Moq;
    using NUnit.Framework;
    using Web.Controllers.CargoAdmin;

    #endregion

    [TestFixture, Category(UnitTestCategories.Controllers)]
    public class RegistrationCommandTest
    {
        [Test]
        public void CanCreateRegistrationCommandTest()
        {
            var instance = new RegistrationCommand("or1", "dest1", "arr1");

            Assert.IsNotNull(instance);
            Assert.AreEqual(instance.OriginUnlocode, "or1");
            Assert.AreEqual(instance.DestinationUnlocode, "dest1");
            Assert.AreEqual(instance.ArrivalDeadline, "arr1");
        }

        [Test]
        public void RegistrationCommandBinderTest()
        {
            //Arrange
            var mock = new Mock<ControllerContext>();
            var httpGet = new NameValueCollection { { "originUnlocode", "or1" }, { "destinationUnlocode", "dest1" }, { "arrivalDeadline", "arr1" } };
            mock.Setup(p => p.HttpContext.Request.Form).Returns(httpGet);

            //Act
            var commandBinder = new RegistrationCommandBinder();
            var bindModel = commandBinder.BindModel(mock.Object, null) as RegistrationCommand;

            //Assert
            Assert.IsNotNull(bindModel);
            Assert.AreEqual(bindModel.OriginUnlocode, "or1");
            Assert.AreEqual(bindModel.DestinationUnlocode, "dest1");
            Assert.AreEqual(bindModel.ArrivalDeadline, "arr1");            
        }
    }
}