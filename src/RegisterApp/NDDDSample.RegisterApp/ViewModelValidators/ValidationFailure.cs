namespace NDDDSample.RegisterApp.ViewModelValidators
{
    using System;

    /// <summary>
    /// A simple Validation error storage object, with a key
    /// and a Desription. 
    /// </summary>    
    public class ValidationFailure
    {
        #region Constants and Fields

        /// <summary>
        /// The description.
        /// </summary>
        private string description = String.Empty;

        /// <summary>
        /// The key.
        /// </summary>
        private string key = String.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationFailure"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public ValidationFailure(string key, string description)
        {
            this.Key = key;
            this.Description = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or set the Description
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
            }
        }

        /// <summary>
        /// Get or set the Key
        /// </summary>
        public string Key
        {
            get
            {
                return this.key;
            }

            set
            {
                this.key = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The to string.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0} {1}", this.Key, this.Description);
        }

        #endregion        
    }
}