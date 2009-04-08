namespace NDDDSample.Application.Impl
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Domain.Model.Cargos;
    using Domain.Model.Locations;

    #endregion

    //TODO: port lately
    public class BookingService : IBookingService
    {
        public TrackingId bookNewCargo(UnLocode origin, UnLocode destination, DateTime arrivalDeadline)
        {
            throw new NotImplementedException();
        }

        public IList<Itinerary> requestPossibleRoutesForCargo(TrackingId trackingId)
        {
            throw new NotImplementedException();
        }

        public void assignCargoToRoute(Itinerary itinerary, TrackingId trackingId)
        {
            throw new NotImplementedException();
        }

        public void changeDestination(TrackingId trackingId, UnLocode unLocode)
        {
            throw new NotImplementedException();
        }
    }
}