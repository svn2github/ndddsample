namespace NDDDSample.Tests.Domain.Model.Locations
{
    #region Usings

    using System;
    using NDDDSample.Domain.Model.Locations;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.DomainModel)]
    public class UnLocodeTest
    {
        [Test, ExpectedException(typeof (AssertionException))]
        
        public void TestNew()
        {
            AssertValid("AA234");
            AssertValid("AAA9B");
            AssertValid("AAAAA");

            AssertInvalid("AAAA");
            AssertInvalid("AAAAAA");
            AssertInvalid("AAAA");
            AssertInvalid("AAAAAA");
            AssertInvalid("22AAA");
            AssertInvalid("AA111");
            AssertInvalid(null);
        }

        [Test]
        public void TestIdString()
        {
            Assert.AreEqual("ABCDE", new UnLocode("AbcDe").IdString);
        }

        [Test]
        public void TestEquals()
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
        public void TestHashCode()
        {
            var allCaps = new UnLocode("ABCDE");
            var mixedCase = new UnLocode("aBcDe");

            Assert.AreEqual(allCaps.GetHashCode(), mixedCase.GetHashCode());
        }

        private static void AssertValid(String unlocode)
        {
            new UnLocode(unlocode);
        }

        private static void AssertInvalid(String unlocode)
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