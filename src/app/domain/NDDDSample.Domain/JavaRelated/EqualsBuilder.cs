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

namespace NDDDSample.Domain.JavaRelated
{
    #region Usings

    using System;
    using System.Reflection;

    #endregion

    public class EqualsBuilder
    {
        /**
     * If the fields tested are equals.
     */
        private bool isEqual;

        /**
         * &ltp&gtConstructor for EqualsBuilder.</p>
         *
         * &ltp&gtStarts off assuming that equals is &ltcode&gttrue</code>.</p>
         * @see java.lang.Object#equals
         */

        public EqualsBuilder()
        {
            isEqual = true;
        }


        /**
     * &ltp&gtThis method uses reflection to determine if the two &ltcode&gtObject</code&gts
     * are equal.</p>
     *
     * &ltp&gtIt uses &ltcode&gtAccessibleObject.setAccessible</code> to gain access to private
     * fields. This means that it will throw a security exception if run under
     * a security manager, if the permissions are not set up correctly. It is also
     * not as efficient as testing explicitly.</p>
     *
     * &ltp&gtTransient members will be not be tested, as they are likely derived
     * fields, and not part of the value of the Object.</p>
     *
     * &ltp&gtStatic fields will not be tested. Superclass fields will be included.</p>
     *
     * @param lhs  &ltcode&gtthis</code> object
     * @param rhs  the other object
     * @return &ltcode&gttrue</code> if the two Objects have tested equals.
     */

        public static bool ReflectionEquals(Object lhs, Object rhs)
        {
            return ReflectionEquals(lhs, rhs, false, null);
        }


        /**
     * &ltp&gtThis method uses reflection to determine if the two &ltcode&gtObject</code&gts
     * are equal.</p>
     *
     * &ltp&gtIt uses &ltcode&gtAccessibleObject.setAccessible</code> to gain access to private
     * fields. This means that it will throw a security exception if run under
     * a security manager, if the permissions are not set up correctly. It is also
     * not as efficient as testing explicitly.</p>
     *
     * &ltp&gtIf the TestTransients parameter is set to &ltcode&gttrue</code>, transient
     * members will be tested, otherwise they are ignored, as they are likely
     * derived fields, and not part of the value of the &ltcode&gtObject</code>.</p>
     *
     * &ltp&gtStatic fields will not be tested. Superclass fields will be included.</p>
     *
     * @param lhs  &ltcode&gtthis</code> object
     * @param rhs  the other object
     * @param testTransients  whether to include transient fields
     * @return &ltcode&gttrue</code> if the two Objects have tested equals.
     */

        public static bool ReflectionEquals(Object lhs, Object rhs, bool testTransients)
        {
            return ReflectionEquals(lhs, rhs, testTransients, null);
        }

        /**
    * &ltp&gtThis method uses reflection to determine if the two &ltcode&gtObject</code&gts
    * are equal.</p>
    *
    * &ltp&gtIt uses &ltcode&gtAccessibleObject.setAccessible</code> to gain access to private
    * fields. This means that it will throw a security exception if run under
    * a security manager, if the permissions are not set up correctly. It is also
    * not as efficient as testing explicitly.</p>
    *
    * &ltp&gtIf the testTransients parameter is set to &ltcode&gttrue</code>, transient
    * members will be tested, otherwise they are ignored, as they are likely
    * derived fields, and not part of the value of the &ltcode&gtObject</code>.</p>
    *
    * &ltp&gtStatic fields will not be included. Superclass fields will be appended
    * up to and including the specified superclass. A null superclass is treated
    * as java.lang.Object.</p>
    *
    * @param lhs  &ltcode&gtthis</code> object
    * @param rhs  the other object
    * @param testTransients  whether to include transient fields
    * @param reflectUpToClass  the superclass to reflect up to (inclusive),
    *  may be &ltcode&gtnull</code>
    * @return &ltcode&gttrue</code> if the two Objects have tested equals.
    * @since 2.0
    */

