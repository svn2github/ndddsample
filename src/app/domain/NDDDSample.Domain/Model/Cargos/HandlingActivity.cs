namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using Handlings;
    using Infrastructure.Builders;
    using Infrastructure.Validations;
    using Locations;
    using Shared;
    using Voyages;

    #endregion

    /// <summary>
    /// A handling activity represents how and where a cargo can be handled,
    /// and can be used to express predictions about what is expected to
    /// happen to a cargo in the future.
    /// </summary>
    public class HandlingActivity : IValueObject<HandlingActivity>
    {
        // TODO make HandlingActivity a part of HandlingEvent too? There is some overlap. 
        private readonly Location location;
        private readonly HandlingType type;
        private readonly Voyage voyage;

        #region Constr

        public HandlingActivity(HandlingType type, Location location)
        {
            Validate.NotNull(type, "Handling event type is required");
            Validate.NotNull(location, "Location is required");

            this.type = type;
            this.location = location;
        }

        public HandlingActivity(HandlingType type, Location location, Voyage voyage)
        {
            Validate.NotNull(type, "Handling event type is required");
            Validate.NotNull(location, "Location is required");
            Validate.NotNull(location, "Voyage is required");

            this.type = type;
            this.location = location;
            this.voyage = voyage;
        }

        protected HandlingActivity()
        {
            // Needed by Hibernate
        }

        #endregion

        #region IValueObject<HandlingActivity> Members

        public bool SameValueAs(HandlingActivity other)
        {
            return other != null && new EqualsBuilder().
                                        Append(type, other.type).
                                        Append(location, other.location).
                                        Append(voyage, other.voyage).
                                        IsEquals();
        }

        #endregion

        #region Object's override

        public override int GetHashCode()
        {
            return new HashCodeBuilder().
                Append(type).
                Append(location).
                Append(voyage).
                ToHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }

            var other = (HandlingActivity) obj;

            return SameValueAs(other);
        }

        #endregion

        #region Props

        public HandlingType Type
        {
            get { return type; }
        }

        public Location Location
        {
            get { return location; }
        }

        public Voyage Voyage
        {
            get { return voyage; }
        }

        #endregion
    }
}