namespace NDDDSample.Domain.Model.Voyages
{
    #region Usings

    using System;
    using Infrastructure.Validations;
    using Shared;

    #endregion

    /// <summary>
    /// Identifies a voyage.
    /// </summary>
    public class VoyageNumber : IValueObject<VoyageNumber>
    {
        private readonly string number;

        #region Constr

        public VoyageNumber(string number)
        {
            Validate.NotNull(number);

            this.number = number;
        }

        protected VoyageNumber()
        {
            // Needed by Hibernate
        }

        #endregion

        public string IdString
        {
            get { return number; }
        }

        #region IValueObject<VoyageNumber> Members

        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        public bool SameValueAs(VoyageNumber other)
        {
            return other != null && number.Equals(other.number);
        }

        #endregion

        #region Object's override

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (!(obj is VoyageNumber))
            {
                return false;
            }

            var other = (VoyageNumber) obj;

            return SameValueAs(other);
        }


        public override int GetHashCode()
        {
            return number.GetHashCode();
        }

        public override string ToString()
        {
            return number;
        }

        #endregion
    }
}