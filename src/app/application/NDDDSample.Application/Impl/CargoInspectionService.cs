namespace NDDDSample.Application.Impl
{
    #region Usings

    using System.Transactions;
    using Domain.JavaRelated;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Infrastructure.Log;

    #endregion

    public class CargoInspectionService : ICargoInspectionService
    {
        private readonly IApplicationEvents applicationEvents;
        private readonly ICargoRepository cargoRepository;
        private readonly IHandlingEventRepository handlingEventRepository;
        private readonly ILog logger = LogFactory.GetApplicationLayer();

        public CargoInspectionService(IApplicationEvents applicationEvents,
                                          ICargoRepository cargoRepository,
                                          IHandlingEventRepository handlingEventRepository)
        {
            this.applicationEvents = applicationEvents;
            this.cargoRepository = cargoRepository;
            this.handlingEventRepository = handlingEventRepository;
        }


        public void InspectCargo(TrackingId trackingId)
        {
            Validate.NotNull(trackingId, "Tracking ID is required");
            Cargo cargo;
            using (var transactionScope = new TransactionScope())
            {
                cargo = cargoRepository.Find(trackingId);
                transactionScope.Complete();
            }
            if (cargo == null)
            {
                logger.Warn("Can't inspect non-existing cargo " + trackingId);
                return;
            }

            HandlingHistory handlingHistory = handlingEventRepository.LookupHandlingHistoryOfCargo(trackingId);

            cargo.DeriveDeliveryProgress(handlingHistory);

            if (cargo.Delivery.IsMisdirected)
            {
                applicationEvents.cargoWasMisdirected(cargo);
            }

            if (cargo.Delivery.IsUnloadedAtDestination)
            {
                applicationEvents.cargoHasArrived(cargo);
            }

            cargoRepository.Store(cargo);
        }
    }
}