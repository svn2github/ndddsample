namespace NDDDSample.RegisterApp.ViewModels
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// The view model base.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
            ThrowOnInvalidPropertyName = true;
        }

        #endregion

        #region Events

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether ThrowOnInvalidPropertyName.
        /// </summary>
        protected bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The verify property name.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        [Conditional("DEBUG"), DebuggerStepThrough]        
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (ThrowOnInvalidPropertyName)
                {
                    throw new Exception(msg);
                }

                Debug.Fail(msg);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        public virtual void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }
}