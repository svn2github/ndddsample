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
 */
#endregion
namespace NDDDSample.Tests.Domain.JavaRelated
{
    #region Usings

    using System;
    using NDDDSample.Domain.JavaRelated;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class HashCodeBuilderTest
    {
        [Test]
        public void testConstructorEx1()
        {
            try
            {
                new HashCodeBuilder(0, 0);
            }
            catch (IllegalArgumentException ex)
            {
                return;
            }

            fail();
        }

        [Test]
        public void testConstructorEx2()
        {
            try
            {
                new HashCodeBuilder(2, 2);
            }
            catch (IllegalArgumentException ex)
            {
                return;
            }

            fail();
        }


        [Test]
        public void testReflectionHashCode()
        {
            Assert.Equals(17 * 37, HashCodeBuilder.reflectionHashCode(new TestObject(0)));
            Assert.Equals(17 * 37 + 123456, HashCodeBuilder.reflectionHashCode(new TestObject(123456)));
        }

        [Test]
        public void testReflectionHierarchyHashCode()
        {
            Assert.Equals(17 * 37 * 37, HashCodeBuilder.reflectionHashCode(new TestSubObject(0, 0, 0)));
            Assert.Equals(17 * 37 * 37 * 37, HashCodeBuilder.reflectionHashCode(new TestSubObject(0, 0, 0), true));
            Assert.Equals((17 * 37 + 7890) * 37 + 123456,
                          HashCodeBuilder.reflectionHashCode(new TestSubObject(123456, 7890, 0)));
            Assert.Equals(((17 * 37 + 7890) * 37 + 0) * 37 + 123456,
                          HashCodeBuilder.reflectionHashCode(new TestSubObject(123456, 7890, 0), true));
        }

        [Test]
        public void testReflectionHierarchyHashCodeEx1()
        {
            try
            {
                HashCodeBuilder.reflectionHashCode(0, 0, new TestSubObject(0, 0, 0), true);
            }
            catch (IllegalArgumentException ex)
            {
                return;
            }
            fail();
        }

        [Test]
        public void testReflectionHierarchyHashCodeEx2()
        {
            try
            {
                HashCodeBuilder.reflectionHashCode(2, 2, new TestSubObject(0, 0, 0), true);
            }
            catch (IllegalArgumentException ex)
            {
                return;
            }
            fail();
        }

        [Test]
        public void testReflectionHashCodeEx1()
        {
            try
            {
                HashCodeBuilder.reflectionHashCode(0, 0, new TestObject(0), true);
            }
            catch (IllegalArgumentException ex)
            {
                return;
            }
            fail();
        }

        [Test]
        public void testReflectionHashCodeEx2()
        {
            try
            {
                HashCodeBuilder.reflectionHashCode(2, 2, new TestObject(0), true);
            }
            catch (IllegalArgumentException ex)
            {
                return;
            }
            fail();
        }

        [Test]
        public void testReflectionHashCodeEx3()
        {
            try
            {
                HashCodeBuilder.reflectionHashCode(13, 19, null, true);
            }
            catch (IllegalArgumentException ex)
            {
                return;
            }
            fail();
        }

        [Test]
        public void testSuper()
        {
            var obj = new object();
            Assert.Equals(17 * 37 + (19 * 41 + obj.GetHashCode()), new HashCodeBuilder(17, 37).appendSuper(
                                                                       new HashCodeBuilder(19, 41).append(obj).
                                                                           toHashCode()
                                                                       ).toHashCode());
        }

        [Test]
        public void testObject()
        {
            object obj = null;
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj = new object();
            Assert.Equals(17 * 37 + obj.GetHashCode(), new HashCodeBuilder(17, 37).append(obj).toHashCode());
        }

        [Test]
        public void testLong()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((long) 0L).toHashCode());
            Assert.Equals(17 * 37 + (int) (123456789L ^ (123456789L >> 32)),
                          new HashCodeBuilder(17, 37).append((long) 123456789L).toHashCode());
        }

        [Test]
        public void testInt()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((int) 0).toHashCode());
            Assert.Equals(17 * 37 + 123456, new HashCodeBuilder(17, 37).append((int) 123456).toHashCode());
        }

        [Test]
        public void testShort()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((short) 0).toHashCode());
            Assert.Equals(17 * 37 + 12345, new HashCodeBuilder(17, 37).append((short) 12345).toHashCode());
        }

        [Test]
        public void testChar()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((char) 0).toHashCode());
            Assert.Equals(17 * 37 + 1234, new HashCodeBuilder(17, 37).append((char) 1234).toHashCode());
        }

        [Test]
        public void testByte()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((byte) 0).toHashCode());
            Assert.Equals(17 * 37 + 123, new HashCodeBuilder(17, 37).append((byte) 123).toHashCode());
        }

        [Test]
        public void testDouble()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((double) 0d).toHashCode());
            double d = 1234567.89;
            long l = Convert.ToInt64(d);
            Assert.Equals(17 * 37 + (int) (l ^ (l >> 32)), new HashCodeBuilder(17, 37).append(d).toHashCode());
        }

        [Test]
        public void testFloat()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((float) 0f).toHashCode());
            float f = 1234.89f;
            int i = Convert.ToInt32(f);
            Assert.Equals(17 * 37 + i, new HashCodeBuilder(17, 37).append(f).toHashCode());
        }

        [Test]
        public void testBoolean()
        {
            Assert.Equals(17 * 37 + 0, new HashCodeBuilder(17, 37).append(true).toHashCode());
            Assert.Equals(17 * 37 + 1, new HashCodeBuilder(17, 37).append(false).toHashCode());
        }

        [Test]
        public void testObjectArray()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((Object[]) null).toHashCode());
            object[] obj = new object[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = new object();
            Assert.Equals((17 * 37 + obj[0].GetHashCode()) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[1] = new object();
            Assert.Equals((17 * 37 + obj[0].GetHashCode()) * 37 + obj[1].GetHashCode(),
                          new HashCodeBuilder(17, 37).append(obj).toHashCode());
        }

        [Test]
        public void testObjectArrayAsObject()
        {
            object[] obj = new object[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[0] = new object();
            Assert.Equals((17 * 37 + obj[0].GetHashCode()) * 37,
                          new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[1] = new object();
            Assert.Equals((17 * 37 + obj[0].GetHashCode()) * 37 + obj[1].GetHashCode(),
                          new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
        }

        [Test]
        public void testLongArray()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((long[]) null).toHashCode());
            long[] obj = new long[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = 5L;
            int h1 = (int) (5L ^ (5L >> 32));
            Assert.Equals((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[1] = 6L;
            int h2 = (int) (6L ^ (6L >> 32));
            Assert.Equals((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).append(obj).toHashCode());
        }

        [Test]
        public void testLongArrayAsObject()
        {
            long[] obj = new long[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[0] = 5L;
            int h1 = (int) (5L ^ (5L >> 32));
            Assert.Equals((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[1] = 6L;
            int h2 = (int) (6L ^ (6L >> 32));
            Assert.Equals((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
        }

        [Test]
        public void testIntArray()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((int[]) null).toHashCode());
            int[] obj = new int[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = 5;
            Assert.Equals((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[1] = 6;
            Assert.Equals((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).append(obj).toHashCode());
        }

        [Test]
        public void testIntArrayAsObject()
        {
            int[] obj = new int[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[0] = 5;
            Assert.Equals((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[1] = 6;
            Assert.Equals((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
        }

        [Test]
        public void testShortArray()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((short[]) null).toHashCode());
            short[] obj = new short[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = (short) 5;
            Assert.Equals((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[1] = (short) 6;
            Assert.Equals((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).append(obj).toHashCode());
        }

        [Test]
        public void testShortArrayAsObject()
        {
            short[] obj = new short[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[0] = (short) 5;
            Assert.Equals((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[1] = (short) 6;
            Assert.Equals((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
        }

        [Test]
        public void testCharArray()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((char[]) null).toHashCode());
            char[] obj = new char[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = (char) 5;
            Assert.Equals((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[1] = (char) 6;
            Assert.Equals((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).append(obj).toHashCode());
        }

        [Test]
        public void testCharArrayAsObject()
        {
            char[] obj = new char[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[0] = (char) 5;
            Assert.Equals((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[1] = (char) 6;
            Assert.Equals((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
        }

        [Test]
        public void testByteArray()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((byte[]) null).toHashCode());
            byte[] obj = new byte[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = (byte) 5;
            Assert.Equals((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[1] = (byte) 6;
            Assert.Equals((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).append(obj).toHashCode());
        }

        [Test]
        public void testByteArrayAsObject()
        {
            byte[] obj = new byte[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[0] = (byte) 5;
            Assert.Equals((17 * 37 + 5) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[1] = (byte) 6;
            Assert.Equals((17 * 37 + 5) * 37 + 6, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
        }

        [Test]
        public void testDoubleArray()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((double[]) null).toHashCode());
            double[] obj = new double[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = 5.4d;
            long l1 = Convert.ToInt64(5.4d);
            int h1 = (int) (l1 ^ (l1 >> 32));
            Assert.Equals((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[1] = 6.3d;
            long l2 = Convert.ToInt64(6.3d);
            int h2 = (int) (l2 ^ (l2 >> 32));
            Assert.Equals((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).append(obj).toHashCode());
        }

        [Test]
        public void testDoubleArrayAsObject()
        {
            double[] obj = new double[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[0] = 5.4d;
            long l1 = Convert.ToInt64(5.4d);
            int h1 = (int) (l1 ^ (l1 >> 32));
            Assert.Equals((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[1] = 6.3d;
            long l2 = Convert.ToInt64(6.3d);
            int h2 = (int) (l2 ^ (l2 >> 32));
            Assert.Equals((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
        }

        [Test]
        public void testFloatArray()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((float[]) null).toHashCode());
            float[] obj = new float[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = 5.4f;
            int h1 = Convert.ToInt32(5.4f);
            Assert.Equals((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[1] = 6.3f;
            int h2 = Convert.ToInt32(6.3f);
            Assert.Equals((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).append(obj).toHashCode());
        }

        [Test]
        public void testFloatArrayAsObject()
        {
            float[] obj = new float[2];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[0] = 5.4f;
            int h1 = Convert.ToInt32(5.4f);
            Assert.Equals((17 * 37 + h1) * 37, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[1] = 6.3f;
            int h2 = Convert.ToInt32(6.3f);
            Assert.Equals((17 * 37 + h1) * 37 + h2, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
        }

        [Test]
        public void testBooleanArray()
        {
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append((bool[]) null).toHashCode());
            var obj = new bool[2];
            Assert.Equals((17 * 37 + 1) * 37 + 1, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = true;
            Assert.Equals((17 * 37 + 0) * 37 + 1, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[1] = false;
            Assert.Equals((17 * 37 + 0) * 37 + 1, new HashCodeBuilder(17, 37).append(obj).toHashCode());
        }

        [Test]
        public void testBooleanArrayAsObject()
        {
            var obj = new bool[2];
            Assert.Equals((17 * 37 + 1) * 37 + 1, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[0] = true;
            Assert.Equals((17 * 37 + 0) * 37 + 1, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
            obj[1] = false;
            Assert.Equals((17 * 37 + 0) * 37 + 1, new HashCodeBuilder(17, 37).append((object) obj).toHashCode());
        }

        [Test]
        public void testboolMultiArray()
        {
            var obj = new bool[2][];
            Assert.Equals((17 * 37) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = new bool[0];
            Assert.Equals(17 * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = new bool[1];
            Assert.Equals((17 * 37 + 1) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0] = new bool[2];
            Assert.Equals(((17 * 37 + 1) * 37 + 1) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[0][0] = true;
            Assert.Equals(((17 * 37 + 0) * 37 + 1) * 37, new HashCodeBuilder(17, 37).append(obj).toHashCode());
            obj[1] = new bool[1];
            Assert.Equals((((17 * 37 + 0) * 37 + 1) * 37 + 1), new HashCodeBuilder(17, 37).append(obj).toHashCode());
        }


        private static void fail()
        {
            Assert.Fail();
        }
    }

    #region Test Related Classes

    public class IllegalArgumentException : Exception {}


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

    #endregion
}