namespace NDDDSample.Tests.Infrastructure.Builders
{
    #region Usings

    using NDDDSample.Infrastructure.Builders;
    using NUnit.Framework;

    #endregion

    /// <summary>
    /// Test class for MathUtilTest
    /// </summary>
    [TestFixture, Category(UnitTestCategories.Infrastructure)]
    public class MathUtilTest
    {
        [SetUp]
        public void SetUp() {}

        [Test]
        public void TestFloatEqualTo()
        {
            float left = 0.10f;
            float right = 0.09f;

            Assert.IsTrue(MathUtil.FloatEqualTo(left, right, 0.02f));
            Assert.IsFalse(MathUtil.FloatEqualTo(left, right, 0.005f));
        }

        [Test]
        public void TestFloatGreaterThan()
        {
            float left = 0.10f;
            float right = 0.09f;

            Assert.IsFalse(MathUtil.FloatGreaterThan(left, right, 0.02f));
            Assert.IsTrue(MathUtil.FloatGreaterThan(left, right, 0.005f));
        }

        [Test]
        public void TestFloatLessThan()
        {
            float left = 0.09f;
            float right = 0.10f;

            Assert.IsFalse(MathUtil.FloatLessThan(left, right, 0.02f));
            Assert.IsTrue(MathUtil.FloatLessThan(left, right, 0.005f));
        }

        [Test]
        public void TestFloatGreaterThanOrEqualTo()
        {
            float left = 0.10f;
            float right = 0.09f;

            Assert.IsTrue(MathUtil.FloatGreaterThanOrEqualTo(left, right, 0.02f));
            Assert.IsTrue(MathUtil.FloatGreaterThanOrEqualTo(left, right, 0.005f));

            left = 0.09f;
            right = 0.10f;
            Assert.IsTrue(MathUtil.FloatGreaterThanOrEqualTo(left, right, 0.02f));
            Assert.IsFalse(MathUtil.FloatGreaterThanOrEqualTo(left, right, 0.005f));
        }

        [Test]
        public void TestFloatLessThanOrEqualTo()
        {
            float left = 0.09f;
            float right = 0.10f;

            Assert.IsTrue(MathUtil.FloatLessThanOrEqualTo(left, right, 0.02f));
            Assert.IsTrue(MathUtil.FloatLessThanOrEqualTo(left, right, 0.005f));

            left = 0.10f;
            right = 0.09f;
            Assert.IsTrue(MathUtil.FloatLessThanOrEqualTo(left, right, 0.02f));
            Assert.IsFalse(MathUtil.FloatLessThanOrEqualTo(left, right, 0.005f));
        }

        [Test]
        public void TestDoubleEqualTo()
        {
            double left = 0.000010;
            double right = 0.000009;

            Assert.IsTrue(MathUtil.DoubleEqualTo(left, right, 0.000002));
            Assert.IsFalse(MathUtil.DoubleEqualTo(left, right, 0.0000005));
        }

        [Test]
        public void TestDoubleGreaterThan()
        {
            double left = 0.000010;
            double right = 0.000009;

            Assert.IsFalse(MathUtil.DoubleGreaterThan(left, right, 0.000002));
            Assert.IsTrue(MathUtil.DoubleGreaterThan(left, right, 0.0000005));
        }

        [Test]
        public void TestDoubleLessThan()
        {
            double left = 0.000009;
            double right = 0.000010;

            Assert.IsFalse(MathUtil.DoubleLessThan(left, right, 0.000002));
            Assert.IsTrue(MathUtil.DoubleLessThan(left, right, 0.0000005));
        }

        [Test]
        public void TestDoubleGreaterThanOrEqualTo()
        {
            double left = 0.000010;
            double right = 0.000009;

            Assert.IsTrue(MathUtil.DoubleGreaterThanOrEqualTo(left, right, 0.000002));
            Assert.IsTrue(MathUtil.DoubleGreaterThanOrEqualTo(left, right, 0.0000005));

            left = 0.000009;
            right = 0.000010;
            Assert.IsTrue(MathUtil.DoubleGreaterThanOrEqualTo(left, right, 0.000002));
            Assert.IsFalse(MathUtil.DoubleGreaterThanOrEqualTo(left, right, 0.0000005));
        }

        [Test]
        public void TestDoubleLessThanOrEqualTo()
        {
            double left = 0.000009;
            double right = 0.000010;

            Assert.IsTrue(MathUtil.DoubleLessThanOrEqualTo(left, right, 0.000002));
            Assert.IsTrue(MathUtil.DoubleLessThanOrEqualTo(left, right, 0.0000005));

            left = 0.000010;
            right = 0.000009;
            Assert.IsTrue(MathUtil.DoubleLessThanOrEqualTo(left, right, 0.000002));
            Assert.IsFalse(MathUtil.DoubleLessThanOrEqualTo(left, right, 0.0000005));
        }
    }
}

// end namespace NDDDSample.Infrastructure.Util