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
    using System;
    using System.Collections.Generic;

    public class HashCodeBuilder
    {
        public HashCodeBuilder()
        {}

        public HashCodeBuilder(object i, object j)
        {
            throw new NotImplementedException();
        }

        public HashCodeBuilder Append<T>(IList<T> movements)
        {
            throw new NotImplementedException();
        }

        public int ToHashCode()
        {
            return 0;
        }

        internal HashCodeBuilder Append(object obj)
        {
            throw new NotImplementedException();
        }

        public static int reflectionHashCode(object testObject)
        {
            throw new NotImplementedException();
        }
        
        public static int reflectionHashCode(object testObject, bool obj2)
        {
            throw new NotImplementedException();
        }

        public static int reflectionHashCode(object testObject, object obj2, bool boolParam)
        {
            throw new NotImplementedException();
        }

        public static int reflectionHashCode(object testObject, object obj2, object obj3, bool boolParam)
        {
            throw new NotImplementedException();
        }

        public HashCodeBuilder append(object o)
        {
            return this;
        }

        public HashCodeBuilder appendSuper(object o)
        {
            throw new NotImplementedException();
        }

        public int toHashCode()
        {
            throw new NotImplementedException();
        }
    }
}