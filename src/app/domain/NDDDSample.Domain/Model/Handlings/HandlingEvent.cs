namespace NDDDSample.Domain.Model.Handlings
{
    #region Usings

    using System;
    using System.Text;
    using Cargos;
    using JavaRelated;
    using Locations;
    using Shared;
    using Voyages;

    #endregion

    public sealed class HandlingEvent : IDomainEvent<HandlingEvent>
    {
        #region Private Props

        private Cargo cargo;
        private DateTime completionTime;
        private Location location;
        private DateTime registrationTime;
        private HandlingType type;
        private Voyage voyage;
        private Guid id;

        #endregion

        #region Nested HandlingType

        /// <summary>
        ///Handling event type. Either requires or prohibits a carrier movement
        ///association, it's never optional.
        /// </summary>
        public class HandlingType : IValueObject<HandlingType>
        {
            public static readonly HandlingType CLAIM = new HandlingType(false);
            public static readonly HandlingType CUSTOMS = new HandlingType(false);
            public static readonly HandlingType LOAD = new HandlingType(true);
            public static readonly HandlingType RECEIVE = new HandlingType(false);
            public static readonly HandlingType UNLOAD = new HandlingType(true);

            private readonly bool voyageRequired;


            /// <summary>
            /// Private enum constructor        
            /// </summary>
            /// <param name="voyageRequired">voyageRequired whether or not a voyage is associated with this event type </param>
            private HandlingType(bool voyageRequired)
            {
                this.voyageRequired = voyageRequired;
            }

            #region IValueObject<HandlingType> Members

            /// <summary>
            /// Value objects compare by the values of their attributes, they don't have an identity.
            /// </summary>
            /// <param name="other">The other value object.</param>
            /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
            public bool SameValueAs(HandlingType other)
            {
                return other != null && Equals(other);
            }

            #endregion

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
        }

        #endregion

        #region Constr

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
            Validate.NotNull(cargo, "Cargo is required");
            Validate.NotNull(completionTime, "Completion time is required");
            Validate.NotNull(registrationTime, "Registration time is required");
            Validate.NotNull(eventType, "Handling event eventType is required");
            Validate.NotNull(location, "Location is required");
            Validate.NotNull(voyage, "Voyage is required");

            if (eventType.ProhibitsVoyage())
            {
                throw new ArgumentException("Voyage is not allowed with event eventType " + eventType);
            }

            this.voyage = voyage;            
            this.completionTime = completionTime;
            this.registrationTime = registrationTime;
            type = eventType;
            this.location = location;
            this.cargo = cargo;
        }

        public HandlingEvent(Cargo cargo,
                             DateTime completionTime,
                             DateTime registrationTime,
                             HandlingType type,
                             Location location)
        {
            Validate.NotNull(cargo, "Cargo is required");
            Validate.NotNull(completionTime, "Completion time is required");
            Validate.NotNull(registrationTime, "Registration time is required");
            Validate.NotNull(type, "Handling event type is required");
            Validate.NotNull(location, "Location is required");

            if (type.RequiresVoyage())
            {
                throw new ArgumentException("Voyage is required for event type " + type);
            }
           
            this.completionTime = completionTime;
            this.registrationTime = registrationTime;
            this.type = type;
            this.location = location;
            this.cargo = cargo;
            voyage = null;
        }

        private HandlingEvent()
        {
            // Needed by Hibernate
        }

        #endregion

        #region IDomainEvent<HandlingEvent> Members

        public bool SameEventAs(HandlingEvent other)
        {
            return other != null && new EqualsBuilder().
                                        Append(cargo, other.cargo).
                                        Append(voyage, other.voyage).
                                        Append(completionTime, other.completionTime).
                                        Append(location, other.location).
                                        Append(type, other.type).
                                        IsEquals();
        }

        #endregion

        #region Public Props

        public HandlingType Type
        {
            get { return type; }
        }

        public Voyage Voyage
        {
            get { return DomainObjectUtils.NullSafe(voyage, Voyage.NONE); }
        }

        public DateTime CompletionTime
        {
            get
            {             
                return completionTime;
            }
        }

        public DateTime RegistrationTime
        {
            get
            {               
                return registrationTime;
            }
        }

        public Location Location
        {
            get { return location; }
        }

        public Cargo Cargo
        {
            get { return cargo; }
        }

        #endregion

        #region Object's override

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

            var eventType = (HandlingEvent) obj;

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
                Append("Cargo: ").Append(cargo.TrackingId).Append("\n").
                Append("Type: ").Append(type).Append("\n").
                Append("Location: ").Append(location.Name).Append("\n").
                Append("Completed on: ").Append(completionTime).Append("\n").
                Append("Registered on: ").Append(registrationTime).Append("\n");

            if (voyage != null)
            {
                builder.Append("Voyage: ").Append(voyage.VoyageNumber).Append("\n");
            }

            return builder.ToString();
        }

        #endregion
    }
}