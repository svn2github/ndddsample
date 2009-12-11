#region The Apache Software License

/*
 *  Copyright 2001-2004 The Apache Software Foundation
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *  ported to C# by Artur Trosin
 */

#endregion

namespace NDDDSample.Tests.Infrastructure.Builders
{
    #region Usings

    using System;
    using NDDDSample.Infrastructure.Builders;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.Infrastructure)]
    public class HashCodeBuilderTest
    {
        [Test]
        public void testConstructorEx1()
        {
            try
            {
                new HashCodeBuilder(0, 0);
            }
            catch (ArgumentException ex)
            {
                return;
            }

            Assert.Fail();
        }

        [Test]
        public void testConstructorEx2()
        {
            try
            {
                new HashCodeBuilder(2, 2);
            }
            catch (ArgumentException ex)
            {
                return;
            }

            Assert.Fail();
        }


        [Test]
        public void testReflectionHashCode()
        {
            Assert.AreEqual(17 * 37, HashCodeBuilder.ReflectionHashCode(new TestObject(0)));
            Assert.AreEqual(17 * 37 + 123456, HashCodeBuilder.ReflectionHashCode(new TestObject(123456)));
        }

        [Test]
        public void testReflectionHierarchyHashCode()
        {
            Assert.AreEqual(17 * 37 * 37, HashCodeBuilder.ReflectionHashCode(new TestSubObject(0, 0, 0)));
            Assert.AreEqual(17 * 37 * 37 * 37, HashCodeBuilder.ReflectionHashCode(new TestSubObject(0, 0, 0), true));
            Assert.AreEqual((17 * 37 + 7890) * 37 + 123456,
                            HashCodeBuilder.ReflectionHashCode(new TestSubObject(123456, 7890, 0)));
            Assert.AreEqual(((17 * 37 + 7890) * 37 + 0) * 37 + 123456,
                            HashCodeBuilder.ReflectionHashCode(new TestSubObject(123456, 7890, 0), true));
        }

        [Test]
        public void testReflectionHierarchyHashCodeEx1()
        {
            try
            {
                HashCodeBuilder.ReflectionHashCode(0, 0, new TestSubObject(0, 0, 0), true);
            }
            catch (ArgumentException ex)
            {
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void testReflectionHierarchyHashCodeEx2()
        {
            try
            {
                HashCodeBuilder.ReflectionHashCode(2, 2, new TestSubObject(0, 0, 0), true);
            }
            catch (ArgumentException ex)
            {
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void testReflectionHashCodeEx1()
        {
            try
            {
                HashCodeBuilder.ReflectionHashCode(0, 0, new TestObject(0), true);
            }
            catch (ArgumentException ex)
            {
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void testReflectionHashCodeEx2()
        {
            try
            {
                HashCodeBuilder.ReflectionHashCode(2, 2, new TestObject(0), true);
            }
            catch (ArgumentException ex)
            {
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void testReflectionHashCodeEx3()
        {
            try
            {
                HashCodeBuilder.ReflectionHashCode(13, 19, null, true);
            }
            catch (ArgumentException ex)
            {
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void testSuper()
        {
            var obj = new object();
            Assert.AreEqual(17 * 37 + (19 * 41 + obj.GetHashCode()), new HashCodeBuilder(17, 37).AppendSuper(
                                                                         new HashCodeBuilder(19, 41).Append(obj).
                                                                             ToHashCode()
                                                                         ).ToHashCode());
        }

        [Test]
        public void testObject()
        {
            object obj = null;
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj = new object();
            Assert.AreEqual(17 * 37 + obj.GetHashCode(), new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
        }

        [Test]
        public void testLong()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((long) 0L).ToHashCode());
            Assert.AreEqual(17 * 37 + (int) (123456789L ^ (123456789L >> 32)),
                            new HashCodeBuilder(17, 37).Append((long) 123456789L).ToHashCode());
        }

        [Test]
        public void testInt()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append(0).ToHashCode());
            Assert.AreEqual(17 * 37 + 123456, new HashCodeBuilder(17, 37).Append((int) 123456).ToHashCode());
        }

        [Test]
        public void testShort()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((short) 0).ToHashCode());
            Assert.AreEqual(17 * 37 + 12345, new HashCodeBuilder(17, 37).Append((short) 12345).ToHashCode());
        }

        [Test]
        public void testChar()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((char) 0).ToHashCode());
            Assert.AreEqual(17 * 37 + 1234, new HashCodeBuilder(17, 37).Append((char) 1234).ToHashCode());
        }

        [Test]
        public void testByte()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((byte) 0).ToHashCode());
            Assert.AreEqual(17 * 37 + 123, new HashCodeBuilder(17, 37).Append((byte) 123).ToHashCode());
        }

        [Test]
        public void testDouble()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((double) 0d).ToHashCode());
            const double d = 1234567.89;
            long l = Convert.ToInt64(d);
            Assert.AreEqual(17 * 37 + (int) (l ^ (l >> 32)), new HashCodeBuilder(17, 37).Append(d).ToHashCode());
        }

        [Test]
        public void testFloat()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((float) 0f).ToHashCode());
            float f = 1234.89f;
            int i = Convert.ToInt32(f);
            Assert.AreEqual(17 * 37 + i, new HashCodeBuilder(17, 37).Append(f).ToHashCode());
        }

        [Test]
        public void testBoolean()
        {
            Assert.AreEqual(17 * 37 + 0, new HashCodeBuilder(17, 37).Append(true).ToHashCode());
            Assert.AreEqual(17 * 37 + 1, new HashCodeBuilder(17, 37).Append(false).ToHashCode());
        }

        [Test]
        public void testObjectArray()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((Object[]) null).ToHashCode());
            object[] obj = new object[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = new object();
            Assert.AreEqual((17 * 37 + obj[0].GetHashCode()) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[1] = new object();
            Assert.AreEqual((17 * 37 + obj[0].GetHashCode()) * 37 + obj[1].GetHashCode(),
                            new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
        }

        [Test]
        public void testObjectArrayAsObject()
        {
            object[] obj = new object[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[0] = new object();
            Assert.AreEqual((17 * 37 + obj[0].GetHashCode()) * 37,
                            new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[1] = new object();
            Assert.AreEqual((17 * 37 + obj[0].GetHashCode()) * 37 + obj[1].GetHashCode(),
                            new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
        }

        [Test]
        public void testLongArray()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((long[]) null).ToHashCode());
            long[] obj = new long[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = 5L;
            int h1 = (int) (5L ^ (5L >> 32));
            Assert.AreEqual((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[1] = 6L;
            int h2 = (int) (6L ^ (6L >> 32));
            Assert.AreEqual((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
        }

        [Test]
        public void testLongArrayAsObject()
        {
            long[] obj = new long[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[0] = 5L;
            int h1 = (int) (5L ^ (5L >> 32));
            Assert.AreEqual((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[1] = 6L;
            int h2 = (int) (6L ^ (6L >> 32));
            Assert.AreEqual((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
        }

        [Test]
        public void testIntArray()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((int[]) null).ToHashCode());
            int[] obj = new int[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = 5;
            Assert.AreEqual((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[1] = 6;
            Assert.AreEqual((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
        }

        [Test]
        public void testIntArrayAsObject()
        {
            int[] obj = new int[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[0] = 5;
            Assert.AreEqual((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[1] = 6;
            Assert.AreEqual((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
        }

        [Test]
        public void testShortArray()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((short[]) null).ToHashCode());
            short[] obj = new short[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = (short) 5;
            Assert.AreEqual((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[1] = (short) 6;
            Assert.AreEqual((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
        }

        [Test]
        public void testShortArrayAsObject()
        {
            short[] obj = new short[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[0] = (short) 5;
            Assert.AreEqual((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[1] = (short) 6;
            Assert.AreEqual((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
        }

        [Test]
        public void testCharArray()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((char[]) null).ToHashCode());
            char[] obj = new char[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = (char) 5;
            Assert.AreEqual((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[1] = (char) 6;
            Assert.AreEqual((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
        }

        [Test]
        public void testCharArrayAsObject()
        {
            char[] obj = new char[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[0] = (char) 5;
            Assert.AreEqual((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[1] = (char) 6;
            Assert.AreEqual((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
        }

        [Test]
        public void testByteArray()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((byte[]) null).ToHashCode());
            byte[] obj = new byte[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = (byte) 5;
            Assert.AreEqual((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[1] = (byte) 6;
            Assert.AreEqual((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
        }

        [Test]
        public void testByteArrayAsObject()
        {
            byte[] obj = new byte[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[0] = (byte) 5;
            Assert.AreEqual((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[1] = (byte) 6;
            Assert.AreEqual((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
        }

        [Test]
        public void testDoubleArray()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((double[]) null).ToHashCode());
            double[] obj = new double[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = 5.4d;
            long l1 = Convert.ToInt64(5.4d);
            int h1 = (int) (l1 ^ (l1 >> 32));
            Assert.AreEqual((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[1] = 6.3d;
            long l2 = Convert.ToInt64(6.3d);
            int h2 = (int) (l2 ^ (l2 >> 32));
            Assert.AreEqual((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
        }

        [Test]
        public void testDoubleArrayAsObject()
        {
            double[] obj = new double[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[0] = 5.4d;
            long l1 = Convert.ToInt64(5.4d);
            int h1 = (int) (l1 ^ (l1 >> 32));
            Assert.AreEqual((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[1] = 6.3d;
            long l2 = Convert.ToInt64(6.3d);
            int h2 = (int) (l2 ^ (l2 >> 32));
            Assert.AreEqual((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
        }

        [Test]
        public void testFloatArray()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((float[]) null).ToHashCode());
            float[] obj = new float[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = 5.4f;
            int h1 = Convert.ToInt32(5.4f);
            Assert.AreEqual((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[1] = 6.3f;
            int h2 = Convert.ToInt32(6.3f);
            Assert.AreEqual((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
        }

        [Test]
        public void testFloatArrayAsObject()
        {
            float[] obj = new float[2];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[0] = 5.4f;
            int h1 = Convert.ToInt32(5.4f);
            Assert.AreEqual((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[1] = 6.3f;
            int h2 = Convert.ToInt32(6.3f);
            Assert.AreEqual((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
        }

        [Test]
        public void testBooleanArray()
        {
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append((bool[]) null).ToHashCode());
            var obj = new bool[2];
            Assert.AreEqual((17 * 37 + 1) * 37 + 1, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = true;
            Assert.AreEqual((17 * 37 + 0) * 37 + 1, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[1] = false;
            Assert.AreEqual((17 * 37 + 0) * 37 + 1, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
        }

        [Test]
        public void testBooleanArrayAsObject()
        {
            var obj = new bool[2];
            Assert.AreEqual((17 * 37 + 1) * 37 + 1, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[0] = true;
            Assert.AreEqual((17 * 37 + 0) * 37 + 1, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
            obj[1] = false;
            Assert.AreEqual((17 * 37 + 0) * 37 + 1, new HashCodeBuilder(17, 37).Append((object) obj).ToHashCode());
        }

        [Test]
        public void testboolMultiArray()
        {
            var obj = new bool[2][];
            Assert.AreEqual((17 * 37) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = new bool[0];
            Assert.AreEqual(17 * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = new bool[1];
            Assert.AreEqual((17 * 37 + 1) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0] = new bool[2];
            Assert.AreEqual(((17 * 37 + 1) * 37 + 1) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[0][0] = true;
            Assert.AreEqual(((17 * 37 + 0) * 37 + 1) * 37, new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
            obj[1] = new bool[1];
            Assert.AreEqual((((17 * 37 + 0) * 37 + 1) * 37 + 1), new HashCodeBuilder(17, 37).Append(obj).ToHashCode());
        }
    }

    public class TestObject
    {
        private int a;

        public TestObject(int a)
        {
            this.a = a;
        }

        public override bool Equals(object o)
        {
            if (o == this)
            {
                return true;
            }
            if (!(o is TestObject))
            {
                return false;
            }
            var rhs = (TestObject) o;
            return (a == rhs.a);
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
        [NonSerialized] private int t;

        public TestSubObject()
            : base(0) {}

        public TestSubObject(int a, int b, int t)
            : base(a)
        {
            this.b = b;
            this.t = t;
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
            var rhs = (TestSubObject) o;
            return base.Equals(o) && (b == rhs.b);
        }
    }
}