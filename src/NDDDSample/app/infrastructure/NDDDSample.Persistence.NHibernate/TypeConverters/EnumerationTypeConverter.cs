#region Usings

using IUserType=NHibernate.UserTypes.IUserType;
using NHibernateUtil=NHibernate.NHibernateUtil;
using SqlType=NHibernate.SqlTypes.SqlType;
using SqlTypeFactory=NHibernate.SqlTypes.SqlTypeFactory;

#endregion

namespace NDDDSample.Persistence.NHibernate.TypeConverters
{
    #region Usings

    using System;
    using System.Data;
    using Domain.Shared;

    #endregion

    /// <summary>
    /// The class is used to map Enumeration derived classes to DB.
    /// The class is used by NHibernate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumerationTypeConverter<T> : IUserType
        where T : Enumeration
    {
        #region Implementation of IUserType

        public new bool Equals(object x, object y)
        {
            bool returnvalue = false;
            if ((x != null) && (y != null))
            {
                returnvalue = x.Equals(y);
            }
            return returnvalue;
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var enumName = NHibernateUtil.String.NullSafeGet(rs, names) as string;

            if (string.IsNullOrEmpty(enumName))
            {
                return null;
            }

            object res = Enumeration.FromValue<T>(enumName);
            return res;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            var id = value as Enumeration;

            if (id == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);
            }
            else
            {
                NHibernateUtil.String.NullSafeSet(cmd, id.Value, index);
            }
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }

        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }

        public SqlType[] SqlTypes
        {
            get { return new[] {SqlTypeFactory.GetString(25)}; }
        }

        public Type ReturnedType
        {
            get { return typeof (T); }
        }

        public bool IsMutable
        {
            get { return false; }
        }

        #endregion
    }
}