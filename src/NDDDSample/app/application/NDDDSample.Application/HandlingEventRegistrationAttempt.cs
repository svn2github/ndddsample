namespace NDDDSample.Application
{
    #region Usings

    using System;
    using System.Runtime.Serialization;
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
    [DataContract]
    public sealed class HandlingEventRegistrationAttempt
    {
        private readonly DateTime completionTime;
        private readonly DateTime registrationTime;
        private readonly TrackingId trackingId;
        private readonly HandlingType type;
        private readonly UnLocode unLocode;
        private readonly VoyageNumber voyageNumber;

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
            get { return completionTime; }
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

        public DateTime RegistrationTime
        {
            get { return registrationTime; }
        }


        public override string ToString()
        {
            return ToStringBuilder.ReflectionToString(this);
        }
    }
}