namespace NDDDSample.Interfaces.HandlingService.WebService
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Text;

    using Application;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;
    using Infrastructure.Builders;
    using Infrastructure.Log;
    using Infrastructure.Utils;
    using System.Linq;

    #endregion

    /// <summary>
    /// This web service endpoint implementation performs basic validation and parsing
    /// of incoming data, and in case of a valid registration attempt, sends an asynchronous message
    /// with the informtion to the handling event registration system for proper registration.
    /// </summary>
    public class HandlingReportService : IHandlingReportService
    {
        private readonly IApplicationEvents applicationEvents;
        private static readonly ILog logger = LogFactory.GetInterfaceLayerLogger();

        public HandlingReportService(IApplicationEvents applicationEvents)
        {
            this.applicationEvents = applicationEvents;
        }

        public void SubmitReport(HandlingReport handlingReport)
        {
            IList<string> errors = new List<string>();

            DateTime? completionTime = HandlingReportParser.ParseCompletionTime(handlingReport, errors);
            VoyageNumber voyageNumber = HandlingReportParser.ParseVoyageNumber(handlingReport.VoyageNumber, errors);
            HandlingType type = HandlingReportParser.ParseEventType(handlingReport.Type, errors);
            UnLocode unLocode = HandlingReportParser.ParseUnLocode(handlingReport.UnLocode, errors);

            foreach (string trackingIdStr in handlingReport.TrackingIds)
            {
                TrackingId trackingId = HandlingReportParser.ParseTrackingId(trackingIdStr, errors);

                if (errors.IsEmpty())
                {
                    DateTime registrationTime = DateTime.Now;
                    var attempt = new HandlingEventRegistrationAttempt(
                        registrationTime, completionTime.Value, trackingId, voyageNumber, type, unLocode);
                    applicationEvents.ReceivedHandlingEventRegistrationAttempt(attempt);
                }
                else
                {
                    string errorString = String.Join("\r\n", errors.ToArray());
                    logger.Error("Parse error in handling report: " + errorString);

                    throw new FaultException<HandlingReportException>(new HandlingReportException(errorString), new FaultReason(errorString));
                }
            }
        }
    }
}