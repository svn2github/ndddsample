namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Handlings;
    using Infrastructure.Utils;
    using Infrastructure.Validations;
    using Locations;
    using Shared;

    #endregion

    /// <summary>
    /// An itinerary - plan, path: an established line of travel.
    /// </summary>
    public class Itinerary : IValueObject<Itinerary>
    {
        internal static readonly Itinerary EMPTY_ITINERARY = new Itinerary();
        private static readonly DateTime END_OF_DAYS = DateTime.MaxValue;
        private readonly IList<Leg> legs = new List<Leg>();
        private int id;

        #region Constr

        public Itinerary(IList<Leg> legs)
        {
            Validate.NotEmpty(legs);
            Validate.NoNullElements(legs);

            this.legs = new List<Leg>(legs);
        }

        //TODO: atrosin, revise the constructor, it is public only for negative test purpose
        public Itinerary()
        {           
            // Needed by Hibernate
        }

        #endregion

        #region IValueObject<Itinerary> Members

        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        public bool SameValueAs(Itinerary other)
        {
            return other != null && legs.Equals(other.legs);
        }

        #endregion

        #region Props

        /// <summary>
        /// the legs of this itinerary, as an <b>immutable</b> list.
        /// </summary>
        public IList<Leg> Legs
        {
            get { return new List<Leg>(legs).AsReadOnly(); }
        }

        /// <summary>
        /// The initial departure location.
        /// </summary>
        internal Location InitialDepartureLocation
        {
            get
            {
                if (legs.IsEmpty())
                {
                    return Location.UNKNOWN;
                }
                return legs[0].LoadLocation;
            }
        }

        /// <summary>
        /// Date when cargo arrives at final destination.
        /// </summary>
        internal Location FinalArrivalLocation
        {
            get
            {
                if (legs.IsEmpty())
                {
                    return Location.UNKNOWN;
                }
                return LastLeg.UnloadLocation;
            }
        }

        /// <summary>
        /// Date when cargo arrives at final destination.
        /// </summary>
        internal DateTime FinalArrivalDate
        {
            get
            {
                Leg lastLeg = LastLeg;

                if (lastLeg == null)
                {
                    return END_OF_DAYS;
                }

                return lastLeg.UnloadTime;
            }
        }

        /// <summary>
        /// The last leg on the itinerary.
        /// </summary>
        private Leg LastLeg
        {
            get
            {
                if (legs.IsEmpty())
                {
                    return null;
                }
                return legs[legs.Count - 1];
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///  Test if the given handling event is expected when executing this itinerary.
        /// </summary>
        /// <param name="handlingEvent">event Event to test.</param>
        /// <returns>true if the event is expected</returns>
        public bool IsExpected(HandlingEvent handlingEvent)
        {
            //TODO: atrosin revise the logic if it is transl corectlly
            if (legs.IsEmpty())
            {
                return true;
            }

            if (handlingEvent.Type == HandlingType.RECEIVE)
            {
                //Check that the first leg's origin is the event's location
                Leg leg = legs[0];
                return leg.LoadLocation.Equals(handlingEvent.Location);
            }

            if (handlingEvent.Type == HandlingType.LOAD)
            {
                //Check that the there is one leg with same load location and voyage
                foreach (Leg leg in legs)
                {
                    if (leg.LoadLocation.SameIdentityAs(handlingEvent.Location) &&
                        leg.Voyage.SameIdentityAs(handlingEvent.Voyage))
                    {
                        return true;
                    }
                }
                return false;
            }

            if (handlingEvent.Type == HandlingType.UNLOAD)
            {
                //Check that the there is one leg with same unload location and voyage
                foreach (Leg leg in legs)
                {
                    if (leg.UnloadLocation.Equals(handlingEvent.Location) &&
                        leg.Voyage.Equals(handlingEvent.Voyage))
                    {
                        return true;
                    }
                }
                return false;
            }

            if (handlingEvent.Type == HandlingType.CLAIM)
            {
                //Check that the last leg's destination is from the event's location
                Leg leg = LastLeg;
                return (leg.UnloadLocation.Equals(handlingEvent.Location));
            }

            //HandlingEvent.Type.CUSTOMS;
            return true;
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

            var itinerary = (Itinerary) obj;

            return SameValueAs(itinerary);
        }

        public override int GetHashCode()
        {
            //TODO: atrosin ensure that hashcode is returned correctly: java version legs.hashCode();
            return legs.GetHashCode();
        }

        #endregion
    }
}