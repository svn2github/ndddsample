namespace NDDDSample.Interfaces.HandlingService
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using WebService;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;
    using Domain.Shared;

    #endregion

    /// <summary>
    /// Utility methods for parsing various forms of handling report formats.
    /// Supports the notification pattern for incremental error reporting.
    /// </summary>
    public static class HandlingReportParser
    {
        public const string ISO_8601_FORMAT = "yyyy-MM-dd HH:mm";

        public static UnLocode ParseUnLocode(string unlocode, IList<string> errors)
        {
            try
            {
                return new UnLocode(unlocode);
            }
            catch (ArgumentException e)
            {
                errors.Add(e.Message);
                return null;
            }
        }

        public static TrackingId ParseTrackingId(string trackingId, IList<string> errors)
        {
            try
            {
                return new TrackingId(trackingId);
            }
            catch (ArgumentException e)
            {
                errors.Add(e.Message);
                return null;
            }
        }

        public static VoyageNumber ParseVoyageNumber(string voyageNumber, IList<string> errors)
        {
            if (!string.IsNullOrEmpty(voyageNumber))
            {
                try
                {
                    return new VoyageNumber(voyageNumber);
                }
                catch (ArgumentException e)
                {
                    errors.Add(e.Message);
                    return null;
                }
            }
            return null;
        }

        public static DateTime ParseDate(string completionTime, IList<String> errors)
        {
            DateTime date;
            try
            {
                date = DateTime.ParseExact(completionTime, ISO_8601_FORMAT, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                errors.Add("Invalid date format: " + completionTime + ", must be on ISO 8601 format: " + ISO_8601_FORMAT);
                throw;
            }
            return date;
        }

        public static HandlingType ParseEventType(string eventType, IList<string> errors)
        {
            try
            {
                return Enumeration.FromValue<HandlingType>(eventType);
            }
            catch (ApplicationException)
            {
                errors.Add(eventType + " is not a valid handling event type. Valid types are: "
                           + Enumeration.GetAll<HandlingType>());
                return null;
            }
        }

        public static DateTime? ParseCompletionTime(HandlingReport handlingReport, IList<String> errors)
        {
            var completionTime = handlingReport.CompletionTime;
            if (completionTime == null)
            {
                errors.Add("Completion time is required");
                return null;
            }

            return completionTime.Value.ToUniversalTime();
        }
    }
}