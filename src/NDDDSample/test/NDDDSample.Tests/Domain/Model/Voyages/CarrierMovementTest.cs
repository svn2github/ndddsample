namespace NDDDSample.Tests.Domain.Model.Voyages
{
    #region Usings

    using System;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.DomainModel)]
    public class CarrierMovementTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException), UserMessage = "Should not accept null constructor arguments")]
        public void TestConstructor()
        {
            new CarrierMovement(null, null, new DateTime(), new DateTime());
            new CarrierMovement(SampleLocations.STOCKHOLM, null, new DateTime(), new DateTime());
        }

        [Test]
        public void TestSameValueAsEqualsHashCode()
        {
            var cm1 = new CarrierMovement(SampleLocations.STOCKHOLM, SampleLocations.HAMBURG, new DateTime(), new DateTime());
            var cm2 = new CarrierMovement(SampleLocations.STOCKHOLM, SampleLocations.HAMBURG, new DateTime(), new DateTime());
            var cm3 = new CarrierMovement(SampleLocations.HAMBURG, SampleLocations.STOCKHOLM, new DateTime(), new DateTime());
            var cm4 = new CarrierMovement(SampleLocations.HAMBURG, SampleLocations.STOCKHOLM, new DateTime(), new DateTime());            

            Assert.IsTrue(cm1.SameValueAs(cm2));
            Assert.IsFalse(cm2.SameValueAs(cm3));
            Assert.IsTrue(cm3.SameValueAs(cm4));

            Assert.IsTrue(cm1.Equals(cm2));
            Assert.IsFalse(cm2.Equals(cm3));
            Assert.IsTrue(cm3.Equals(cm4));

            Assert.IsTrue(cm1.GetHashCode() == cm2.GetHashCode());
            Assert.IsFalse(cm2.GetHashCode() == cm3.GetHashCode());
            Assert.IsTrue(cm3.GetHashCode() == cm4.GetHashCode());
        }
    }
}