namespace NDDDSample.RegisterApp.Views
{
    /// <summary>
    /// The i message box creator.
    /// </summary>
    public interface IMessageBoxCreator
    {
        #region Public Methods

        /// <summary>
        /// The show message box.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        void ShowMessageBox(string title, string message);

        #endregion
    }
}