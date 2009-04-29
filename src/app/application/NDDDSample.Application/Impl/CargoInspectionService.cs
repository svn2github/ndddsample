namespace NDDDSample.Application.Impl
{
    #region Usings

    using System.Transactions;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Infrastructure.Log;
    using Infrastructure.Validations;

    #endregion

    public class CargoInspectionService : ICargoInspectionService
    {
        private readonly IApplicationEvents applicationEvents;
        private readonly ICargoRepository cargoRepository;
        private readonly IHandlingEventRepository handlingEventRepository;
        private readonly ILog logger = LogFactory.GetApplicationLayerLogger();

        public CargoInspectionService(IApplicationEvents applicationEvents,
                                      ICargoRepository cargoRepository,
                                      IHandlingEventRepository handlingEventRepository)
        {
            this.applicationEvents = applicationEvents;
            this.cargoRepository = cargoRepository;
            this.handlingEventRepository = handlingEventRepository;
        }

        #region ICargoInspectionService Members

        public void InspectCargo(TrackingId trackingId)
        {
            Validate.NotNull(trackingId, "Tracking ID is required");
            Cargo cargo;
            using (var transactionScope = new TransactionScope())
            {
                cargo = cargoRepository.Find(trackingId);


                if (cargo == null)
                {
                    logger.Warn("Can't inspect non-existing cargo " + trackingId);
                    return;
                }

                HandlingHistory handlingHistory = handlingEventRepository.LookupHandlingHistoryOfCargo(trackingId);

                cargo.DeriveDeliveryProgress(handlingHistory);

                if (cargo.Delivery.IsMisdirected)
                {
                    applicationEvents.CargoWasMisdirected(cargo);
                }

                if (cargo.Delivery.IsUnloadedAtDestination)
                {
                    applicationEvents.CargoHasArrived(cargo);
                }

                cargoRepository.Store(cargo);
                transactionScope.Complete();
            }
        }

        #endregion
    }
}