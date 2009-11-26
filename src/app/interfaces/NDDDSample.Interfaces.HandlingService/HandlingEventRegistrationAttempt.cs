namespace NDDDSample.Interfaces.HandlingService
{
    #region Usings

    using System;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;
    using Infrastructure.Builders;

    #endregion

    /// <summary>
    /// This is a simple transfer object for passing incoming handling event
    /// registration attempts to proper the registration procedure.
    /// It is used as a message queue element. 
    /// </summary>
    [Serializable]
    public class HandlingEventRegistrationAttempt
    {
        private readonly DateTime registrationTime;
        private readonly DateTime completionTime;
        private readonly TrackingId trackingId;
        private readonly VoyageNumber voyageNumber;
        private readonly HandlingType type;
        private readonly UnLocode unLocode;

        public HandlingEventRegistrationAttempt(DateTime registrationDate,
                                                DateTime completionDate,
                                                TrackingId trackingId,
                                                VoyageNumber voyageNumber,
                                                HandlingType type,
                                                UnLocode unLocode)
        {
            registrationTime = registrationDate;
            completionTime = completionDate;
            this.trackingId = trackingId;
            this.voyageNumber = voyageNumber;
            this.type = type;
            this.unLocode = unLocode;
        }

        public DateTime CompletionTime
        {
            get { return completionTime.ToUniversalTime(); }
        }

        public DateTime RegistrationTime
        {
            get { return registrationTime; }
        }

        public TrackingId TrackingId
        {
            get { return trackingId; }
        }

        public VoyageNumber VoyageNumber
        {
            get { return voyageNumber; }
        }

        public HandlingType Type
        {
            get { return type; }
        }

        public UnLocode UnLocode
        {
            get { return unLocode; }
        }

        public override string ToString()
        {
            return ToStringBuilder.ReflectionToString(this);
        }
    }
}