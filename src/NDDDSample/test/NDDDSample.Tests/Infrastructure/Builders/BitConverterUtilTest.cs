namespace NDDDSample.Tests.Infrastructure.Builders
{
    #region Usings

    using NDDDSample.Infrastructure.Builders;
    using NUnit.Framework;

    #endregion

    /// <summary>
    /// Test class for BitConverterUtil
    /// </summary>
    [TestFixture, Category(UnitTestCategories.Infrastructure)]
    public class BitConverterUtilTest
    {
        [SetUp]
        public void SetUp() {}

        /// <summary>
        /// Tests SingleToInt32Bits().
        /// </summary>
        [Test]
        public void TestSingleToInt32Bits()
        {
            Assert.IsTrue(BitConverterUtil.SingleToInt32Bits(24.154f) == 1103182692);
            Assert.IsTrue(BitConverterUtil.SingleToInt32Bits(1.0f) == 1065353216);
            Assert.IsTrue(BitConverterUtil.SingleToInt32Bits(float.PositiveInfinity) == (int) 0x7f800000U);

            // Use "unchecked" syntax to avoid compiler error: "Constant value '4286578688' cannot be 
            // converted to a 'int' (use 'unchecked' syntax to override). The literal 0xff800000 
            // (4286578688)won't fit in a signed int without overflow.
            // C# statements can execute in either checked or unchecked context. In a checked context,
            // arithmetic overflow raises an exception. In an unchecked context, arithmetic overflow 
            // is ignored and the result is truncated.
            unchecked
            {
                Assert.IsTrue(BitConverterUtil.SingleToInt32Bits(float.NegativeInfinity) == (int) 0xff800000U);
            }
        }
    }
}