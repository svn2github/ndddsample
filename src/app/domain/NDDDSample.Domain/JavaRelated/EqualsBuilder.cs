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
        // If the fields tested are equals.     
        private bool isEqual;

        // Constructor for EqualsBuilder.
        // Starts off assuming that equals is true
        public EqualsBuilder()
        {
            isEqual = true;
        }

        /// <summary>
        ///  This method uses reflection to determine if the two Object
        /// are equal.
        ///
        /// It uses AccessibleObject.setAccessible to gain access to private
        /// fields. This means that it will throw a security exception if run under
        /// a security manager, if the permissions are not set up correctly. It is also
        /// not as efficient as testing explicitly.
        ///
        /// Transient members will be not be tested, as they are likely derived
        /// fields, and not part of the value of the Object.
        ///
        /// Static fields will not be tested. Superclass fields will be included.        
        /// </summary>
        /// <param name="lhs">this object</param>
        /// <param name="rhs">the other object</param>
        /// <returns>true if the two Objects have tested equals.</returns>
        public static bool ReflectionEquals(Object lhs, Object rhs)
        {
            return ReflectionEquals(lhs, rhs, false, null);
        }

        /// <summary>
        /// This method uses reflection to determine if the two Object
        /// are equal.
        ///
        /// It uses AccessibleObject.setAccessible to gain access to private
        /// fields. This means that it will throw a security exception if run under
        /// a security manager, if the permissions are not set up correctly. It is also
        /// not as efficient as testing explicitly.
        ///
        /// If the TestTransients parameter is set to true, transient
        /// members will be tested, otherwise they are ignored, as they are likely
        /// derived fields, and not part of the value of the Object.
        ///
        /// Static fields will not be tested. Superclass fields will be included.
        /// </summary>
        /// <param name="lhs">this object</param>
        /// <param name="rhs">the other object</param>
        /// <param name="testTransients">whether to include transient fields</param>
        /// <returns>true if the two Objects have tested equals.</returns>
        public static bool ReflectionEquals(Object lhs, Object rhs, bool testTransients)
        {
            return ReflectionEquals(lhs, rhs, testTransients, null);
        }

        /// <summary>
        /// This method uses reflection to determine if the two Object
        /// are equal.
        ///
        /// It uses AccessibleObject.setAccessible to gain access to private
        /// fields. This means that it will throw a security exception if run under
        /// a security manager, if the permissions are not set up correctly. It is also
        /// not as efficient as testing explicitly.
        ///
        /// If the testTransients parameter is set to true, transient
        /// members will be tested, otherwise they are ignored, as they are likely
        /// derived fields, and not part of the value of the Object.
        ///
        /// Static fields will not be included. Superclass fields will be appended
        /// up to and including the specified superclass. A null superclass is treated
        /// as java.lang.Object.
        /// </summary>
        /// <param name="lhs">this object</param>
        /// <param name="rhs">the other object</param>
        /// <param name="testTransients">whether to include transient fields</param>
        /// <param name="reflectUpToClass">the superclass to reflect up to (inclusive), may be null</param>
        /// <returns>true if the two Objects have tested equals.</returns>
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

        /// <summary>
        /// Appends the fields and values defined by the given object of the given Class.
        /// </summary>
        /// <param name="builder">the builder to Append to</param>
        /// <param name="clazz">the class to Append details of</param>
        /// <param name="lhs">the left hand object</param>
        /// <param name="rhs">the right hand object</param>
        /// <param name="useTransients">whether to test transient fields</param>
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
                    } //TODO: atrosin revise for more specific exc.
                    catch (Exception e)
                    {
                        //this can't happen. Would get a Security exception instead
                        //throw a runtime exception in case the impossible happens.
                        throw new Exception("Unexpected IllegalAccessException");
                    }
                }
            }
        }

        /// <summary>
        /// Adds the result of super.equals() to this builder.
        /// </summary>
        /// <param name="superEquals">the result of calling super.equals()</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
        public EqualsBuilder AppendSuper(bool superEquals)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = superEquals;
            return this;
        }

        /// <summary>
        /// Test if two Object are equal using their equals method.
        /// </summary>
        /// <param name="lhs">the left hand object</param>
        /// <param name="rhs">the right hand object</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>        
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

        /// <summary>
        /// Test if two long are equal.
        /// </summary>
        /// <param name="lhs">the left hand long</param>
        /// <param name="rhs">the right hand long</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
        public EqualsBuilder Append(long lhs, long rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /// <summary>
        /// Test if two int are equal.
        /// </summary>
        /// <param name="lhs">the left hand int</param>
        /// <param name="rhs">the right hand int</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
        public EqualsBuilder Append(int lhs, int rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /// <summary>
        /// Test if two short are equal.
        /// </summary>
        /// <param name="lhs">the left hand short</param>
        /// <param name="rhs">the right hand short</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
        public EqualsBuilder Append(short lhs, short rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /// <summary>
        /// Test if two char are equal.
        /// </summary>
        /// <param name="lhs">the left hand char</param>
        /// <param name="rhs">the right hand char</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
        public EqualsBuilder Append(char lhs, char rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /// <summary>
        /// Test if two byte are equal.
        /// </summary>
        /// <param name="lhs">the left hand byte</param>
        /// <param name="rhs">the right hand byte</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
        public EqualsBuilder Append(byte lhs, byte rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /// <summary>
        /// Test if two double are equal by testing that the
        /// pattern of bits returned by doubleToLong are equal.     
        /// This handles NaNs, Infinties, and &ltcode>-0.0.        
        /// It is compatible with the hash code generated by
        /// HashCodeBuilder.
        /// </summary>
        /// <param name="lhs">the left hand double</param>
        /// <param name="rhs">the right hand double</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
        public EqualsBuilder Append(double lhs, double rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /// <summary>
        /// Test if two float are equal byt testing that the
        /// pattern of bits returned by doubleToLong are equal.        
        /// This handles NaNs, Infinties, and &ltcode>-0.0.        
        /// It is compatible with the hash code generated by
        /// HashCodeBuilder.        
        /// </summary>
        /// <param name="lhs">the left hand float</param>
        /// <param name="rhs">the right hand float</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
        public EqualsBuilder Append(float lhs, float rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /// <summary>
        /// Test if two bools are equal.
        /// </summary>
        /// <param name="lhs">the left hand bool</param>
        /// <param name="rhs">the right hand bool</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
        public EqualsBuilder Append(bool lhs, bool rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        /// <summary>
        /// Performs a deep comparison of two Object arrays.        
        /// This also will be called for the top level of
        /// multi-dimensional, ragged, and multi-typed arrays.        
        /// </summary>
        /// <param name="lhs">the left hand Object[]</param>
        /// <param name="rhs">the right hand Object[]</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
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

        /// <summary>
        /// Deep comparison of array of long. Length and all
        /// values are compared.        
        /// The method {@link #Append(long, long)} is used.      
        /// </summary>
        /// <param name="lhs">the left hand long[]</param>
        /// <param name="rhs">the right hand long[]</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
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

        /// <summary>
        /// Deep comparison of array of long. Length and all
        /// values are compared.        
        /// The method {@link #Append(int, int)} is used.    
        /// </summary>
        /// <param name="lhs">the left hand int[]</param>
        /// <param name="rhs">the right hand int[]</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
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

        /// <summary>
        /// Deep comparison of array of long. Length and all
        /// values are compared.        
        /// The method {@link #Append(short, short)} is used.    
        /// </summary>
        /// <param name="lhs">the left hand short[]</param>
        /// <param name="rhs">the right hand short[]</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
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

        /// <summary>
        /// Deep comparison of array of long. Length and all
        /// values are compared.        
        /// The method {@link #Append(char, char)} is used.    
        /// </summary>
        /// <param name="lhs">the left hand char[]</param>
        /// <param name="rhs">the right hand char[]</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
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

        /// <summary>
        /// Deep comparison of array of long. Length and all
        /// values are compared.        
        /// The method {@link #Append(byte, byte)} is used.    
        /// </summary>
        /// <param name="lhs">the left hand byte[]</param>
        /// <param name="rhs">the right hand byte[]</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
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

        /// <summary>
        /// Deep comparison of array of long. Length and all
        /// values are compared.        
        /// The method {@link #Append(double, double)} is used.    
        /// </summary>
        /// <param name="lhs">the left hand double[]</param>
        /// <param name="rhs">the right hand double[]</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
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

        /// <summary>
        /// Deep comparison of array of long. Length and all
        /// values are compared.        
        /// The method {@link #Append(float, float)} is used.    
        /// </summary>
        /// <param name="lhs">the left hand float[]</param>
        /// <param name="rhs">the right hand float[]</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
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

        /// <summary>
        /// Deep comparison of array of long. Length and all
        /// values are compared.        
        /// The method {@link #Append(boolean, boolean)} is used.    
        /// </summary>
        /// <param name="lhs">the left hand boolean[]</param>
        /// <param name="rhs">the right hand boolean[]</param>
        /// <returns>EqualsBuilder - used to chain calls.</returns>
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

        /// <summary>
        /// Return true if the fields that have been checked are all equal.
        /// </summary>
        /// <returns>bool</returns>
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