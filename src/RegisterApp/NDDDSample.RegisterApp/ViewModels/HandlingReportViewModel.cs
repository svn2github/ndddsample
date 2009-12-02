namespace NDDDSample.RegisterApp.ViewModels
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.ServiceModel;
    using Commands;
    using HandlingReportService;
    using ViewModelValidators;

    #endregion

    /// <summary>
    /// The handling report view model.
    /// </summary>
    public class HandlingReportViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Constants and Fields

        /// <summary>
        /// The handling report service.
        /// </summary>
        private readonly IHandlingReportService handlingReportServiceClient;

        /// <summary>
        /// The register app window validator.
        /// </summary>
        private readonly HandlingReportViewModelValidator handlingReportViewModelValidator;

        /// <summary>
        /// The location.
        /// </summary>
        private string location;

        /// <summary>
        /// The selected handling type.
        /// </summary>
        private HandlingType selectedHandlingType = HandlingType.None;

        /// <summary>
        /// The tracking id.
        /// </summary>
        private string trackingId;

        /// <summary>
        /// The validation errors.
        /// </summary>
        private IList<ValidationFailure> validationErrors = new List<ValidationFailure>();

        /// <summary>
        /// The voyage.
        /// </summary>
        private string voyage;

        /// <summary>
        /// The Completion Time.
        /// </summary>
        private string completionTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlingReportViewModel"/> class.
        /// </summary>
        /// <param name="handlingReportServiceClient">
        /// The handling Report Service Client.
        /// </param>
        public HandlingReportViewModel(IHandlingReportService handlingReportServiceClient)
        {
            this.handlingReportServiceClient = handlingReportServiceClient;

            // initialise validator for this view model
            handlingReportViewModelValidator = new HandlingReportViewModelValidator(this);            

            HandlingReportCommands.RegisterHandlingReport = RelayCommand.RegisterCommand(
                param => CanRegister(), param => Register());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Error.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets HandlingTypeList.
        /// </summary>
        public IDictionary HandlingTypeList
        {
            get
            {
                IDictionary handlingTypeList = new Dictionary<HandlingType, string>
                                                   {
                                                       {HandlingType.None, HandlingType.None.ToString()},
                                                       {HandlingType.Unload, HandlingType.Unload.ToString()},
                                                       {HandlingType.Receive, HandlingType.Receive.ToString()},
                                                       {HandlingType.Load, HandlingType.Load.ToString()},
                                                       {HandlingType.Customs, HandlingType.Customs.ToString()},
                                                       {HandlingType.Claim, HandlingType.Claim.ToString()}
                                                   };
                return handlingTypeList;
            }
        }

        /// <summary>
        /// Gets or sets Location.
        /// </summary>
        public string Location
        {
            get { return location; }

            set { location = value.Trim(); }
        }

        /// <summary>
        /// Gets or sets SelectedHandlingType.
        /// </summary>
        public HandlingType SelectedHandlingType
        {
            get { return selectedHandlingType; }

            set { selectedHandlingType = value; }
        }

        /// <summary>
        /// Gets or sets TrackingId.
        /// </summary>
        public string TrackingId
        {
            get { return trackingId; }

            set { trackingId = value.Trim(); }
        }

        /// <summary>
        /// Gets or sets ValidationErrors.
        /// </summary>
        public IList<ValidationFailure> ValidationErrors
        {
            get { return validationErrors; }

            set { validationErrors = value; }
        }

        /// <summary>
        /// Gets or sets Voyage.
        /// </summary>
        public string Voyage
        {
            get { return voyage; }

            set { voyage = value.Trim(); }
        }

        public string CompletionTime
        {
            get { return completionTime; }
            set { completionTime = value; }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        public string this[string columnName]
        {
            get
            {
                handlingReportViewModelValidator.Validate();
                string errorText = string.Empty;
                if (validationErrors.Any(x => x.Key == columnName))
                {
                    errorText = validationErrors.Where(x => x.Key == columnName).First().Description;
                }

                return errorText;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The validate.
        /// </summary>
        public void Validate()
        {
            handlingReportViewModelValidator.Validate();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the handling type can be registered.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the handling type can be registered; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRegister()
        {
            return validationErrors.Count == 0;
        }

        /// <summary>
        /// Loads the data from database.
        /// </summary>
        public void Register()
        {
            const string ISO_8601_FORMAT = "yyyy-MM-dd HH:mm";
            var handlingReport = new HandlingReport
                                     {
                                         CompletionTime =
                                             DateTime.ParseExact(completionTime, ISO_8601_FORMAT,
                                                                 CultureInfo.InvariantCulture),
                                         TrackingIds = new[] {trackingId},
                                         VoyageNumber = voyage,
                                         UnLocode = location,
                                         Type = selectedHandlingType.ToString().ToUpper()
                                     };

            var error = string.Empty;
            try
            {
                handlingReportServiceClient.SubmitReport(handlingReport);
            }
            catch (FaultException<HandlingReportException> exception)
            {
                error = exception.Message;
            }
            catch (CommunicationException exception)
            {
                error = exception.Message;               
            }
        }

        #endregion
    }
}