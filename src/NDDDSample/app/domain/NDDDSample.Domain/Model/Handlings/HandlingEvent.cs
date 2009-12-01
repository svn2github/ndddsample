namespace NDDDSample.Domain.Model.Handlings
{
    #region Usings

    using System;
    using System.Text;
    using Cargos;
    using Infrastructure.Builders;
    using Infrastructure.Validations;
    using Locations;
    using Shared;
    using Voyages;

    #endregion

    public class HandlingEvent : IDomainEvent<HandlingEvent>
    {
        #region Private Props

        private readonly Cargo cargo;
        private readonly DateTime completionTime;
        private readonly Location location;
        private readonly DateTime registrationTime;
        private readonly HandlingType type;
        private readonly Voyage voyage;
        private int id;

        #endregion

        #region Constr

        protected HandlingEvent()
        {
            // Needed by Hibernate
        }

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

        #endregion

        #region IDomainEvent<HandlingEvent> Members

        public virtual bool SameEventAs(HandlingEvent other)
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

        public virtual HandlingType Type
        {
            get { return type; }
        }

        public virtual Voyage Voyage
        {
            get { return DomainObjectUtils.NullSafe(voyage, Voyage.NONE); }
        }

        public virtual DateTime CompletionTime
        {
            get { return completionTime; }
        }

        public virtual DateTime RegistrationTime
        {
            get { return registrationTime; }
        }

        public virtual Location Location
        {
            get { return location; }
        }

        public virtual Cargo Cargo
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