        public static bool ReflectionEquals(Object lhs, Object rhs, bool testTransients, Type reflectUpToClass)
        {
            if (lhs == rhs)
            {
                return true;
            }
            if (lhs == null || rhs == null)
            {
                return false;
            }
            // Find the leaf class since there may be transients in the leaf 
            // class or in classes between the leaf and root.
            // If we are not testing transients or a subclass has no ivars, 
            // then a subclass can test equals to a superclass.
            Type lhsClass = lhs.GetType();
            Type rhsClass = rhs.GetType();
            Type testClass;
            if (lhsClass.IsInstanceOfType(rhs))
            {
                testClass = lhsClass;
                if (!rhsClass.IsInstanceOfType(lhs))
                {
                    // rhsClass is a subclass of lhsClass
                    testClass = rhsClass;
                }
            }
            else if (rhsClass.IsInstanceOfType(lhs))
            {
                testClass = rhsClass;
                if (!lhsClass.IsInstanceOfType(rhs))
                {
                    // lhsClass is a subclass of rhsClass
                    testClass = lhsClass;
                }
            }
            else
            {
                // The two classes are not related.
                return false;
            }
            EqualsBuilder equalsBuilder = new EqualsBuilder();
            try
            {
                ReflectionAppend(lhs, rhs, testClass, equalsBuilder, testTransients);
                while (testClass.BaseType != null && testClass != reflectUpToClass)
                {
                    testClass = testClass.BaseType;
                    ReflectionAppend(lhs, rhs, testClass, equalsBuilder, testTransients);
                }
            }
            catch (ArgumentException e)
            {
                // In this case, we tried to test a subclass vs. a superclass and
                // the subclass has ivars or the ivars are transient and 
                // we are testing transients.
                // If a subclass has ivars that we are trying to test them, we get an
                // exception and we know that the objects are not equal.
                return false;
            }
            return equalsBuilder.IsEquals();
        }


        /**
    * &ltp&gtAppends the fields and values defined by the given object of the
    * given Class.</p>
    * 
    * @param lhs  the left hand object
    * @param rhs  the right hand object
    * @param clazz  the class to Append details of
    * @param builder  the builder to Append to
    * @param useTransients  whether to test transient fields
    */

        private static void ReflectionAppend(
            Object lhs,
            Object rhs,
            Type clazz,
            EqualsBuilder builder,
            bool useTransients)
        {
            FieldInfo[] fields = clazz.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                                 | BindingFlags.GetField);
            //TODO:atrosin Revise: AccessibleObject.setAccessible(fields, true);
            for (int i = 0; i < fields.Length && builder.isEqual; i++)
            {
                FieldInfo f = fields[i];
                //TODO:atrosin Revise:f.getName().indexOf('$')
                if ((f.Name.IndexOf('$') == -1)
                    && (useTransients || !isTransient(f))
                    && !f.IsStatic)
                {
                    try
                    {
                        builder.Append(f.GetValue(lhs), f.GetValue(rhs));
                    }//TODO: atrosin revise for more specific exc.
                    catch (Exception e)
                    {
                        //this can't happen. Would get a Security exception instead
                        //throw a runtime exception in case the impossible happens.
                        throw new Exception("Unexpected IllegalAccessException");
                    }
                }
            }
        }

        /**
    * &ltp&gtAdds the result of &ltcode&gtsuper.equals()</code> to this builder.</p>
    *
    * @param superEquals  the result of calling &ltcode&gtsuper.equals()</code>
    * @return EqualsBuilder - used to chain calls.
    * @since 2.0
    */

