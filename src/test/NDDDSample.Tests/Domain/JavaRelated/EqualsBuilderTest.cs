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
    public class EqualsBuilderTest
    {
        [Test]
        public void testReflectionEquals()
        {
            TestObject o1 = new TestObject(4);
            TestObject o2 = new TestObject(5);
            Assert.IsTrue(EqualsBuilder.reflectionEquals(o1, o1));
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(o1, o2));
            o2.setA(4);
            Assert.IsTrue(EqualsBuilder.reflectionEquals(o1, o2));

            Assert.IsTrue(!EqualsBuilder.reflectionEquals(o1, this));

            Assert.IsTrue(!EqualsBuilder.reflectionEquals(o1, null));
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(null, o2));
            Assert.IsTrue(EqualsBuilder.reflectionEquals((Object) null, (Object) null));
        }

        [Test]
        public void testReflectionHierarchyEquals()
        {
            testReflectionHierarchyEquals(false);
            testReflectionHierarchyEquals(true);
            // Transients
            Assert.IsTrue(EqualsBuilder.reflectionEquals(new TestTTLeafObject(1, 2, 3, 4),
                                                         new TestTTLeafObject(1, 2, 3, 4), true));
            Assert.IsTrue(EqualsBuilder.reflectionEquals(new TestTTLeafObject(1, 2, 3, 4),
                                                         new TestTTLeafObject(1, 2, 3, 4), false));
            Assert.IsTrue(
                !EqualsBuilder.reflectionEquals(new TestTTLeafObject(1, 0, 0, 4), new TestTTLeafObject(1, 2, 3, 4), true));
            Assert.IsTrue(
                !EqualsBuilder.reflectionEquals(new TestTTLeafObject(1, 2, 3, 4), new TestTTLeafObject(1, 2, 3, 0), true));
            Assert.IsTrue(
                !EqualsBuilder.reflectionEquals(new TestTTLeafObject(0, 2, 3, 4), new TestTTLeafObject(1, 2, 3, 4), true));
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
            Assert.IsTrue(EqualsBuilder.reflectionEquals(ttlo, ttlo, testTransients));
            Assert.IsTrue(EqualsBuilder.reflectionEquals(new TestSubObject(1, 10), new TestSubObject(1, 10),
                                                         testTransients));
            // same super values, diff sub values
            Assert.IsTrue(
                !EqualsBuilder.reflectionEquals(new TestSubObject(1, 10), new TestSubObject(1, 11), testTransients));
            Assert.IsTrue(
                !EqualsBuilder.reflectionEquals(new TestSubObject(1, 11), new TestSubObject(1, 10), testTransients));
            // diff super values, same sub values
            Assert.IsTrue(
                !EqualsBuilder.reflectionEquals(new TestSubObject(0, 10), new TestSubObject(1, 10), testTransients));
            Assert.IsTrue(
                !EqualsBuilder.reflectionEquals(new TestSubObject(1, 10), new TestSubObject(0, 10), testTransients));

            // mix super and sub types: equals
            Assert.IsTrue(EqualsBuilder.reflectionEquals(to1, teso, testTransients));
            Assert.IsTrue(EqualsBuilder.reflectionEquals(teso, to1, testTransients));

            Assert.IsTrue(EqualsBuilder.reflectionEquals(to1, ttso, false));
            // Force testTransients = false for this assert
            Assert.IsTrue(EqualsBuilder.reflectionEquals(ttso, to1, false));
            // Force testTransients = false for this assert

            Assert.IsTrue(EqualsBuilder.reflectionEquals(to1, tttso, false));
            // Force testTransients = false for this assert
            Assert.IsTrue(EqualsBuilder.reflectionEquals(tttso, to1, false));
            // Force testTransients = false for this assert

            Assert.IsTrue(EqualsBuilder.reflectionEquals(ttso, tttso, false));
            // Force testTransients = false for this assert
            Assert.IsTrue(EqualsBuilder.reflectionEquals(tttso, ttso, false));
            // Force testTransients = false for this assert

            // mix super and sub types: NOT equals
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(new TestObject(0), new TestEmptySubObject(1), testTransients));
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(new TestEmptySubObject(1), new TestObject(0), testTransients));

            Assert.IsTrue(!EqualsBuilder.reflectionEquals(new TestObject(0), new TestTSubObject(1, 1), testTransients));
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(new TestTSubObject(1, 1), new TestObject(0), testTransients));

            Assert.IsTrue(!EqualsBuilder.reflectionEquals(new TestObject(1), new TestSubObject(0, 10), testTransients));
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(new TestSubObject(0, 10), new TestObject(1), testTransients));

            Assert.IsTrue(!EqualsBuilder.reflectionEquals(to1, ttlo));
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(tso1, this));
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
            Assert.IsTrue(EqualsBuilder.reflectionEquals(to, to, testTransients));
            Assert.IsTrue(EqualsBuilder.reflectionEquals(to2, to2, testTransients));

            // symmetry test
            Assert.IsTrue(EqualsBuilder.reflectionEquals(to, toBis, testTransients)
                          && EqualsBuilder.reflectionEquals(toBis, to, testTransients));

            // transitive test
            Assert.IsTrue(
                EqualsBuilder.reflectionEquals(to, toBis, testTransients)
                && EqualsBuilder.reflectionEquals(toBis, toTer, testTransients)
                && EqualsBuilder.reflectionEquals(to, toTer, testTransients));

            // consistency test
            oToChange.setA(to.getA());
            if (oToChange is TestSubObject)
            {
                ((TestSubObject) oToChange).setB(((TestSubObject) to).getB());
            }
            Assert.IsTrue(EqualsBuilder.reflectionEquals(oToChange, to, testTransients));
            Assert.IsTrue(EqualsBuilder.reflectionEquals(oToChange, to, testTransients));
            oToChange.setA(to.getA() + 1);
            if (oToChange is TestSubObject)
            {
                ((TestSubObject) oToChange).setB(((TestSubObject) to).getB() + 1);
            }
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(oToChange, to, testTransients));
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(oToChange, to, testTransients));

            // non-null reference test
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(to, null, testTransients));
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(to2, null, testTransients));
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(null, to, testTransients));
            Assert.IsTrue(!EqualsBuilder.reflectionEquals(null, to2, testTransients));
            Assert.IsTrue(EqualsBuilder.reflectionEquals((Object) null, (Object) null, testTransients));
        }

        public void testSuper()
        {
            TestObject o1 = new TestObject(4);
            TestObject o2 = new TestObject(5);
            Assert.AreEqual(true, new EqualsBuilder().appendSuper(true).append(o1, o1).isEquals());
            Assert.AreEqual(false, new EqualsBuilder().appendSuper(false).append(o1, o1).isEquals());
            Assert.AreEqual(false, new EqualsBuilder().appendSuper(true).append(o1, o2).isEquals());
            Assert.AreEqual(false, new EqualsBuilder().appendSuper(false).append(o1, o2).isEquals());
        }

        [Test]
        public void testObject()
        {
            TestObject o1 = new TestObject(4);
            TestObject o2 = new TestObject(5);
            Assert.IsTrue(new EqualsBuilder().append(o1, o1).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(o1, o2).isEquals());
            o2.setA(4);
            Assert.IsTrue(new EqualsBuilder().append(o1, o2).isEquals());

            Assert.IsTrue(!new EqualsBuilder().append(o1, this).isEquals());

            Assert.IsTrue(!new EqualsBuilder().append(o1, null).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(null, o2).isEquals());
            Assert.IsTrue(new EqualsBuilder().append((Object) null, (Object) null).isEquals());
        }

        [Test]
        public void testLong()
        {
            long o1 = 1L;
            long o2 = 2L;
            Assert.IsTrue(new EqualsBuilder().append(o1, o1).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(o1, o2).isEquals());
        }

        [Test]
        public void testInt()
        {
            int o1 = 1;
            int o2 = 2;
            Assert.IsTrue(new EqualsBuilder().append(o1, o1).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(o1, o2).isEquals());
        }

        [Test]
        public void testShort()
        {
            short o1 = 1;
            short o2 = 2;
            Assert.IsTrue(new EqualsBuilder().append(o1, o1).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(o1, o2).isEquals());
        }

        [Test]
        public void testChar()
        {
            char o1 = '1';
            char o2 = '2';
            Assert.IsTrue(new EqualsBuilder().append(o1, o1).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(o1, o2).isEquals());
        }

        [Test]
        public void testByte()
        {
            byte o1 = 1;
            byte o2 = 2;
            Assert.IsTrue(new EqualsBuilder().append(o1, o1).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(o1, o2).isEquals());
        }

        [Test]
        public void testDouble()
        {
            double o1 = 1;
            double o2 = 2;
            Assert.IsTrue(new EqualsBuilder().append(o1, o1).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(o1, o2).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(o1, Double.NaN).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(Double.NaN, Double.NaN).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(Double.PositiveInfinity, Double.PositiveInfinity).isEquals());
        }

        [Test]
        public void testFloat()
        {
            float o1 = 1;
            float o2 = 2;
            Assert.IsTrue(new EqualsBuilder().append(o1, o1).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(o1, o2).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(o1, float.NaN).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(float.NaN, float.NaN).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(float.PositiveInfinity, float.PositiveInfinity).isEquals());
        }

        [Test]
        public void testAccessors()
        {
            EqualsBuilder equalsBuilder = new EqualsBuilder();
            Assert.IsTrue(equalsBuilder.isEquals());
            equalsBuilder.setEquals(true);
            Assert.IsTrue(equalsBuilder.isEquals());
            equalsBuilder.setEquals(false);
            Assert.IsFalse(equalsBuilder.isEquals());
        }

        [Test]
        public void testBoolean()
        {
            bool o1 = true;
            bool o2 = false;
            Assert.IsTrue(new EqualsBuilder().append(o1, o1).isEquals());
            Assert.IsTrue(!new EqualsBuilder().append(o1, o2).isEquals());
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

            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj2, obj2).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[1].setA(6);
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[1].setA(5);
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[2] = obj1[1];
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[2] = null;
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[1] = '7';
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1[1] = true;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());

            obj2 = null;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
            obj1 = null;
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(array1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array1, array2).isEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().append(array1, array2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(array1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array1, array2).isEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().append(array1, array2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(array1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array1, array2).isEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().append(array1, array2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(array1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array1, array2).isEquals());
            array1[1, 1] = '0';
            Assert.IsTrue(!new EqualsBuilder().append(array1, array2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(array1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array1, array2).isEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().append(array1, array2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(array1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array1, array2).isEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().append(array1, array2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(array1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array1, array2).isEquals());
            array1[1, 1] = 0;
            Assert.IsTrue(!new EqualsBuilder().append(array1, array2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(array1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array1, array2).isEquals());
            array1[1, 1] = false;
            Assert.IsTrue(!new EqualsBuilder().append(array1, array2).isEquals());

            // compare 1 dim to 2.
            var array3 = new bool[] {true, true};
            Assert.IsTrue(new EqualsBuilder().append(array1, array3).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array3, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array2, array3).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array3, array2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(array1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array1, array2).isEquals());
            array1[1][1] = 0;
            Assert.IsTrue(!new EqualsBuilder().append(array1, array2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(array1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(array1, array2).isEquals());
            ((long[]) array1[1])[1] = 0;
            Assert.IsTrue(!new EqualsBuilder().append(array1, array2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array2).isEquals());
            array1[1].setA(6);
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array2).isEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array2).isEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array2).isEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array2).isEquals());
            array1[1] = '7';
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array2).isEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array2).isEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array2).isEquals());
            array1[1] = 7;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array1).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, obj2).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(obj1, array2).isEquals());
            array1[1] = true;
            Assert.IsTrue(!new EqualsBuilder().append(obj1, obj2).isEquals());
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
            Assert.IsTrue(Array.Equals(x, y));
            Assert.IsTrue(Array.Equals(y, x));
            // real tests:
            Assert.IsTrue(x[0].Equals(x[0]));
            Assert.IsTrue(y[0].Equals(y[0]));
            Assert.IsTrue(x[0].Equals(y[0]));
            Assert.IsTrue(y[0].Equals(x[0]));
            Assert.IsTrue(new EqualsBuilder().append(x, x).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(y, y).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(x, y).isEquals());
            Assert.IsTrue(new EqualsBuilder().append(y, x).isEquals());
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
            new EqualsBuilder().append(x1, x2);
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

        public class TestEmptySubObject : TestObject
        {
            public TestEmptySubObject(int a) : base(a) {}
        }

        public class TestTSubObject : TestObject
        {
            [NonSerialized] private int t;

            public TestTSubObject(int a, int t) : base(a)
            {
                this.t = t;
            }
        }

        public class TestTTSubObject : TestTSubObject
        {
            [NonSerialized] private int tt;

            public TestTTSubObject(int a, int t, int tt) : base(a, t)
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

            public int getB()
            {
                return b;
            }
        }

        #endregion
    }
}