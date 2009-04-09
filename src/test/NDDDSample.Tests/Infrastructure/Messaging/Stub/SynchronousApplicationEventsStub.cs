namespace NDDDSample.Tests.Infrastructure.Messaging.Stub
{
    #region Usings

    using Application;
    using Interfaces.Handlings;
    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;

    #endregion

    public class SynchronousApplicationEventsStub : IApplicationEvents
    {
        private ICargoInspectionService cargoInspectionService;

        public void setCargoInspectionService(ICargoInspectionService cargoInspectionSrv)
        {
            this.cargoInspectionService = cargoInspectionSrv;
        }


        public void cargoWasHandled(HandlingEvent evnt)
        {
            System.Console.WriteLine("EVENT: cargo was handled: " + evnt);
            cargoInspectionService.InspectCargo(evnt.Cargo.TrackingId);
        }


        public void cargoWasMisdirected(Cargo cargo)
        {
            System.Console.WriteLine("EVENT: cargo was misdirected");
        }


        public void cargoHasArrived(Cargo cargo)
        {
            System.Console.WriteLine("EVENT: cargo has arrived: " + cargo.TrackingId.IdString);
        }


        public void receivedHandlingEventRegistrationAttempt(HandlingEventRegistrationAttempt attempt)
        {
            System.Console.WriteLine("EVENT: received handling event registration attempt");
        }
    }
}