        public EqualsBuilder AppendSuper(bool superEquals)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = superEquals;
            return this;
        }

        /**
    * &ltp&gtTest if two &ltcode&gtObject</code&gts are equal using their
    * &ltcode&gtequals</code> method.</p>
    *
    * @param lhs  the left hand object
    * @param rhs  the right hand object
    * @return EqualsBuilder - used to chain calls.
    */

        public EqualsBuilder Append(Object lhs, Object rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            Type lhsClass = lhs.GetType();
            if (!lhsClass.IsArray)
            {
                //the simple case, not an array, just test the element
                isEqual = lhs.Equals(rhs);
            }
            else
            {
                //'Switch' on type of array, to dispatch to the correct handler
                // This handles multi dimensional arrays
                if (lhs is long[])
                {
                    Append((long[]) lhs, (long[]) rhs);
                }
                else if (lhs is int[])
                {
                    Append((int[]) lhs, (int[]) rhs);
                }
                else if (lhs is short[])
                {
                    Append((short[]) lhs, (short[]) rhs);
                }
                else if (lhs is char[])
                {
                    Append((char[]) lhs, (char[]) rhs);
                }
                else if (lhs is byte[])
                {
                    Append((byte[]) lhs, (byte[]) rhs);
                }
                else if (lhs is double[])
                {
                    Append((double[]) lhs, (double[]) rhs);
                }
                else if (lhs is float[])
                {
                    Append((float[]) lhs, (float[]) rhs);
                }
                else if (lhs is bool[])
                {
                    Append((bool[]) lhs, (bool[]) rhs);
                }
                else
                {
                    // Not an array of primitives
                    Append((Object[]) lhs, (Object[]) rhs);
                }
            }
            return this;
        }

        /**
    * &ltp&gtTest if two &ltcode&gtlong</code&gts are equal.</p>
    *
    * @param lhs  the left hand &ltcode&gtlong</code>
    * @param rhs  the right hand &ltcode&gtlong</code>
    * @return EqualsBuilder - used to chain calls.
    */

        public EqualsBuilder Append(long lhs, long rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /**
         * &ltp&gtTest if two &ltcode&gtint</code&gts are equal.</p>
         *
         * @param lhs  the left hand &ltcode&gtint</code>
         * @param rhs  the right hand &ltcode&gtint</code>
         * @return EqualsBuilder - used to chain calls.
         */

        public EqualsBuilder Append(int lhs, int rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /**
     * &ltp&gtTest if two &ltcode&gtshort</code&gts are equal.</p>
     *
     * @param lhs  the left hand &ltcode&gtshort</code>
     * @param rhs  the right hand &ltcode&gtshort</code>
     * @return EqualsBuilder - used to chain calls.
     */

        public EqualsBuilder Append(short lhs, short rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /**
         * &ltp&gtTest if two &ltcode&gtchar</code&gts are equal.</p>
         *
         * @param lhs  the left hand &ltcode&gtchar</code>
         * @param rhs  the right hand &ltcode&gtchar</code>
         * @return EqualsBuilder - used to chain calls.
         */

        public EqualsBuilder Append(char lhs, char rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /**
     * &ltp&gtTest if two &ltcode&gtbyte</code&gts are equal.</p>
     *
     * @param lhs  the left hand &ltcode&gtbyte</code>
     * @param rhs  the right hand &ltcode&gtbyte</code>
     * @return EqualsBuilder - used to chain calls.
     */

        public EqualsBuilder Append(byte lhs, byte rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /**
    * &ltp&gtTest if two &ltcode&gtdouble</code&gts are equal by testing that the
    * pattern of bits returned by &ltcode&gtdoubleToLong</code> are equal.</p>
    *
    * &ltp&gtThis handles NaNs, Infinties, and &ltcode>-0.0</code>.</p>
    *
    * &ltp&gtIt is compatible with the hash code generated by
    * &ltcode&gtHashCodeBuilder</code>.</p>
    *
    * @param lhs  the left hand &ltcode&gtdouble</code>
    * @param rhs  the right hand &ltcode&gtdouble</code>
    * @return EqualsBuilder - used to chain calls.
    */

        public EqualsBuilder Append(double lhs, double rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }


        /**
     * &ltp&gtTest if two &ltcode&gtfloat</code&gts are equal byt testing that the
     * pattern of bits returned by doubleToLong are equal.</p>
     *
     * &ltp&gtThis handles NaNs, Infinties, and &ltcode>-0.0</code>.</p>
     *
     * &ltp&gtIt is compatible with the hash code generated by
     * &ltcode&gtHashCodeBuilder</code>.</p>
     *
     * @param lhs  the left hand &ltcode&gtfloat</code>
     * @param rhs  the right hand &ltcode&gtfloat</code>
     * @return EqualsBuilder - used to chain calls.
     */

        public EqualsBuilder Append(float lhs, float rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /**
         * &ltp&gtTest if two &ltcode&gtbools</code&gts are equal.</p>
         *
         * @param lhs  the left hand &ltcode&gtbool</code>
         * @param rhs  the right hand &ltcode&gtbool</code>
         * @return EqualsBuilder - used to chain calls.
          */

        public EqualsBuilder Append(bool lhs, bool rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }


        /**
    * &ltp&gtPerforms a deep comparison of two &ltcode&gtObject</code> arrays.</p>
    *
    * &ltp&gtThis also will be called for the top level of
    * multi-dimensional, ragged, and multi-typed arrays.</p>
    *
    * @param lhs  the left hand &ltcode&gtObject[]</code>
    * @param rhs  the right hand &ltcode&gtObject[]</code>
    * @return EqualsBuilder - used to chain calls.
    */

        public EqualsBuilder Append(Object[] lhs, Object[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                if (lhs[i] != null)
                {
                    Type lhsClass = lhs[i].GetType();
                    if (!lhsClass.IsInstanceOfType(rhs[i]))
                    {
                        isEqual = false; //If the types don't match, not equal
                        break;
                    }
                }
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        /**
     * &ltp&gtDeep comparison of array of &ltcode&gtlong</code>. Length and all
     * values are compared.</p>
     *
     * &ltp&gtThe method {@link #Append(long, long)} is used.</p>
     *
     * @param lhs  the left hand &ltcode&gtlong[]</code>
     * @param rhs  the right hand &ltcode&gtlong[]</code>
     * @return EqualsBuilder - used to chain calls.
     */

        public EqualsBuilder Append(long[] lhs, long[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        /**
     * &ltp&gtDeep comparison of array of &ltcode&gtint</code>. Length and all
     * values are compared.</p>
     *
     * &ltp&gtThe method {@link #Append(int, int)} is used.</p>
     *
     * @param lhs  the left hand &ltcode&gtint[]</code>
     * @param rhs  the right hand &ltcode&gtint[]</code>
     * @return EqualsBuilder - used to chain calls.
     */

        public EqualsBuilder Append(int[] lhs, int[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        /**
    * &ltp&gtDeep comparison of array of &ltcode&gtshort</code>. Length and all
    * values are compared.</p>
    *
    * &ltp&gtThe method {@link #Append(short, short)} is used.</p>
    *
    * @param lhs  the left hand &ltcode&gtshort[]</code>
    * @param rhs  the right hand &ltcode&gtshort[]</code>
    * @return EqualsBuilder - used to chain calls.
    */

        public EqualsBuilder Append(short[] lhs, short[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        /**
     * &ltp&gtDeep comparison of array of &ltcode&gtchar</code>. Length and all
     * values are compared.</p>
     *
     * &ltp&gtThe method {@link #Append(char, char)} is used.</p>
     *
     * @param lhs  the left hand &ltcode&gtchar[]</code>
     * @param rhs  the right hand &ltcode&gtchar[]</code>
     * @return EqualsBuilder - used to chain calls.
     */

        public EqualsBuilder Append(char[] lhs, char[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        /**
         * &ltp&gtDeep comparison of array of &ltcode&gtbyte</code>. Length and all
         * values are compared.</p>
         *
         * &ltp&gtThe method {@link #Append(byte, byte)} is used.</p>
         *
         * @param lhs  the left hand &ltcode&gtbyte[]</code>
         * @param rhs  the right hand &ltcode&gtbyte[]</code>
         * @return EqualsBuilder - used to chain calls.
         */

        public EqualsBuilder Append(byte[] lhs, byte[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        /**
         * &ltp&gtDeep comparison of array of &ltcode&gtdouble</code>. Length and all
         * values are compared.</p>
         *
         * &ltp&gtThe method {@link #Append(double, double)} is used.</p>
         *
         * @param lhs  the left hand &ltcode&gtdouble[]</code>
         * @param rhs  the right hand &ltcode&gtdouble[]</code>
         * @return EqualsBuilder - used to chain calls.
         */

        public EqualsBuilder Append(double[] lhs, double[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        /**
         * &ltp&gtDeep comparison of array of &ltcode&gtfloat</code>. Length and all
         * values are compared.</p>
         *
         * &ltp&gtThe method {@link #Append(float, float)} is used.</p>
         *
         * @param lhs  the left hand &ltcode&gtfloat[]</code>
         * @param rhs  the right hand &ltcode&gtfloat[]</code>
         * @return EqualsBuilder - used to chain calls.
         */

        public EqualsBuilder Append(float[] lhs, float[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        /**
         * &ltp&gtDeep comparison of array of &ltcode&gtbool</code>. Length and all
         * values are compared.</p>
         *
         * &ltp&gtThe method {@link #Append(boolean, boolean)} is used.</p>
         *
         * @param lhs  the left hand &ltcode&gtboolean[]</code>
         * @param rhs  the right hand &ltcode&gtboolean[]</code>
         * @return EqualsBuilder - used to chain calls.
         */

        public EqualsBuilder Append(bool[] lhs, bool[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        /**
         * &ltp&gtReturn &ltcode&gttrue</code> if the fields that have been checked
         * are all equal.</p>
         *
         * @return bool
         */

        public bool IsEquals()
        {
            return isEqual;
        }


        private static bool isTransient(FieldInfo fieldInfo)
        {
            return (fieldInfo.Attributes & FieldAttributes.NotSerialized) == FieldAttributes.NotSerialized;
        }
    }
}