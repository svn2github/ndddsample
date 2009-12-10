namespace NDDDSample.Web.Controllers.Tracking
{
    #region Usings

    using System;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Domain.Model.Voyages;

    #endregion

    public sealed class HandlingEventViewAdapter
    {
        private readonly HandlingEvent handlingEvent;
        private readonly Cargo cargo;
        public const string FORMAT = "yyyy-MM-dd hh:mm";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="handlingEvent"> handling event</param>
        /// <param name="cargo">Cargo</param>
        public HandlingEventViewAdapter(HandlingEvent handlingEvent, Cargo cargo)
        {
            this.handlingEvent = handlingEvent;
            this.cargo = cargo;
        }

        /// <summary>
        /// Location where the event occurred.
        /// </summary>
        public string Location
        {
            get { return handlingEvent.Location.Name; }
        }


        /// <summary>
        /// Time when the event was completed.
        /// </summary>
        public string Time
        {
            get { return handlingEvent.CompletionTime.ToString(FORMAT); }
        }

        /// <summary>
        /// Type of event.
        /// </summary>
        public string Type
        {
            get { return handlingEvent.Type.ToString(); }
        }


        /// <summary>
        /// Voyage number, or empty string if not applicable.
        /// </summary>
        public string VoyageNumber
        {
            get
            {
                Voyage voyage = handlingEvent.Voyage;
                return voyage.VoyageNumber.IdString;
            }
        }


        /// <summary>
        /// True if the event was expected, according to the cargo's itinerary.
        /// </summary>
        public bool IsExpected
        {
            get { return cargo.Itinerary.IsExpected(handlingEvent); }
        }

        public string Description
        {
            get
            {
                var args = new object[] {};

                if (HandlingType.LOAD == handlingEvent.Type
                    || HandlingType.UNLOAD == handlingEvent.Type)
                {
                    args = new Object[]
                               {
                                   handlingEvent.Voyage.VoyageNumber.IdString,
                                   handlingEvent.Location.Name,
                                   handlingEvent.CompletionTime
                               };
                }
                else if (HandlingType.RECEIVE == handlingEvent.Type
                         || HandlingType.CLAIM == handlingEvent.Type)
                {
                    args = new Object[]
                               {
                                   handlingEvent.Location.Name,
                                   handlingEvent.CompletionTime
                               };
                }

                string key = "deliveryHistory.eventDescription." + handlingEvent.Type.DisplayName;

                return MessageSource.GetMessage(key, args);
            }
        }
    }
}