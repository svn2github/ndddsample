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

    /// <summary>
    ///<p>Assists in implementing object.GetHashCode() methods.</p>
    ///<p> This class enables a good <code>GetHashCode</code> method to be built for any class. It
    ///follows the rules laid out in the book
    ///<a HREF="http://java.sun.com/docs/books/effective/index.html">Effective Java</a>
    ///by Joshua Bloch. Writing a good <code>GetHashCode</code> method is actually quite
    ///difficult. This class aims to simplify the process.</p>
    ///<p>All relevant fields from the object should be included in the
    ///<code>GetHashCode</code> method. Derived fields may be excluded. In general, any
    ///field used in the <code>equals</code> method must be used in the <code>GetHashCode</code>
    ///method.</p>
    ///<p>To use this class write code as follows:</p>
    ///<pre>
    ///public class Person {
    ///String name;
    ///int age;
    ///boolean isSmoker;
    ///...
    ///public int GetHashCode() {
    // you pick a hard-coded, randomly chosen, non-zero, odd number
    // ideally different for each class
    ///return new HashCodeBuilder(17, 37).
    ///    Append(name).
    ///    Append(age).
    ///    Append(smoker).
    ///    ToHashCode();
    /// }
    ///}
    ///</pre>
    ///<p>If required, the superclass <code>hashCode()</code> can be added
    ///using {@link #AppendSuper}.</p>
    ///<p>Alternatively, there is a method that uses reflection to determine
    ///the fields to test. Because these fields are usually private, the method,
    ///<code>reflectionHashCode</code>, uses <code>AccessibleObject.setAccessible</code> to
    ///change the visibility of the fields. This will fail under a security manager,
    ///unless the appropriate permissions are set up correctly. It is also slower
    ///than testing explicitly.</p>
    ///<p>A typical invocation for this method would look like:</p>
    ///<pre>
    ///public int hashCode() {
    ///return HashCodeBuilder.reflectionHashCode(this);
    ///}
    ///</pre>
    ///@author Stephen Colebourne
    ///@author Gary Gregory
    ///@author Pete Gieser    
    ///@since 1.0
    ///ported to C# by Artur Trosin
    ///@version $Id: HashCodeBuilder.java 161243 2005-04-14 04:30:28Z ggregory $
    /// </summary>
    public class HashCodeBuilder
    {
        /// <summary>
        ///Constant to use in building the hashCode.
        /// </summary>
        private readonly int iConstant;

        /// <summary>
        /// Running total of the hashCode.
        /// </summary>
        private int iTotal;

        /// <summary>
        /// Constructor.
        /// This constructor uses two hard coded choices for the constants
        /// needed to build a GetHashCode
        /// </summary>
        public HashCodeBuilder()
        {
            iConstant = 37;
            iTotal = 17;
        }

        /// <summary>
        /// Constructor.
        /// Two randomly chosen, non-zero, odd numbers must be passed in.
        /// Ideally these should be different for each class, however this is
        /// not vital.
        /// Prime numbers are preferred, especially for the multiplier.
        /// throws ArgumentException if the number is zero or even
        /// </summary>
        /// <param name="initialNonZeroOddNumber">a non-zero, odd number used as the initial value</param>
        /// <param name="multiplierNonZeroOddNumber">a non-zero, odd number used as the multiplier</param>
        public HashCodeBuilder(int initialNonZeroOddNumber, int multiplierNonZeroOddNumber)
        {
            if (initialNonZeroOddNumber == 0)
            {
                throw new ArgumentException("HashCodeBuilder requires a non zero initial value");
            }
            if (initialNonZeroOddNumber % 2 == 0)
            {
                throw new ArgumentException("HashCodeBuilder requires an odd initial value");
            }
            if (multiplierNonZeroOddNumber == 0)
            {
                throw new ArgumentException("HashCodeBuilder requires a non zero multiplier");
            }
            if (multiplierNonZeroOddNumber % 2 == 0)
            {
                throw new ArgumentException("HashCodeBuilder requires an odd multiplier");
            }
            iConstant = multiplierNonZeroOddNumber;
            iTotal = initialNonZeroOddNumber;
        }

        /// <summary>
        /// This method uses reflection to build a valid hash code.
        /// This constructor uses two hard coded choices for the constants
        /// needed to build a hash code.
        /// It uses &ltcode&gtAccessibleObject.setAccessible</code> to gain access to private
        /// fields. This means that it will throw a security exception if run under
        /// a security manager, if the permissions are not set up correctly. It is
        /// also not as efficient as testing explicitly.
        /// Transient members will be not be used, as they are likely derived
        /// fields, and not part of the value of the &ltcode&gtObject
        /// Static fields will not be tested. Superclass fields will be included.
        /// throws ArgumentException if the object is null
        /// </summary>
        /// <param name="obj">the Object to create a GetHashCode</param>
        /// <returns>hash code</returns>
        public static int ReflectionHashCode(object obj)
        {
            return ReflectionHashCode(17, 37, obj, false, null);
        }

        /**
     * &ltp&gtThis method uses reflection to build a valid hash code.</p>
     *
     * &ltp&gtThis constructor uses two hard coded choices for the constants needed
     * to build a hash code.</p>
     *
     * &ltp> It uses &ltcode&gtAccessibleObject.setAccessible</code> to gain access to private
     * fields. This means that it will throw a security exception if run under
     * a security manager, if the permissions are not set up correctly. It is
     * also not as efficient as testing explicitly.</p>
     *
     * &ltP&gtIf the TestTransients parameter is set to &ltcode&gttrue</code>, transient
     * members will be tested, otherwise they are ignored, as they are likely
     * derived fields, and not part of the value of the &ltcode&gtObject</code>.</p>
     *
     * &ltp&gtStatic fields will not be tested. Superclass fields will be included.</p>
     *
     * @param object  the Object to create a &ltcode&gthashCode</code> for
     * @param testTransients  whether to include transient fields
     * @return int hash code
     * @throws IllegalArgumentException if the object is &ltcode&gtnull</code>
     */

        public static int ReflectionHashCode(object obj, bool testTransients)
        {
            return ReflectionHashCode(17, 37, obj, testTransients, null);
        }

        /**
     * &ltp&gtThis method uses reflection to build a valid hash code.</p>
     *
     * &ltp&gtIt uses &ltcode&gtAccessibleObject.setAccessible</code> to gain access to private
     * fields. This means that it will throw a security exception if run under
     * a security manager, if the permissions are not set up correctly. It is
     * also not as efficient as testing explicitly.</p>
     *
     * &ltp&gtTransient members will be not be used, as they are likely derived
     * fields, and not part of the value of the &ltcode&gtObject</code>.</p>
     *
     * &ltp&gtStatic fields will not be tested. Superclass fields will be included.</p>
     *
     * &ltp&gtTwo randomly chosen, non-zero, odd numbers must be passed in. Ideally
     * these should be different for each class, however this is not vital.
     * Prime numbers are preferred, especially for the multiplier.</p>
     *
     * @param initialNonZeroOddNumber  a non-zero, odd number used as the initial value
     * @param multiplierNonZeroOddNumber  a non-zero, odd number used as the multiplier
     * @param object  the Object to create a &ltcode&gthashCode</code> for
     * @return int hash code
     * @throws IllegalArgumentException if the Object is &ltcode&gtnull</code>
     * @throws IllegalArgumentException if the number is zero or even
     */

        public static int ReflectionHashCode(
            int initialNonZeroOddNumber, int multiplierNonZeroOddNumber, object obj)
        {
            return ReflectionHashCode(initialNonZeroOddNumber, multiplierNonZeroOddNumber, obj, false, null);
        }


        /**
     * &ltp&gtThis method uses reflection to build a valid hash code.</p>
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
     * &ltp&gtTwo randomly chosen, non-zero, odd numbers must be passed in. Ideally
     * these should be different for each class, however this is not vital.
     * Prime numbers are preferred, especially for the multiplier.</p>
     *
     * @param initialNonZeroOddNumber  a non-zero, odd number used as the initial value
     * @param multiplierNonZeroOddNumber  a non-zero, odd number used as the multiplier
     * @param object  the Object to create a &ltcode&gthashCode</code> for
     * @param testTransients  whether to include transient fields
     * @return int hash code
     * @throws IllegalArgumentException if the Object is &ltcode&gtnull</code>
     * @throws IllegalArgumentException if the number is zero or even
     */

        public static int ReflectionHashCode(
            int initialNonZeroOddNumber, int multiplierNonZeroOddNumber,
            Object obj, bool testTransients)
        {
            return ReflectionHashCode(initialNonZeroOddNumber, multiplierNonZeroOddNumber, obj, testTransients, null);
        }

        /**
     * &ltp&gtThis method uses reflection to build a valid hash code.</p>
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
     * &ltp&gtStatic fields will not be included. Superclass fields will be included
     * up to and including the specified superclass. A null superclass is treated
     * as java.lang.Object.</p>
     *
     * &ltp&gtTwo randomly chosen, non-zero, odd numbers must be passed in. Ideally
     * these should be different for each class, however this is not vital.
     * Prime numbers are preferred, especially for the multiplier.</p>
     *
     * @param initialNonZeroOddNumber  a non-zero, odd number used as the initial value
     * @param multiplierNonZeroOddNumber  a non-zero, odd number used as the multiplier
     * @param object  the Object to create a &ltcode&gthashCode</code> for
     * @param testTransients  whether to include transient fields
     * @param reflectUpToClass  the superclass to reflect up to (inclusive),
     *  may be &ltcode&gtnull</code>
     * @return int hash code
     * @throws IllegalArgumentException if the Object is &ltcode&gtnull</code>
     * @throws IllegalArgumentException if the number is zero or even
     * @since 2.0
     */

        public static int ReflectionHashCode(
            int initialNonZeroOddNumber,
            int multiplierNonZeroOddNumber,
            Object obj,
            bool testTransients,
            Type reflectUpToClass)
        {
            if (obj == null)
            {
                throw new ArgumentException("The object to build a hash code for must not be null");
            }
            HashCodeBuilder builder = new HashCodeBuilder(initialNonZeroOddNumber, multiplierNonZeroOddNumber);
            Type clazz = obj.GetType();
            reflectionAppend(obj, clazz, builder, testTransients);
            while (clazz.BaseType != null && clazz != reflectUpToClass)
            {
                clazz = clazz.BaseType;
                reflectionAppend(obj, clazz, builder, testTransients);
            }
            return builder.ToHashCode();
        }

        /**
     * &ltp&gtAppends the fields and values defined by the given object of the
     * given &ltcode&gtClass</code>.</p>
     * 
     * @param object  the object to Append details of
     * @param clazz  the class to Append details of
     * @param builder  the builder to Append to
     * @param useTransients  whether to use transient fields
     */

        private static void reflectionAppend(object obj, Type clazz, HashCodeBuilder builder, bool useTransients)
        {
            //TODO: atrosin what does it mean? f.Name.IndexOf('$')                        
            //TODO: atrosin //AccessibleObject.setAccessible(fields, true);

            FieldInfo[] fields =
                clazz.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                | BindingFlags.GetField);
            //AccessibleObject.setAccessible(fields, true);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo f = fields[i];

                if ((f.Name.IndexOf('$') == -1)
                    && (useTransients || !isTransient(f))
                    && !f.IsStatic)
                {
                    try
                    {
                        builder.Append(f.GetValue(obj));
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
* &ltp&gtAdds the result of super.hashCode() to this builder.</p>
*
* @param superHashCode  the result of calling &ltcode&gtsuper.hashCode()</code>
* @return this HashCodeBuilder, used to chain calls.
* @since 2.0
*/

        public HashCodeBuilder AppendSuper(int superHashCode)
        {
            iTotal = iTotal * iConstant + superHashCode;
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for an &ltcode&gtObject</code>.</p>
     *
     * @param object  the Object to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(object obj)
        {
            if (obj == null)
            {
                iTotal = iTotal * iConstant;
            }
            else
            {
                if (obj.GetType().IsArray == false)
                {
                    //the simple case, not an array, just the element
                    iTotal = iTotal * iConstant + obj.GetHashCode();
                }
                else
                {
                    //'Switch' on type of array, to dispatch to the correct handler
                    // This handles multi dimensional arrays
                    if (obj is long[])
                    {
                        Append((long[]) obj);
                    }
                    else if (obj is int[])
                    {
                        Append((int[]) obj);
                    }
                    else if (obj is short[])
                    {
                        Append((short[]) obj);
                    }
                    else if (obj is char[])
                    {
                        Append((char[]) obj);
                    }
                    else if (obj is byte[])
                    {
                        Append((byte[]) obj);
                    }
                    else if (obj is double[])
                    {
                        Append((double[]) obj);
                    }
                    else if (obj is float[])
                    {
                        Append((float[]) obj);
                    }
                    else if (obj is bool[])
                    {
                        Append((bool[]) obj);
                    }
                    else
                    {
                        // Not an array of primitives
                        Append((object[]) obj);
                    }
                }
            }
            return this;
        }

        /**
         * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtlong</code>.</p>
         *
         * @param value  the long to add to the &ltcode&gthashCode</code>
         * @return this
         */

        public HashCodeBuilder Append(long value)
        {
            iTotal = iTotal * iConstant + ((int) (value ^ (value >> 32)));
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for an &ltcode&gtint</code>.</p>
     *
     * @param value  the int to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(int value)
        {
            iTotal = iTotal * iConstant + value;
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtshort</code>.</p>
     *
     * @param value  the short to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(short value)
        {
            iTotal = iTotal * iConstant + value;
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtchar</code>.</p>
     *
     * @param value  the char to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(char value)
        {
            iTotal = iTotal * iConstant + value;
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtbyte</code>.</p>
     *
     * @param value  the byte to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(byte value)
        {
            iTotal = iTotal * iConstant + value;
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtdouble</code>.</p>
     *
     * @param value  the double to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(double value)
        {
            return Append(Convert.ToInt64(value));
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtfloat</code>.</p>
     *
     * @param value  the float to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(float value)
        {
            iTotal = iTotal * iConstant + Convert.ToInt32(value); /* Float.floatToIntBits(value);*/
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtboolean</code>.</p>
     *
     * @param value  the boolean to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(bool value)
        {
            iTotal = iTotal * iConstant + (value ? 0 : 1);
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for an &ltcode&gtObject</code> array.</p>
     *
     * @param array  the array to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(object[] array)
        {
            if (array == null)
            {
                iTotal = iTotal * iConstant;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Append(array[i]);
                }
            }
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtlong</code> array.</p>
     *
     * @param array  the array to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(long[] array)
        {
            if (array == null)
            {
                iTotal = iTotal * iConstant;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Append(array[i]);
                }
            }
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for an &ltcode&gtint</code> array.</p>
     *
     * @param array  the array to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(int[] array)
        {
            if (array == null)
            {
                iTotal = iTotal * iConstant;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Append(array[i]);
                }
            }
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtshort</code> array.</p>
     *
     * @param array  the array to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(short[] array)
        {
            if (array == null)
            {
                iTotal = iTotal * iConstant;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Append(array[i]);
                }
            }
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtchar</code> array.</p>
     *
     * @param array  the array to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(char[] array)
        {
            if (array == null)
            {
                iTotal = iTotal * iConstant;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Append(array[i]);
                }
            }
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtbyte</code> array.</p>
     *
     * @param array  the array to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(byte[] array)
        {
            if (array == null)
            {
                iTotal = iTotal * iConstant;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Append(array[i]);
                }
            }
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtdouble</code> array.</p>
     *
     * @param array  the array to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(double[] array)
        {
            if (array == null)
            {
                iTotal = iTotal * iConstant;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Append(array[i]);
                }
            }
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtfloat</code> array.</p>
     *
     * @param array  the array to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(float[] array)
        {
            if (array == null)
            {
                iTotal = iTotal * iConstant;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Append(array[i]);
                }
            }
            return this;
        }

        /**
     * &ltp&gtAppend a &ltcode&gthashCode</code> for a &ltcode&gtboolean</code> array.</p>
     *
     * @param array  the array to add to the &ltcode&gthashCode</code>
     * @return this
     */

        public HashCodeBuilder Append(bool[] array)
        {
            if (array == null)
            {
                iTotal = iTotal * iConstant;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Append(array[i]);
                }
            }
            return this;
        }

        /**
     * &ltp&gtReturn the computed &ltcode&gthashCode</code>.</p>
     *
     * @return &ltcode&gthashCode</code> based on the fields appended
     */

        public int ToHashCode()
        {
            return iTotal;
        }

        private static bool isTransient(FieldInfo fieldInfo)
        {
            return (fieldInfo.Attributes & FieldAttributes.NotSerialized) == FieldAttributes.NotSerialized;
        }
    }
}