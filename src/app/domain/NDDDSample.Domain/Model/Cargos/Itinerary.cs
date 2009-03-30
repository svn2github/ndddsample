namespace NDDDSample.Domain.Model.Cargos
{
    using System;
    using Handlings;
    using Locations;
    using Shared;
    using System.Collections.Generic;
    using TempHelper;

    /// <summary>
    /// An itinerary.
    /// </summary>
    public class Itinerary : IValueObject<Itinerary>
    {
        private readonly List<Leg> legs = new List<Leg>();
        static Itinerary EMPTY_ITINERARY = new Itinerary();
        private static readonly DateTime END_OF_DAYS = DateTime.MaxValue;

        public Itinerary(List<Leg> legs)
        {
            Validate.notEmpty(legs);
            Validate.noNullElements(legs);

            this.legs = legs;
        }

        /// <summary>
        /// the legs of this itinerary, as an <b>immutable</b> list.
        /// </summary>
        /// <returns></returns>
        public IList<Leg> Legs()
        {
            return new List<Leg>(legs).AsReadOnly();
        }

        /// <summary>
        ///  Test if the given handling event is expected when executing this itinerary.
        /// </summary>
        /// <param name="handlingEvent">event Event to test.</param>
        /// <returns>true if the event is expected</returns>
        public bool IsExpected(HandlingEvent handlingEvent)
        {
            if (legs.IsEmpty())
            {
                return true;
            }

            if (handlingEvent.Type() == HandlingEvent.HandlingType.RECEIVE)
            {
                //Check that the first leg's origin is the event's location
                Leg leg = legs[0];
                return (leg.LoadLocation().Equals(handlingEvent.Location()));
            }

            if (handlingEvent.Type() == HandlingEvent.HandlingType.LOAD)
            {
                //Check that the there is one leg with same load location and voyage
                foreach (Leg leg in legs)
                {
                    if (leg.LoadLocation().SameIdentityAs(handlingEvent.Location()) &&
                        leg.Voyage().SameIdentityAs(handlingEvent.Voyage()))
                        return true;
                }
                return false;
            }

            if (handlingEvent.Type() == HandlingEvent.HandlingType.UNLOAD)
            {
                //Check that the there is one leg with same unload location and voyage
                foreach (Leg leg in legs)
                {
                    if (leg.UnloadLocation().Equals(handlingEvent.Location()) &&
                        leg.Voyage().Equals(handlingEvent.Voyage()))
                        return true;
                }
                return false;
            }

            if (handlingEvent.Type() == HandlingEvent.HandlingType.CLAIM)
            {
                //Check that the last leg's destination is from the event's location
                Leg leg = LastLeg();
                return (leg.UnloadLocation().Equals(handlingEvent.Location()));
            }

            //HandlingEvent.Type.CUSTOMS;
            return true;
        }

        /// <summary>
        /// The initial departure location.
        /// </summary>
        /// <returns></returns>
        Location InitialDepartureLocation()
        {
            if (legs.IsEmpty())
            {
                return Location.UNKNOWN;
            }
            return legs[0].LoadLocation();
        }

        /// <summary>
        /// Date when cargo arrives at final destination.
        /// </summary>
        /// <returns></returns>
        Location FinalArrivalLocation()
        {
            if (legs.IsEmpty())
            {
                return Location.UNKNOWN;
            }
            return LastLeg().UnloadLocation();
        }

        /// <summary>
        /// Date when cargo arrives at final destination.
        /// </summary>
        /// <returns></returns>
        internal DateTime FinalArrivalDate()
        {
            Leg lastLeg = LastLeg();

            if (lastLeg == null)
            {
                //TODO: atrosin revise translation new Date(END_OF_DAYS.getTime());
                return END_OF_DAYS;
            }

            //TODO: atrosin revise translation new Date(LastLeg.unloadTime().getTime());
            return lastLeg.UnloadTime();
        }

        /// <summary>
        /// The last leg on the itinerary.
        /// </summary>
        /// <returns></returns>
        Leg LastLeg()
        {
            if (legs.IsEmpty())
            {
                return null;
            }
            return legs[legs.Count - 1];
        }


        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        public bool SameValueAs(Itinerary other)
        {
            return other != null && legs.Equals(other.legs);
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            var itinerary = (Itinerary)obj;

            return SameValueAs(itinerary);
        }

        public int GetHashCode()
        {
            //TODO: atrosin ensure that hashcode is returned correctly: java version legs.hashCode();
            return legs.GetHashCode();
        }

        Itinerary()
        {
            // Needed by Hibernate
        }

        // Auto-generated surrogate key
        private long id;
    }
}
