namespace NDDDSample.RegisterApp.Commands
{
    #region Usings

    using System;
    using System.Windows.Input;

    #endregion

    /// <summary>
    /// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute"/> and <see cref="CanExecute"/>.    
    /// </summary>
    /// <typeparam name="T">
    /// Parameter type.
    /// </typeparam>
    public class DelegateCommand<T> : ICommand
    {
        #region Constants and Fields

        /// <summary>
        /// The can execute method.
        /// </summary>
        private readonly Func<T, bool> canExecuteMethod;

        /// <summary>
        /// The execute method.
        /// </summary>
        private readonly Action<T> executeMethod;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class. 
        /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="executeMethod">
        /// Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.
        /// </param>
        /// <remarks>
        /// <seealso cref="CanExecute"/>
        /// will always return true.
        /// </remarks>
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class. 
        /// Initializes a new instance of <see cref="DelegateCommand{T}"/>.
        /// </summary>
        /// <param name="executeMethod">
        /// Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.
        /// </param>
        /// <param name="canExecuteMethod">
        /// Delegate to execute when CanExecute is called on the command.  This can be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// When both <paramref name="executeMethod"/> and <paramref name="canExecuteMethod"/> ar <see langword="null"/>.
        /// </exception>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            if (executeMethod == null && canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }

            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute. You must keep a hard
        /// reference to the handler to avoid garbage collection and unexpected results. See remarks for more information.
        /// </summary>
        /// <remarks>
        /// When subscribing to the <see cref="ICommand.CanExecuteChanged"/> event using 
        /// code (not when binding using XAML) will need to keep a hard reference to the event handler. This is to prevent 
        /// garbage collection of the event handler because the command implements the Weak Event pattern so it does not have
        /// a hard reference to this handler. An example implementation can be seen in the CompositeCommand and CommandBehaviorBase
        /// classes. In most scenarios, there is no reason to sign up to the CanExecuteChanged event directly, but if you do, you
        /// are responsible for maintaining the reference.
        /// </remarks>
        /// <example>
        /// The following code holds a reference to the event handler. The myEventHandlerReference value should be stored
        /// in an instance member to avoid it from being garbage collected.
        /// <code>
        /// EventHandler myEventHandlerReference = new EventHandler(this.OnCanExecuteChanged);
        /// command.CanExecuteChanged += myEventHandlerReference;
        /// </code>
        /// </example>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this command can be executed; otherwise, <see langword="false"/>.
        /// </returns>
        public bool CanExecute(T parameter)
        {
            if (this.canExecuteMethod == null)
            {
                return true;
            }

            return this.canExecuteMethod(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null"/>.
        /// </param>
        public void Execute(T parameter)
        {
            if (this.executeMethod == null)
            {
                return;
            }

            this.executeMethod(parameter);
        }

        #endregion

        #region Implemented Interfaces

        #region ICommand

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        /// </param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        /// </param>
        void ICommand.Execute(object parameter)
        {
            this.Execute((T)parameter);
        }

        #endregion

        #endregion
    }
}