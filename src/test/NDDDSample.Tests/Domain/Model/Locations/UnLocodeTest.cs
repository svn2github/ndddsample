namespace NDDDSample.Tests.Domain.Model.Locations
{
    using System;
    using NDDDSample.Domain.Model.Locations;
    using NUnit.Framework;

    [TestFixture]
    public class UnLocodeTest
    {        
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void testNew()
        {
            assertValid("AA234");
            assertValid("AAA9B");
            assertValid("AAAAA");

            assertInvalid("AAAA");
            assertInvalid("AAAAAA");
            assertInvalid("AAAA");
            assertInvalid("AAAAAA");
            assertInvalid("22AAA");
            assertInvalid("AA111");
            assertInvalid(null);
        }
        
        [Test]
        public void testIdString()
        {
            Assert.AreEqual("ABCDE", new UnLocode("AbcDe").IdString);
        }

        [Test]
        public void testEquals()
        {
            var allCaps = new UnLocode("ABCDE");
            var mixedCase = new UnLocode("aBcDe");

            Assert.IsTrue(allCaps.Equals(mixedCase));
            Assert.IsTrue(mixedCase.Equals(allCaps));
            Assert.IsTrue(allCaps.Equals(allCaps));

            Assert.IsFalse(allCaps.Equals(null));
            Assert.IsFalse(allCaps.Equals(new UnLocode("FGHIJ")));
        }

        [Test]
        public void testHashCode()
        {
            var allCaps = new UnLocode("ABCDE");
            var mixedCase = new UnLocode("aBcDe");

            Assert.AreEqual(allCaps.GetHashCode(), mixedCase.GetHashCode());
        }

        private void assertValid(String unlocode)
        {
            new UnLocode(unlocode);
        }
        
        private void assertInvalid(String unlocode)
        {
            try
            {
                new UnLocode(unlocode);                
            }
            catch (Exception expected)
            {
                Assert.Fail("The combination [" + unlocode + "] is not a valid UnLocode");
            }
        }
    }
}