namespace NDDDSample.Web.Controllers.Tracking
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Domain.Model.Locations;

    #endregion

    /// <summary>
    /// View adapter for displaying a cargo in a tracking context.
    /// </summary>
    public sealed class CargoTrackingViewAdapter
    {
        private readonly Cargo cargo;       
        private readonly List<HandlingEventViewAdapter> events;
        private const string FORMAT = "yyyy-MM-dd hh:mm";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cargo">cargo</param>        
        /// <param name="handlingEvents">handlingEvents</param>
        public CargoTrackingViewAdapter(Cargo cargo, ICollection<HandlingEvent> handlingEvents)
        {                  
            this.cargo = cargo;

            events = new List<HandlingEventViewAdapter>(handlingEvents.Count);
            foreach (HandlingEvent handlingEvent in handlingEvents)
            {
                events.Add(new HandlingEventViewAdapter(handlingEvent, cargo));
            }
        }

        /// <summary>
        /// Display Text
        /// </summary>
        /// <param name="location">a location</param>
        /// <returns>A formatted string for displaying the location.</returns>
        private string GetDisplayText(Location location)
        {
            return location.Name;
        }

        /// <summary>
        /// An unmodifiable list of handling event view adapters.
        /// </summary>
        public IList<HandlingEventViewAdapter> Events
        {
            get { return new List<HandlingEventViewAdapter>(events).AsReadOnly(); }
        }

        /// <summary>
        /// Translate cargo status.
        /// </summary>
        /// <returns>A translated string describing the cargo status.</returns>
        public string GetStatusText()
        {
            Delivery delivery = cargo.Delivery;
            string code = "cargo.status." + delivery.TransportStatus.DisplayName;

            object[] args = null;

            if (delivery.TransportStatus == TransportStatus.IN_PORT)
            {
                args = new object[] {GetDisplayText(delivery.LastKnownLocation)};
            }
            else if (delivery.TransportStatus == TransportStatus.ONBOARD_CARRIER)
            {
                args = new object[] {delivery.CurrentVoyage.VoyageNumber.IdString};
            }
            else if (delivery.TransportStatus == TransportStatus.CLAIMED
                     || delivery.TransportStatus == TransportStatus.NOT_RECEIVED
                     || delivery.TransportStatus == TransportStatus.UNKNOWN)
            {
                args = null;
            }

            return MessageSource.GetMessage(code, args, "[Unknown status]");
        }

        /// <summary>
        /// Cargo destination location.
        /// </summary>
        public string Destination
        {
            get { return GetDisplayText(cargo.RouteSpecification.Destination); }
        }


        /// <summary>
        /// Cargo origin location.
        /// </summary>
        public string Origin
        {
            get { return GetDisplayText(cargo.Origin); }
        }

        /// <summary>
        /// Cargo tracking id.
        /// </summary>
        public string TrackingId
        {
            get { return cargo.TrackingId.IdString; }
        }

        public string Eta
        {
            get
            {
                DateTime eta = cargo.Delivery.EstimatedTimeOfArrival;

                if (eta == Delivery.ETA_UNKOWN)
                {
                    return "?";
                }

                return eta.ToString(FORMAT);
            }
        }

        public String GetNextExpectedActivity()
        {
            HandlingActivity activity = cargo.Delivery.NextExpectedActivity;
            if (activity == null)
            {
                return "";
            }

            //TODO: atrosin refactor repetead string format\concatination
            string text = "Next expected activity is to ";
            HandlingType type = activity.Type;
            if (type.SameValueAs(HandlingType.LOAD))
            {
                return
                  text + type.DisplayName.ToLower() + " cargo into voyage " + activity.Voyage.VoyageNumber +
                  " in " + activity.Location.Name;
            }
            else if (type.SameValueAs(HandlingType.UNLOAD))
            {
                return
                  text + type.DisplayName.ToLower() + " cargo off of " + activity.Voyage.VoyageNumber +
                  " in " + activity.Location.Name;
            }
            else
            {
                return text + type.DisplayName.ToLower() + " cargo in " + activity.Location.Name;
            }
        }

        /// <summary>
        /// True if cargo is misdirected.
        /// </summary>
        public bool IsMisdirected
        {
            get { return cargo.Delivery.IsMisdirected; }
        }
    }
}