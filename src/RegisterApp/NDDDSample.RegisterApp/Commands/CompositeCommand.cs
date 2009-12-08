namespace NDDDSample.RegisterApp.Commands
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    #endregion

    /// <summary>
    /// The composite command.
    /// </summary>
    public class CompositeCommand : ICommand
    {
        #region Constants and Fields

        /// <summary>
        /// The registered commands.
        /// </summary>
        private readonly List<ICommand> registeredCommands = new List<ICommand>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of all the registered commands.
        /// </summary>
        /// <value>A list of registered commands.</value>
        /// <remarks>This returns a copy of the commands subscribed to the CompositeCommand.</remarks>
        public IList<ICommand> RegisteredCommands
        {
            get
            {
                IList<ICommand> commandList;
                lock (this.registeredCommands)
                {
                    commandList = this.registeredCommands.ToList();
                }

                return commandList;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Forwards <see cref="ICommand.CanExecute"/> to the registered commands and returns
        /// <see langword="true"/> if all of the commands return <see langword="true"/>.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.
        /// If the command does not require data to be passed, this object can be set to <see langword="null"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if all of the commands return <see langword="true"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool CanExecute(object parameter)
        {
            bool hasEnabledCommandsThatShouldBeExecuted = false;

            ICommand[] commandList;
            lock (this.registeredCommands)
            {
                commandList = this.registeredCommands.ToArray();
            }

            foreach (ICommand command in commandList)
            {
                if (!command.CanExecute(parameter))
                {
                    return false;
                }

                hasEnabledCommandsThatShouldBeExecuted = true;
            }

            return hasEnabledCommandsThatShouldBeExecuted;
        }

        /// <summary>
        /// The can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }

            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Forwards <see cref="ICommand.Execute"/> to the registered commands.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.
        /// If the command does not require data to be passed, this object can be set to <see langword="null"/>.
        /// </param>
        public virtual void Execute(object parameter)
        {
            Queue<ICommand> commands;
            lock (this.registeredCommands)
            {
                commands = new Queue<ICommand>(this.registeredCommands.ToList());
            }

            while (commands.Count > 0)
            {
                ICommand command = commands.Dequeue();
                command.Execute(parameter);
            }
        }

        /// <summary>
        /// Adds a command to the collection and signs up for the <see cref="ICommand.CanExecuteChanged"/> event of it.
        /// </summary>        
        /// <param name="command">
        /// The command to register.
        /// </param>
        public virtual void RegisterCommand(ICommand command)
        {
            lock (this.registeredCommands)
            {
                this.registeredCommands.Add(command);
            }
        }

        /// <summary>
        /// Removes a command from the collection and removes itself from the <see cref="ICommand.CanExecuteChanged"/> event of it.
        /// </summary>
        /// <param name="command">
        /// The command to unregister.
        /// </param>
        public virtual void UnregisterCommand(ICommand command)
        {            
            lock (this.registeredCommands)
            {
                this.registeredCommands.Remove(command);
            }
        }

        #endregion
    }
}