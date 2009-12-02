namespace NDDDSample.RegisterApp.Commands
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.Windows.Input;

    #endregion

    /// <summary>
    /// The relay command.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Constants and Fields

        /// <summary>
        /// The _can execute.
        /// </summary>
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// The _execute.
        /// </summary>
        private readonly Action<object> execute;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">
        /// The execute.
        /// </param>
        public RelayCommand(Action<object> execute)
            : this(null, execute) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="canExecute">
        /// The can execute.
        /// </param>
        /// <param name="execute">
        /// The execute.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region Events

        /// <summary>
        /// The can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }

            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Implemented Interfaces

        #region ICommand

        /// <summary>
        /// The can execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The can execute.
        /// </returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }

        #endregion

        public static RelayCommand RegisterCommand(Predicate<object> canExecute, Action<object> execute)
        {
            return new RelayCommand(canExecute, execute);
        }

        #endregion
    }
}