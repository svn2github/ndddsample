namespace NDDDSample.Domain.Model.Handlings
{
    #region Usings

    using System;
    using System.Text;
    using Cargos;
    using Locations;
    using Shared;
    using TempHelper;
    using Voyages;

    #endregion

    public sealed class HandlingEvent : IDomainEvent<HandlingEvent>
    {
        private HandlingType type;
        private Voyage voyage;
        private Location location;
        private DateTime completionTime;
        private DateTime registrationTime;
        private Cargo cargo;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cargo">cargo</param>
        /// <param name="completionTime">completion time, the reported time that the event actually happened (e.g. the receive took place).</param>
        /// <param name="registrationTime">registration time, the time the message is received</param>
        /// <param name="eventType">type of event</param>
        /// <param name="location">where the event took place</param>
        /// <param name="voyage">the voyage</param>
        public HandlingEvent(Cargo cargo,
                             DateTime completionTime,
                             DateTime registrationTime,
                             HandlingType eventType,
                             Location location,
                             Voyage voyage)
        {
            Validate.notNull(cargo, "Cargo is required");
            Validate.notNull(completionTime, "Completion time is required");
            Validate.notNull(registrationTime, "Registration time is required");
            Validate.notNull(eventType, "Handling event eventType is required");
            Validate.notNull(location, "Location is required");
            Validate.notNull(voyage, "Voyage is required");

            if (eventType.ProhibitsVoyage())
            {
                throw new ArgumentException("Voyage is not allowed with event eventType " + eventType);
            }

            this.voyage = voyage;
            //TODO: atrosin revise (DateTime) completionTime.Clone(); and (DateTime) registrationTime.clone();
            this.completionTime = completionTime;
            this.registrationTime = registrationTime;
            this.type = eventType;
            this.location = location;
            this.cargo = cargo;
        }

        public HandlingEvent(Cargo cargo,
                       DateTime completionTime,
                       DateTime registrationTime,
                       HandlingType type,
                       Location location)
        {
            Validate.notNull(cargo, "Cargo is required");
            Validate.notNull(completionTime, "Completion time is required");
            Validate.notNull(registrationTime, "Registration time is required");
            Validate.notNull(type, "Handling event type is required");
            Validate.notNull(location, "Location is required");

            if (type.RequiresVoyage())
            {
                throw new ArgumentException("Voyage is required for event type " + type);
            }

            //TODO: atrosin revise (DateTime) completionTime.Clone(); and (DateTime) registrationTime.clone();
            this.completionTime = completionTime;
            this.registrationTime = registrationTime;
            this.type = type;
            this.location = location;
            this.cargo = cargo;
            this.voyage = null;
        }

        public HandlingType Type()
        {
            return this.type;
        }

        public Voyage Voyage()
        {
            return DomainObjectUtils.nullSafe(this.voyage, Voyage.NONE);
        }

        public DateTime CompletionTime()
        {
            //TODO: atrosin revise translation new Date(this.completionTime.getTime());
            return this.completionTime;
        }

        public DateTime RegistrationTime()
        {
            //TODO: atrosin revise new Date(this.registrationTime.getTime());
            return this.registrationTime;
        }

        public Location Location()
        {
            return this.location;
        }

        public Cargo Cargo()
        {
            return this.cargo;
        }

        public bool SameEventAs(HandlingEvent other)
        {
            return other != null && new EqualsBuilder().
              Append(this.cargo, other.cargo).
              Append(this.voyage, other.voyage).
              Append(this.completionTime, other.completionTime).
              Append(this.location, other.location).
              Append(this.type, other.type).
              IsEquals();
        }

        public override bool Equals(object obj)
        {

            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            HandlingEvent eventType = (HandlingEvent)obj;

            return SameEventAs(eventType);
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder().
              Append(cargo).
              Append(voyage).
              Append(completionTime).
              Append(location).
              Append(type).
              ToHashCode();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("\n--- Handling event ---\n").
             Append("Cargo: ").Append(cargo.TrackingId()).Append("\n").
             Append("Type: ").Append(type).Append("\n").
             Append("Location: ").Append(location.Name()).Append("\n").
             Append("Completed on: ").Append(completionTime).Append("\n").
             Append("Registered on: ").Append(registrationTime).Append("\n");

            if (voyage != null)
            {
                builder.Append("Voyage: ").Append(voyage.VoyageNumber()).Append("\n");
            }

            return builder.ToString();
        }

        private HandlingEvent()
        {
            // Needed by Hibernate
        }


        // Auto-generated surrogate key
        private long id;


        #region Nested HandlingType

        /// <summary>
        ///Handling event type. Either requires or prohibits a carrier movement
        ///association, it's never optional.
        /// </summary>
        public class HandlingType : IValueObject<HandlingType>
        {
            public static readonly HandlingType LOAD = new HandlingType(true);
            public static readonly HandlingType UNLOAD = new HandlingType(true);
            public static readonly HandlingType RECEIVE = new HandlingType(false);
            public static readonly HandlingType CLAIM = new HandlingType(false);
            public static readonly HandlingType CUSTOMS = new HandlingType(false);

            private readonly bool voyageRequired;


            /// <summary>
            /// Private enum constructor        
            /// </summary>
            /// <param name="voyageRequired">voyageRequired whether or not a voyage is associated with this event type </param>
            private HandlingType(bool voyageRequired)
            {
                this.voyageRequired = voyageRequired;
            }

            /// <summary>
            /// return True if a voyage association is required for this event type.
            /// </summary>
            /// <returns></returns>
            public bool RequiresVoyage()
            {
                return voyageRequired;
            }
            /// <summary>
            /// return True if a voyage association is prohibited for this event type.
            /// </summary>
            /// <returns></returns>
            public bool ProhibitsVoyage()
            {
                return !RequiresVoyage();
            }

            /// <summary>
            /// Value objects compare by the values of their attributes, they don't have an identity.
            /// </summary>
            /// <param name="other">The other value object.</param>
            /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
            public bool SameValueAs(HandlingType other)
            {
                return other != null && this.Equals(other);
            }

        }

        #endregion

    }
}