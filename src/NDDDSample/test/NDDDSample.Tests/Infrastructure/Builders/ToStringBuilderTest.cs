namespace NDDDSample.Tests.Infrastructure.Builders
{
    #region Usings

    using NDDDSample.Infrastructure.Builders;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.Infrastructure)]
    public class ToStringBuilderTest
    {
        [Test]
        public void CanLogNull()
        {
            Assert.IsNotNull(ToStringBuilder.ReflectionToString(null));
        }

        [Test]
        public void CanLogSimpleObject()
        {
            Assert.IsTrue(ToStringBuilder.ReflectionToString(new TestObject(100)).Contains("100"));
        }

        [Test]
        public void CanLogDerived()
        {
            Assert.IsTrue(ToStringBuilder.ReflectionToString(new TestSubObject(100, 999)).Contains("100"));
            Assert.IsTrue(ToStringBuilder.ReflectionToString(new TestSubObject(100, 999)).Contains("999"));
        }

        [Test]
        public void CanLogType()
        {
            Assert.IsTrue(
                ToStringBuilder.ReflectionToString(new TestSubObject(100, 999)).Contains(
                    typeof (TestSubObject).ToString()));
        }

        #region Test Classes

        public class TestObject
        {
            protected int a;
            public TestObject() {}

            public TestObject(int a)
            {
                this.a = a;
            }

            public void setA(int a)
            {
                this.a = a;
            }

            public int getA()
            {
                return a;
            }
        }

        public class TestSubObject : TestObject
        {
            private int b;

            public TestSubObject()
                : base(0) {}

            public TestSubObject(int a, int b)
                : base(a)
            {
                this.b = b;
            }

            public override bool Equals(object o)
            {
                if (o == this)
                {
                    return true;
                }
                if (!(o is TestSubObject))
                {
                    return false;
                }
                TestSubObject rhs = (TestSubObject) o;
                return base.Equals(o) && (b == rhs.b);
            }

            public void setB(int b)
            {
                this.b = b;
            }

            public int getB()
            {
                return b;
            }
        }

        #endregion
    }
}