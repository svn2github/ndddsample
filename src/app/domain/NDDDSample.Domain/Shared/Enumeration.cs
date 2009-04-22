namespace NDDDSample.Domain.Shared
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    #endregion

    public abstract class Enumeration : IComparable
    {
        private string displayName;
        private string value;

        protected Enumeration(string value)
        {
            this.value = value;
            displayName = value;
        }

        protected Enumeration(string value, string displayName)
        {
            this.value = value;
            this.displayName = displayName;
        }

        public string Value
        {
            get { return value; }
        }

        public string DisplayName
        {
            get { return displayName; }
        }

        #region IComparable Members

        public int CompareTo(object other)
        {
            return Value.CompareTo(((Enumeration) other).Value);
        }

        #endregion

        public override string ToString()
        {
            return DisplayName;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var type = typeof (T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                //var instance = Activator.CreateInstance(typeof(T));
                var locatedValue = info.GetValue(null) as T;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static T FromValue<T>(string value) where T : Enumeration
        {
            var matchingItem = parse<T, string>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }

        private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof (T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }
    }
}