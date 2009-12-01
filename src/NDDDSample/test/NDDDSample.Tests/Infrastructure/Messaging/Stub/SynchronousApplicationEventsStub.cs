namespace NDDDSample.Tests.Infrastructure.Messaging.Stub
{
    #region Usings

    using Application;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;

    #endregion

    public class SynchronousApplicationEventsStub : IApplicationEvents
    {
        private ICargoInspectionService cargoInspectionService;

        #region IApplicationEvents Members

        public void CargoWasHandled(HandlingEvent evnt)
        {
            System.Console.WriteLine("EVENT: cargo was handled: " + evnt);
            cargoInspectionService.InspectCargo(evnt.Cargo.TrackingId);
        }


        public void CargoWasMisdirected(Cargo cargo)
        {
            System.Console.WriteLine("EVENT: cargo was misdirected");
        }


        public void CargoHasArrived(Cargo cargo)
        {
            System.Console.WriteLine("EVENT: cargo has arrived: " + cargo.TrackingId.IdString);
        }


        public void ReceivedHandlingEventRegistrationAttempt(HandlingEventRegistrationAttempt attempt)
        {
            System.Console.WriteLine("EVENT: received handling event registration attempt");
        }

        #endregion

        public void SetCargoInspectionService(ICargoInspectionService cargoInspectionSrv)
        {
            cargoInspectionService = cargoInspectionSrv;
        }
    }
}