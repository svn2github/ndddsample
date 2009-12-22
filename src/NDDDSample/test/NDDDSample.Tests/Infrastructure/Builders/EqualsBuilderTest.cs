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
    public class EqualsBuilderTest
    {
        [Test]
        public void testReflectionEquals()
        {
            TestObject o1 = new TestObject(4);
            TestObject o2 = new TestObject(5);
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(o1, o1));
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(o1, o2));
            o2.setA(4);
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(o1, o2));

            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(o1, this));

            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(o1, null));
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(null, o2));
            Assert.IsTrue(EqualsBuilder.ReflectionEquals((Object) null, (Object) null));
        }

        [Test]
        public void testReflectionHierarchyEquals()
        {
            testReflectionHierarchyEquals(false);
            testReflectionHierarchyEquals(true);
            // Transients
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(new TestTTLeafObject(1, 2, 3, 4),
                                                         new TestTTLeafObject(1, 2, 3, 4), true));
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(new TestTTLeafObject(1, 2, 3, 4),
                                                         new TestTTLeafObject(1, 2, 3, 4), false));
            Assert.IsTrue(
                !EqualsBuilder.ReflectionEquals(new TestTTLeafObject(1, 0, 0, 4), new TestTTLeafObject(1, 2, 3, 4), true));
            Assert.IsTrue(
                !EqualsBuilder.ReflectionEquals(new TestTTLeafObject(1, 2, 3, 4), new TestTTLeafObject(1, 2, 3, 0), true));
            Assert.IsTrue(
                !EqualsBuilder.ReflectionEquals(new TestTTLeafObject(0, 2, 3, 4), new TestTTLeafObject(1, 2, 3, 4), true));
        }

        public void testReflectionHierarchyEquals(bool testTransients)
        {
            TestObject to1 = new TestObject(4);
            TestObject to1Bis = new TestObject(4);
            TestObject to1Ter = new TestObject(4);
            TestObject to2 = new TestObject(5);
            TestEmptySubObject teso = new TestEmptySubObject(4);
            TestTSubObject ttso = new TestTSubObject(4, 1);
            TestTTSubObject tttso = new TestTTSubObject(4, 1, 2);
            TestTTLeafObject ttlo = new TestTTLeafObject(4, 1, 2, 3);
            TestSubObject tso1 = new TestSubObject(1, 4);
            TestSubObject tso1bis = new TestSubObject(1, 4);
            TestSubObject tso1ter = new TestSubObject(1, 4);
            TestSubObject tso2 = new TestSubObject(2, 5);

            testReflectionEqualsEquivalenceRelationship(to1, to1Bis, to1Ter, to2, new TestObject(), testTransients);
            testReflectionEqualsEquivalenceRelationship(tso1, tso1bis, tso1ter, tso2, new TestSubObject(),
                                                        testTransients);

            // More sanity checks:

            // same values
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(ttlo, ttlo, testTransients));
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(new TestSubObject(1, 10), new TestSubObject(1, 10),
                                                         testTransients));
            // same super values, diff sub values
            Assert.IsTrue(
                !EqualsBuilder.ReflectionEquals(new TestSubObject(1, 10), new TestSubObject(1, 11), testTransients));
            Assert.IsTrue(
                !EqualsBuilder.ReflectionEquals(new TestSubObject(1, 11), new TestSubObject(1, 10), testTransients));
            // diff super values, same sub values
            Assert.IsTrue(
                !EqualsBuilder.ReflectionEquals(new TestSubObject(0, 10), new TestSubObject(1, 10), testTransients));
            Assert.IsTrue(
                !EqualsBuilder.ReflectionEquals(new TestSubObject(1, 10), new TestSubObject(0, 10), testTransients));

            // mix super and sub types: equals
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(to1, teso, testTransients));
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(teso, to1, testTransients));

            Assert.IsTrue(EqualsBuilder.ReflectionEquals(to1, ttso, false));
            // Force testTransients = false for this assert
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(ttso, to1, false));
            // Force testTransients = false for this assert

            Assert.IsTrue(EqualsBuilder.ReflectionEquals(to1, tttso, false));
            // Force testTransients = false for this assert
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(tttso, to1, false));
            // Force testTransients = false for this assert

            Assert.IsTrue(EqualsBuilder.ReflectionEquals(ttso, tttso, false));
            // Force testTransients = false for this assert
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(tttso, ttso, false));
            // Force testTransients = false for this assert

            // mix super and sub types: NOT equals
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(new TestObject(0), new TestEmptySubObject(1), testTransients));
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(new TestEmptySubObject(1), new TestObject(0), testTransients));

            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(new TestObject(0), new TestTSubObject(1, 1), testTransients));
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(new TestTSubObject(1, 1), new TestObject(0), testTransients));

            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(new TestObject(1), new TestSubObject(0, 10), testTransients));
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(new TestSubObject(0, 10), new TestObject(1), testTransients));

            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(to1, ttlo));
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(tso1, this));
        }

        /**
  * Equivalence relationship tests inspired by "Effective Java":
  * <ul>
  * <li>reflection</li>
  * <li>symmetry</li>
  * <li>transitive</li>
  * <li>consistency</li>
  * <li>non-null reference</li>
  * </ul>
  * @param to a TestObject
  * @param toBis a TestObject, equal to to and toTer
  * @param toTer Left hand side, equal to to and toBis
  * @param to2 a different TestObject
  * @param oToChange a TestObject that will be changed
  */

        public void testReflectionEqualsEquivalenceRelationship(
            TestObject to,
            TestObject toBis,
            TestObject toTer,
            TestObject to2,
            TestObject oToChange,
            bool testTransients)
        {
            // reflection test
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(to, to, testTransients));
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(to2, to2, testTransients));

            // symmetry test
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(to, toBis, testTransients)
                          && EqualsBuilder.ReflectionEquals(toBis, to, testTransients));

            // transitive test
            Assert.IsTrue(
                EqualsBuilder.ReflectionEquals(to, toBis, testTransients)
                && EqualsBuilder.ReflectionEquals(toBis, toTer, testTransients)
                && EqualsBuilder.ReflectionEquals(to, toTer, testTransients));

            // consistency test
            oToChange.setA(to.getA());
            if (oToChange is TestSubObject)
            {
                ((TestSubObject) oToChange).setB(((TestSubObject) to).getB());
            }
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(oToChange, to, testTransients));
            Assert.IsTrue(EqualsBuilder.ReflectionEquals(oToChange, to, testTransients));
            oToChange.setA(to.getA() + 1);
            if (oToChange is TestSubObject)
            {
                ((TestSubObject) oToChange).setB(((TestSubObject) to).getB() + 1);
            }
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(oToChange, to, testTransients));
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(oToChange, to, testTransients));

            // non-null reference test
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(to, null, testTransients));
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(to2, null, testTransients));
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(null, to, testTransients));
            Assert.IsTrue(!EqualsBuilder.ReflectionEquals(null, to2, testTransients));
            Assert.IsTrue(EqualsBuilder.ReflectionEquals((Object) null, (Object) null, testTransients));
        }

        public void testSuper()
        {
            TestObject o1 = new TestObject(4);
            TestObject o2 = new TestObject(5);
            Assert.AreEqual(true, new EqualsBuilder().AppendSuper(true).Append(o1, o1).IsEquals());
            Assert.AreEqual(false, new EqualsBuilder().AppendSuper(false).Append(o1, o1).IsEquals());
            Assert.AreEqual(false, new EqualsBuilder().AppendSuper(true).Append(o1, o2).IsEquals());
            Assert.AreEqual(false, new EqualsBuilder().AppendSuper(false).Append(o1, o2).IsEquals());
        }

        [Test]
        public void testObject()
        {
            TestObject o1 = new TestObject(4);
            TestObject o2 = new TestObject(5);
            Assert.IsTrue(new EqualsBuilder().Append(o1, o1).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(o1, o2).IsEquals());
            o2.setA(4);
            Assert.IsTrue(new EqualsBuilder().Append(o1, o2).IsEquals());

            Assert.IsTrue(!new EqualsBuilder().Append(o1, this).IsEquals());

            Assert.IsTrue(!new EqualsBuilder().Append(o1, null).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(null, o2).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append((Object) null, (Object) null).IsEquals());
        }

        [Test]
        public void testLong()
        {
            long o1 = 1L;
            long o2 = 2L;
            Assert.IsTrue(new EqualsBuilder().Append(o1, o1).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(o1, o2).IsEquals());
        }

        [Test]
        public void testInt()
        {
            int o1 = 1;
            int o2 = 2;
            Assert.IsTrue(new EqualsBuilder().Append(o1, o1).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(o1, o2).IsEquals());
        }

        [Test]
        public void testShort()
        {
            short o1 = 1;
            short o2 = 2;
            Assert.IsTrue(new EqualsBuilder().Append(o1, o1).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(o1, o2).IsEquals());
        }

        [Test]
        public void testChar()
        {
            char o1 = '1';
            char o2 = '2';
            Assert.IsTrue(new EqualsBuilder().Append(o1, o1).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(o1, o2).IsEquals());
        }

        [Test]
        public void testByte()
        {
            byte o1 = 1;
            byte o2 = 2;
            Assert.IsTrue(new EqualsBuilder().Append(o1, o1).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(o1, o2).IsEquals());
        }

        [Test]
        public void testDouble()
        {
            double o1 = 1;
            double o2 = 2;
            Assert.IsTrue(new EqualsBuilder().Append(o1, o1).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(o1, o2).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(o1, Double.NaN).IsEquals());
            //TODO: atrosin NaN != NaN in java is equal??
            //Assert.IsTrue(new EqualsBuilder().Append(Double.NaN, Double.NaN).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(Double.PositiveInfinity, Double.PositiveInfinity).IsEquals());
        }

        /// <summary>
        /// Tests Append(double lhs, double rhs, double epsilon).
        /// </summary>
        /// <remarks>
        /// This is not in the Java version
        /// </remarks>
        [Test]
        public void testDoubleApproximate()
        {
            double o1 = 0.09;
            double o2 = 0.10;

            Assert.IsFalse(new EqualsBuilder().Append(o1, o2, 0.005).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(o1, o2, 0.02).IsEquals());
        }

        [Test]
        public void testFloat()
        {
            float o1 = 1;
            float o2 = 2;
            Assert.IsTrue(new EqualsBuilder().Append(o1, o1).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(o1, o2).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(o1, float.NaN).IsEquals());
            //TODO: atrosin NaN != NaN in java is equal??
            //Assert.IsTrue(new EqualsBuilder().Append(float.NaN, float.NaN).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(float.PositiveInfinity, float.PositiveInfinity).IsEquals());
        }

        /// <summary>
        /// Tests Append(float lhs, float rhs, float epsilon).
        /// </summary>
        /// <remarks>
        /// This is not in the Java version
        /// </remarks>
        [Test]
        public void testFloatApproximate()
        {
            float o1 = 0.09f;
            float o2 = 0.10f;

            Assert.IsFalse(new EqualsBuilder().Append(o1, o2, 0.005f).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(o1, o2, 0.02f).IsEquals());
        }

        [Test]
        public void testBoolean()
        {
            bool o1 = true;
            bool o2 = false;
            Assert.IsTrue(new EqualsBuilder().Append(o1, o1).IsEquals());
            Assert.IsTrue(!new EqualsBuilder().Append(o1, o2).IsEquals());
        }

        [Test]
        public void testObjectArray()
        {
            TestObject[] obj1 = new TestObject[3];
            obj1[0] = new TestObject(4);
            obj1[1] = new TestObject(5);
            obj1[2] = null;
            TestObject[] obj2 = new TestObject[3];
            obj2[0] = new TestObject(4);
            obj2[1] = new TestObject(5);
            obj2[2] = null;

            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj2, obj2).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[1].setA(6);
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[1].setA(5);
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[2] = obj1[1];
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[2] = null;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testLongArray()
        {
            long[] obj1 = new long[2];
            obj1[0] = 5L;
            obj1[1] = 6L;
            long[] obj2 = new long[2];
            obj2[0] = 5L;
            obj2[1] = 6L;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testIntArray()
        {
            int[] obj1 = new int[2];
            obj1[0] = 5;
            obj1[1] = 6;
            int[] obj2 = new int[2];
            obj2[0] = 5;
            obj2[1] = 6;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testShortArray()
        {
            short[] obj1 = new short[2];
            obj1[0] = 5;
            obj1[1] = 6;
            short[] obj2 = new short[2];
            obj2[0] = 5;
            obj2[1] = 6;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testCharArray()
        {
            char[] obj1 = new char[2];
            obj1[0] = '5';
            obj1[1] = '6';
            char[] obj2 = new char[2];
            obj2[0] = '5';
            obj2[1] = '6';
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[1] = '7';
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testByteArray()
        {
            byte[] obj1 = new byte[2];
            obj1[0] = 5;
            obj1[1] = 6;
            byte[] obj2 = new byte[2];
            obj2[0] = 5;
            obj2[1] = 6;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testDoubleArray()
        {
            double[] obj1 = new double[2];
            obj1[0] = 5;
            obj1[1] = 6;
            double[] obj2 = new double[2];
            obj2[0] = 5;
            obj2[1] = 6;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testFloatArray()
        {
            float[] obj1 = new float[2];
            obj1[0] = 5;
            obj1[1] = 6;
            float[] obj2 = new float[2];
            obj2[0] = 5;
            obj2[1] = 6;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testBooleanArray()
        {
            bool[] obj1 = new bool[2];
            obj1[0] = true;
            obj1[1] = false;
            bool[] obj2 = new bool[2];
            obj2[0] = true;
            obj2[1] = false;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1[1] = true;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testMultiLongArray()
        {
            var array1 = new long[2,2];
            var array2 = new long[2,2];

            for (int i = 0; i < array1.GetLength(0); ++i)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    array1[i, j] = (i + 1) * (j + 1);
                    array2[i, j] = (i + 1) * (j + 1);
                }
            }
            Assert.IsTrue(new EqualsBuilder().Append(array1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(array1, array2).IsEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().Append(array1, array2).IsEquals());
        }

        [Test]
        public void testMultiIntArray()
        {
            var array1 = new int[2,2];
            var array2 = new int[2,2];

            for (int i = 0; i < array1.GetLength(0); ++i)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    array1[i, j] = (i + 1) * (j + 1);
                    array2[i, j] = (i + 1) * (j + 1);
                }
            }
            Assert.IsTrue(new EqualsBuilder().Append(array1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(array1, array2).IsEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().Append(array1, array2).IsEquals());
        }

        [Test]
        public void testMultiShortArray()
        {
            var array1 = new short[2,2];
            var array2 = new short[2,2];

            for (short i = 0; i < array1.GetLength(0); ++i)
            {
                for (short j = 0; j < array1.GetLength(1); j++)
                {
                    array1[i, j] = (short) ((i + 1) * (j + 1));
                    array2[i, j] = (short) ((i + 1) * (j + 1));
                }
            }
            Assert.IsTrue(new EqualsBuilder().Append(array1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(array1, array2).IsEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().Append(array1, array2).IsEquals());
        }

        [Test]
        public void testMultiCharArray()
        {
            var array1 = new char[2,2];
            var array2 = new char[2,2];
            for (int i = 0; i < array1.GetLength(0); ++i)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    array1[i, j] = Convert.ToChar(i);
                    array2[i, j] = Convert.ToChar(i);
                }
            }
            Assert.IsTrue(new EqualsBuilder().Append(array1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(array1, array2).IsEquals());
            array1[1, 1] = '0';
            Assert.IsTrue(!new EqualsBuilder().Append(array1, array2).IsEquals());
        }

        [Test]
        public void testMultiByteArray()
        {
            var array1 = new byte[2,2];
            var array2 = new byte[2,2];
            for (byte i = 0; i < array1.GetLength(0); ++i)
            {
                for (byte j = 0; j < array1.GetLength(1); j++)
                {
                    array1[i, j] = i;
                    array2[i, j] = i;
                }
            }
            Assert.IsTrue(new EqualsBuilder().Append(array1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(array1, array2).IsEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().Append(array1, array2).IsEquals());
        }

        [Test]
        public void testMultiFloatArray()
        {
            var array1 = new float[2,2];
            var array2 = new float[2,2];
            for (int i = 0; i < array1.GetLength(0); ++i)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    array1[i, j] = i;
                    array2[i, j] = i;
                }
            }
            Assert.IsTrue(new EqualsBuilder().Append(array1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(array1, array2).IsEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().Append(array1, array2).IsEquals());
        }

        [Test]
        public void testMultiDoubleArray()
        {
            var array1 = new double[2,2];
            var array2 = new double[2,2];
            for (int i = 0; i < array1.GetLength(0); ++i)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    array1[i, j] = i;
                    array2[i, j] = i;
                }
            }
            Assert.IsTrue(new EqualsBuilder().Append(array1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(array1, array2).IsEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().Append(array1, array2).IsEquals());
        }

        [Test]
        public void testMultiBooleanArray()
        {
            var array1 = new bool[2,2];
            var array2 = new bool[2,2];
            for (int i = 0; i < array1.GetLength(0); ++i)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    array1[i, j] = (i == 1) || (j == 1);
                    array2[i, j] = (i == 1) || (j == 1);
                }
            }
            Assert.IsTrue(new EqualsBuilder().Append(array1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(array1, array2).IsEquals());
            array1[1, 1] = false;
            Assert.IsTrue(!new EqualsBuilder().Append(array1, array2).IsEquals());

            // compare 1 dim to 2.
            var array3 = new bool[] {true, true};
            Assert.IsFalse(new EqualsBuilder().Append(array1, array3).IsEquals());
            Assert.IsFalse(new EqualsBuilder().Append(array3, array1).IsEquals());
            Assert.IsFalse(new EqualsBuilder().Append(array2, array3).IsEquals());
            Assert.IsFalse(new EqualsBuilder().Append(array3, array2).IsEquals());
        }

        [Test]
        public void testRaggedArray()
        {
            var array1 = new long[2][];
            var array2 = new long[2][];


            for (int i = 0; i < array1.Length; ++i)
            {
                array1[i] = new long[2];
                array2[i] = new long[2];
                for (int j = 0; j < array1[i].Length; ++j)
                {
                    array1[i][j] = (i + 1) * (j + 1);
                    array2[i][j] = (i + 1) * (j + 1);
                }
            }
            Assert.IsTrue(new EqualsBuilder().Append(array1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(array1, array2).IsEquals());
            array1[1][1] = 0;
            Assert.IsTrue(!new EqualsBuilder().Append(array1, array2).IsEquals());
        }

        [Test]
        public void testMixedArray()
        {
            var array1 = new object[2];
            var array2 = new object[2];
            for (int i = 0; i < array1.Length; ++i)
            {
                array1[i] = new long[2];
                array2[i] = new long[2];
                for (int j = 0; j < 2; ++j)
                {
                    ((long[]) array1[i])[j] = (i + 1) * (j + 1);
                    ((long[]) array2[i])[j] = (i + 1) * (j + 1);
                }
            }
            Assert.IsTrue(new EqualsBuilder().Append(array1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(array1, array2).IsEquals());
            ((long[]) array1[1])[1] = 0;
            Assert.IsTrue(!new EqualsBuilder().Append(array1, array2).IsEquals());
        }


        [Test]
        public void testObjectArrayHiddenByObject()
        {
            TestObject[] array1 = new TestObject[2];
            array1[0] = new TestObject(4);
            array1[1] = new TestObject(5);
            TestObject[] array2 = new TestObject[2];
            array2[0] = new TestObject(4);
            array2[1] = new TestObject(5);
            Object obj1 = array1;
            Object obj2 = array2;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array2).IsEquals());
            array1[1].setA(6);
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testLongArrayHiddenByObject()
        {
            long[] array1 = new long[2];
            array1[0] = 5L;
            array1[1] = 6L;
            long[] array2 = new long[2];
            array2[0] = 5L;
            array2[1] = 6L;
            Object obj1 = array1;
            Object obj2 = array2;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array2).IsEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testIntArrayHiddenByObject()
        {
            int[] array1 = new int[2];
            array1[0] = 5;
            array1[1] = 6;
            int[] array2 = new int[2];
            array2[0] = 5;
            array2[1] = 6;
            Object obj1 = array1;
            Object obj2 = array2;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array2).IsEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testShortArrayHiddenByObject()
        {
            short[] array1 = new short[2];
            array1[0] = 5;
            array1[1] = 6;
            short[] array2 = new short[2];
            array2[0] = 5;
            array2[1] = 6;
            Object obj1 = array1;
            Object obj2 = array2;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array2).IsEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testCharArrayHiddenByObject()
        {
            char[] array1 = new char[2];
            array1[0] = '5';
            array1[1] = '6';
            char[] array2 = new char[2];
            array2[0] = '5';
            array2[1] = '6';
            Object obj1 = array1;
            Object obj2 = array2;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array2).IsEquals());
            array1[1] = '7';
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testByteArrayHiddenByObject()
        {
            byte[] array1 = new byte[2];
            array1[0] = 5;
            array1[1] = 6;
            byte[] array2 = new byte[2];
            array2[0] = 5;
            array2[1] = 6;
            Object obj1 = array1;
            Object obj2 = array2;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array2).IsEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testDoubleArrayHiddenByObject()
        {
            double[] array1 = new double[2];
            array1[0] = 5;
            array1[1] = 6;
            double[] array2 = new double[2];
            array2[0] = 5;
            array2[1] = 6;
            Object obj1 = array1;
            Object obj2 = array2;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array2).IsEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testFloatArrayHiddenByObject()
        {
            float[] array1 = new float[2];
            array1[0] = 5;
            array1[1] = 6;
            float[] array2 = new float[2];
            array2[0] = 5;
            array2[1] = 6;
            Object obj1 = array1;
            Object obj2 = array2;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array2).IsEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        [Test]
        public void testBooleanArrayHiddenByObject()
        {
            bool[] array1 = new bool[2];
            array1[0] = true;
            array1[1] = false;
            bool[] array2 = new bool[2];
            array2[0] = true;
            array2[1] = false;
            Object obj1 = array1;
            Object obj2 = array2;
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array1).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, obj2).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(obj1, array2).IsEquals());
            array1[1] = true;
            Assert.IsTrue(!new EqualsBuilder().Append(obj1, obj2).IsEquals());
        }

        /**
  * Tests two instances of classes that can be equal and that are not "related". The two classes are not subclasses
  * of each other and do not share a parent aside from Object.
  * See http://issues.apache.org/bugzilla/show_bug.cgi?id=33069
  */

        [Test]
        public void testUnrelatedClasses()
        {
            Object[] x = new Object[] {new TestACanEqualB(1)};
            Object[] y = new Object[] {new TestBCanEqualA(1)};

            // sanity checks:
            Assert.IsTrue(Array.Equals(x, x));
            Assert.IsTrue(Array.Equals(y, y));
            //in C# they are false
            Assert.IsFalse(Array.Equals(x, y));
            Assert.IsFalse(Array.Equals(y, x));

            // real tests:
            Assert.IsTrue(x[0].Equals(x[0]));
            Assert.IsTrue(y[0].Equals(y[0]));
            Assert.IsTrue(x[0].Equals(y[0]));
            Assert.IsTrue(y[0].Equals(x[0]));
            Assert.IsTrue(new EqualsBuilder().Append(x, x).IsEquals());
            Assert.IsTrue(new EqualsBuilder().Append(y, y).IsEquals());
            //in C# they are false
            Assert.IsFalse(new EqualsBuilder().Append(x, y).IsEquals());
            Assert.IsFalse(new EqualsBuilder().Append(y, x).IsEquals());
        }

        /**
* Test from http://issues.apache.org/bugzilla/show_bug.cgi?id=33067
*/

        [Test]
        public void testNpeForNullElement()
        {
            Object[] x1 = new Object[] {1, null, 3};
            Object[] x2 = new Object[] {1, 2, 3};

            // causes an NPE in 2.0 according to:
            // http://issues.apache.org/bugzilla/show_bug.cgi?id=33067
            new EqualsBuilder().Append(x1, x2);
        }

        #region Test Classes

        public class TestObject
        {
            private int a;
            public TestObject() {}

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
                TestObject rhs = (TestObject) o;
                return (a == rhs.a);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
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

            public override int GetHashCode()
            {
                return base.GetHashCode();
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

        public class TestEmptySubObject : TestObject
        {
            public TestEmptySubObject(int a) : base(a) {}
        }

        public class TestTSubObject : TestObject
        {
            [NonSerialized] private int t;

            public TestTSubObject(int a, int t)
                : base(a)
            {
                this.t = t;
            }
        }

        public class TestTTSubObject : TestTSubObject
        {
            [NonSerialized] private int tt;

            public TestTTSubObject(int a, int t, int tt)
                : base(a, t)
            {
                this.tt = tt;
            }
        }

        public class TestTTLeafObject : TestTTSubObject
        {
            private int leafValue;

            public TestTTLeafObject(int a, int t, int tt, int leafValue)
                : base(a, t, tt)
            {
                this.leafValue = leafValue;
            }
        }

        public class TestTSubObject2 : TestObject
        {
            [NonSerialized] private int t;
            public TestTSubObject2(int a, int t) : base(a) {}

            public int getT()
            {
                return t;
            }

            public void setT(int t)
            {
                this.t = t;
            }
        }

        public class TestACanEqualB
        {
            private int a;

            public TestACanEqualB(int a)
            {
                this.a = a;
            }

            public override bool Equals(Object o)
            {
                if (o == this)
                {
                    return true;
                }
                if (o is TestACanEqualB)
                {
                    return a == ((TestACanEqualB) o).getA();
                }
                if (o is TestBCanEqualA)
                {
                    return a == ((TestBCanEqualA) o).getB();
                }
                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public int getA()
            {
                return a;
            }
        }

        public class TestBCanEqualA
        {
            private int b;

            public TestBCanEqualA(int b)
            {
                this.b = b;
            }

            public override bool Equals(Object o)
            {
                if (o == this)
                {
                    return true;
                }
                if (o is TestACanEqualB)
                {
                    return b == ((TestACanEqualB) o).getA();
                }
                if (o is TestBCanEqualA)
                {
                    return b == ((TestBCanEqualA) o).getB();
                }
                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public int getB()
            {
                return b;
            }
        }

        #endregion
    }
}