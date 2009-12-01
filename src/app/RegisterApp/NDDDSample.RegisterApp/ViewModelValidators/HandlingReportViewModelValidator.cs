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
        public const string ISO_8601_FORMAT = "yyyy-MM-dd HH:mm";

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

        #region Properties

        /// <summary>
        /// Gets or sets handlingReportViewModel.
        /// </summary>
        private HandlingReportViewModel handlingReportViewModel { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The validate.
        /// </summary>
        public void Validate()
        {
            var localValidationErrors = new ObservableCollection<ValidationFailure>();
           
            try
            {
                DateTime.ParseExact(this.handlingReportViewModel.CompletionTime, ISO_8601_FORMAT, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                localValidationErrors.Add(new ValidationFailure("CompletionTime", "Invalid date format: " + this.handlingReportViewModel.CompletionTime + ", must be on ISO 8601 format: " + ISO_8601_FORMAT));                
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

            if (this.handlingReportViewModel.SelectedHandlingType.Equals(HandlingType.None))
            {
                localValidationErrors.Add(new ValidationFailure("SelectedHandlingType", "Handling  has to be set!"));
            }

            this.handlingReportViewModel.ValidationErrors = localValidationErrors;
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
        #endregion
    }
}