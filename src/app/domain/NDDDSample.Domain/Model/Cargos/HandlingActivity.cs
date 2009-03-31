namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using Handlings;
    using JavaRelated;
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
        private readonly HandlingEvent.HandlingType type;
        private readonly Voyage voyage;

        public HandlingActivity(HandlingEvent.HandlingType type, Location location)
        {
            Validate.notNull(type, "Handling event type is required");
            Validate.notNull(location, "Location is required");

            this.type = type;
            this.location = location;
        }

        public HandlingActivity(HandlingEvent.HandlingType type, Location location, Voyage voyage)
        {
            Validate.notNull(type, "Handling event type is required");
            Validate.notNull(location, "Location is required");
            Validate.notNull(location, "Voyage is required");

            this.type = type;
            this.location = location;
            this.voyage = voyage;
        }

        private HandlingActivity()
        {
            // Needed by Hibernate
        }

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

        public bool Equals(object obj)
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

        public HandlingEvent.HandlingType Type()
        {
            return type;
        }

        public Location Location()
        {
            return location;
        }

        public Voyage Voyage()
        {
            return voyage;
        }
    }
}