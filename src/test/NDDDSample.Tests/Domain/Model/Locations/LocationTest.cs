namespace NDDDSample.Tests.Domain.Model.Voyages
{
    #region Usings

    using System;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class LocationTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException), UserMessage = "Should not allow any null constructor arguments")]
        public void testEquals()
        {            
            // Same UN locode - equal
            Assert.IsTrue(new Location(new UnLocode("ATEST"), "test-name").
                Equals(new Location(new UnLocode("ATEST"), "test-name")));

            // Different UN locodes - not equal
            Assert.IsFalse(new Location(new UnLocode("ATEST"), "test-name").
                 Equals(new Location(new UnLocode("TESTB"), "test-name")));

            // Always equal to itself
            Location location = new Location(new UnLocode("ATEST"), "test-name");
            Assert.IsTrue(location.Equals(location));

            // Never equal to null
            Assert.IsFalse(location.Equals(null));

            // Special UNKNOWN location is equal to itself
            Assert.IsTrue(Location.UNKNOWN.Equals(Location.UNKNOWN));

            new Location(null, null);
        }       
    }
}