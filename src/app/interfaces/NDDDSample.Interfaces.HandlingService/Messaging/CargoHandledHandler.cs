namespace NDDDSample.Interfaces.HandlingService.Messaging
{
    #region Usings

    using System;
    using Application;
    using Domain.Model.Cargos;
    using Infrastructure.Log;
    using Infrastructure.Messaging;

    #endregion

    /// <summary>
    ///  Consumes messages and delegates notification of misdirected
    /// cargo to the tracking service.
    ///
    /// This is a programmatic hook into the  infrastructure to
    /// make cargo inspection message-driven.
    /// </summary>
    public class CargoHandledHandler : IMessageHandler<CargoHandledMessage>
    {
        private readonly ICargoInspectionService cargoInspectionService;
        private readonly ILog logger = LogFactory.GetExternalServiceLogger();

        public CargoHandledHandler(ICargoInspectionService cargoInspectionService)
        {
            this.cargoInspectionService = cargoInspectionService;
        }

        public void Handle(CargoHandledMessage message)
        {
            try
            {
                cargoInspectionService.InspectCargo(new TrackingId(message.TrackingId));
            }
            catch (Exception e)
            {
                logger.Error(e, e);
            }
        }
    }
}