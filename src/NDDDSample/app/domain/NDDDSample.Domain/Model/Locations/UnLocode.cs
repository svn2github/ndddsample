namespace NDDDSample.Domain.Model.Locations
{
    #region Usings

    using System.Text.RegularExpressions;
    using Infrastructure.Validations;
    using Shared;

    #endregion

    /// <summary>
    /// United nations location code.
    /// http://www.unece.org/cefact/locode/
    /// http://www.unece.org/cefact/locode/DocColumnDescription.htm#LOCODE</summary>
    public class UnLocode : IValueObject<UnLocode>
    {
        // Country code is exactly two letters.
        // Location code is usually three letters, but may contain the numbers 2-9 as well
        private static readonly Regex VALID_PATTERN = new Regex("[a-zA-Z]{2}[a-zA-Z2-9]{3}", RegexOptions.Compiled);
        private readonly string unlocode;

        #region Constr

        /// <summary>
        ///  Constructor.
        /// </summary>
        /// <param name="countryAndLocation">Location string</param>
        public UnLocode(string countryAndLocation)
        {
            Validate.NotNull(countryAndLocation, "Country and location may not be null");
            Validate.IsTrue(VALID_PATTERN.Match(countryAndLocation).Success,
                            countryAndLocation + " is not a valid UN/LOCODE (does not match pattern)");

            unlocode = countryAndLocation.ToUpper();
        }

        protected UnLocode()
        {
            // Needed by Hibernate
        }

        #endregion

        /// <summary>
        /// Voyage code and location code concatenated, always upper case.
        /// </summary>
        public string IdString
        {
            get { return unlocode; }
        }

        #region IValueObject<UnLocode> Members

        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        public bool SameValueAs(UnLocode other)
        {
            return other != null && unlocode.Equals(other.unlocode);
        }

        #endregion

        #region Object's Override 

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (UnLocode) obj;

            return SameValueAs(other);
        }

        public override int GetHashCode()
        {
            return unlocode.GetHashCode();
        }

        public override string ToString()
        {
            return IdString;
        }

        #endregion
    }
}