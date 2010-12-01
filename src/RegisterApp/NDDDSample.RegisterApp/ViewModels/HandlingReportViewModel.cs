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

    using Log;

    using ViewModelValidators;

    using Views;

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
        /// The message box creator.
        /// </summary>
        private readonly IMessageBoxCreator messageBoxCreator;

        /// <summary>
        /// The Completion Time.
        /// </summary>
        private string completionTime;

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

        private static readonly ILog logger = LogFactory.GetRegisterAppLogger();

        /// <summary>
        /// Gets RegisterReportCommand.
        /// </summary>
        public DelegateCommand<object> RegisterReportCommand { get; private set; }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlingReportViewModel"/> class.
        /// </summary>
        /// <param name="handlingReportServiceClient">
        /// The handling Report Service Client.
        /// </param>
        /// <param name="messageBoxCreator">
        /// The message Box Creator.
        /// </param>
        public HandlingReportViewModel(
            IHandlingReportService handlingReportServiceClient, IMessageBoxCreator messageBoxCreator)
        {
            this.handlingReportServiceClient = handlingReportServiceClient;
            this.messageBoxCreator = messageBoxCreator;

            // initialise validator for this view model
            this.handlingReportViewModelValidator = new HandlingReportViewModelValidator(this);

            this.RegisterReportCommand = new DelegateCommand<object>(this.Register, this.CanRegister);
            HandlingReportCommands.RegisterHandlingReportCommand.RegisterCommand(this.RegisterReportCommand);                
        }

        #endregion

        #region Enums

        /// <summary>
        /// The handling type.
        /// </summary>
        public enum HandlingType
        {
            /// <summary>
            /// The none.
            /// </summary>
            None, 

            /// <summary>
            /// The customs.
            /// </summary>
            Customs, 

            /// <summary>
            /// The receive.
            /// </summary>
            Receive, 

            /// <summary>
            /// The load.
            /// </summary>
            Load, 

            /// <summary>
            /// The unload.
            /// </summary>
            Unload, 

            /// <summary>
            /// The claim.
            /// </summary>
            Claim
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets CompletionTime.
        /// </summary>
        public string CompletionTime
        {
            get
            {
                return this.completionTime;
            }

            set
            {
                this.completionTime = value;
            }
        }

        /// <summary>
        /// Gets Error.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
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
                        { HandlingType.None, HandlingType.None.ToString() }, 
                        { HandlingType.Unload, HandlingType.Unload.ToString() }, 
                        { HandlingType.Receive, HandlingType.Receive.ToString() }, 
                        { HandlingType.Load, HandlingType.Load.ToString() }, 
                        { HandlingType.Customs, HandlingType.Customs.ToString() }, 
                        { HandlingType.Claim, HandlingType.Claim.ToString() }
                    };
                return handlingTypeList;
            }
        }

        /// <summary>
        /// Gets or sets Location.
        /// </summary>
        public string Location
        {
            get
            {
                return this.location;
            }

            set
            {
                this.location = value.Trim();
            }
        }

        /// <summary>
        /// Gets or sets SelectedHandlingType.
        /// </summary>
        public HandlingType SelectedHandlingType
        {
            get
            {
                return this.selectedHandlingType;
            }

            set
            {
                this.selectedHandlingType = value;
            }
        }

        /// <summary>
        /// Gets or sets TrackingId.
        /// </summary>
        public string TrackingId
        {
            get
            {
                return this.trackingId;
            }

            set
            {
                this.trackingId = value.Trim();
            }
        }

        /// <summary>
        /// Gets or sets ValidationErrors.
        /// </summary>
        public IList<ValidationFailure> ValidationErrors
        {
            get
            {
                return this.validationErrors;
            }

            set
            {
                this.validationErrors = value;
            }
        }

        /// <summary>
        /// Gets or sets Voyage.
        /// </summary>
        public string Voyage
        {
            get
            {
                return this.voyage;
            }

            set
            {
                this.voyage = value.Trim();
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// The tindexer.
        /// </summary>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        public string this[string columnName]
        {
            get
            {
                this.handlingReportViewModelValidator.Validate();
                string errorText = string.Empty;
                if (this.validationErrors.Any(x => x.Key == columnName))
                {
                    errorText = this.validationErrors.Where(x => x.Key == columnName).First().Description;
                }

                return errorText;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether the handling type can be registered.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the handling type can be registered; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRegister(object arg)
        {
            return this.validationErrors.Count == 0;
        }

        /// <summary>
        /// Loads the data from database.
        /// </summary>
        public void Register(object obj)
        {
            const string ISO_8601_FORMAT = "yyyy-MM-dd HH:mm";
            var handlingReport = new HandlingReport
                {
                    CompletionTime =
                        DateTime.ParseExact(this.completionTime, ISO_8601_FORMAT, CultureInfo.InvariantCulture), 
                    TrackingIds = new[] { this.trackingId }, 
                    VoyageNumber = this.voyage, 
                    UnLocode = this.location, 
                    Type = this.selectedHandlingType.ToString().ToUpper()
                };
            
            try
            {
                this.handlingReportServiceClient.SubmitReport(handlingReport);
                this.messageBoxCreator.ShowMessageBox("Success", "Event is succefully registered");
            }
            catch (FaultException<HandlingReportException> exception)
            {
                logger.Error(exception);
                this.messageBoxCreator.ShowMessageBox("HandlingReportException", exception.Message);
            }
            catch (CommunicationException exception)
            {
                logger.Error(exception);
                this.messageBoxCreator.ShowMessageBox("CommunicationException", exception.Message);
            }
        }

        /// <summary>
        /// The validate.
        /// </summary>
        public void Validate()
        {
            this.handlingReportViewModelValidator.Validate();
        }

        #endregion
    }
}