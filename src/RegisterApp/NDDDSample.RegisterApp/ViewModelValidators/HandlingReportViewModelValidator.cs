namespace NDDDSample.RegisterApp.ViewModelValidators
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    using ViewModels;

    #endregion

    /// <summary>
    /// The register app window validator.
    /// </summary>
    public class HandlingReportViewModelValidator
    {
        #region Constants and Fields

        /// <summary>
        /// The is o_8601_ format.
        /// </summary>
        public const string ISO_8601_FORMAT = "yyyy-MM-dd HH:mm";
        
        /// <summary>
        /// The handlingReportViewModel.
        /// </summary>
        private readonly HandlingReportViewModel handlingReportViewModel;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlingReportViewModelValidator"/> class.
        /// </summary>
        /// <param name="handlingReportViewModel">
        /// The handling report view model.
        /// </param>
        public HandlingReportViewModelValidator(HandlingReportViewModel handlingReportViewModel)
        {
            this.handlingReportViewModel = handlingReportViewModel;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The parse date.
        /// </summary>
        /// <param name="completionTime">
        /// The completion time.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        /// <returns>
        /// The datetime
        /// </returns>
        public static DateTime ParseDate(string completionTime, IList<string> errors)
        {
            DateTime date;
            try
            {
                date = DateTime.ParseExact(completionTime, ISO_8601_FORMAT, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                errors.Add(
                    "Invalid date format: " + completionTime + ", must be on ISO 8601 format: " + ISO_8601_FORMAT);
                throw;
            }

            return date;
        }

        /// <summary>
        /// The validate.
        /// </summary>
        public void Validate()
        {
            var localValidationErrors = new ObservableCollection<ValidationFailure>();

            if (String.IsNullOrEmpty(this.handlingReportViewModel.CompletionTime))
            {
                localValidationErrors.Add(new ValidationFailure("CompletionTime", "Completion Time has to be set!"));
            }
            else
            {
                try
                {
                    DateTime.ParseExact(
                        this.handlingReportViewModel.CompletionTime, ISO_8601_FORMAT, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    localValidationErrors.Add(
                        new ValidationFailure(
                            "CompletionTime",
                            "Invalid date format: " + this.handlingReportViewModel.CompletionTime +
                            ", must be on ISO 8601 format: " + ISO_8601_FORMAT));
                }
            }

            if (String.IsNullOrEmpty(this.handlingReportViewModel.Voyage))
            {
                localValidationErrors.Add(new ValidationFailure("Voyage", "Voyage has to be set!"));
            }

            if (String.IsNullOrEmpty(this.handlingReportViewModel.Location))
            {
                localValidationErrors.Add(new ValidationFailure("Location", "Location has to be set!"));
            }

            if (String.IsNullOrEmpty(this.handlingReportViewModel.TrackingId))
            {
                localValidationErrors.Add(new ValidationFailure("TrackingId", "TrackingId has to be set!"));
            }

            if (this.handlingReportViewModel.SelectedHandlingType.Equals(HandlingReportViewModel.HandlingType.None))
            {
                localValidationErrors.Add(new ValidationFailure("SelectedHandlingType", "Handling  has to be set!"));
            }

            this.handlingReportViewModel.ValidationErrors = localValidationErrors;
        }

        #endregion
    }
}