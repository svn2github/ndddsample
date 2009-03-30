namespace NDDDSample.Domain.Model.Cargos
{
    using Handlings;
    using Locations;
    using Shared;
    using TempHelper;
    using Voyages;

    /// <summary>
    /// A handling activity represents how and where a cargo can be handled,
    /// and can be used to express predictions about what is expected to
    /// happen to a cargo in the future.
    /// </summary>
    public class HandlingActivity : IValueObject<HandlingActivity>
    {
        // TODO make HandlingActivity a part of HandlingEvent too? There is some overlap. 

        private HandlingEvent.HandlingType type;
        private Location location;
        private Voyage voyage;

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

        public bool SameValueAs(HandlingActivity other)
        {
            return other != null && new EqualsBuilder().
              Append(this.type, other.type).
              Append(this.location, other.location).
              Append(this.voyage, other.voyage).
              IsEquals();
        }

        #region Object's override

        public override int GetHashCode()
        {
            return new HashCodeBuilder().
                Append(this.type).
                Append(this.location).
                Append(this.voyage).
                ToHashCode();
        }

        public bool Equals(object obj)
        {
            if (obj == this) return true;
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;

            var other = (HandlingActivity)obj;

            return SameValueAs(other);
        }

        #endregion


        HandlingActivity()
        {
            // Needed by Hibernate
        }
    }
}
