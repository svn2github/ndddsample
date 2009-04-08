namespace NDDDSample.Application
{
    #region Usings

    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Interfaces.Handlings;

    #endregion

    /// <summary>
    /// /**
    ///This interface provides a way to let other parts
    /// of the system know about events that have occurred.
    /// <p/>
    /// It may be implemented synchronously or asynchronously, using
    /// for example JMS. 
    /// </summary>
    public interface IApplicationEvents
    {
        /// <summary>
        /// A cargo has been handled.
        /// </summary>
        /// <param name="evnt">handling event</param>
        void cargoWasHandled(HandlingEvent evnt);

        /// <summary>
        ///  A cargo has been misdirected.
        /// </summary>
        /// <param name="cargo">cargo</param>
        void cargoWasMisdirected(Cargo cargo);

        /// <summary>
        /// A cargo has arrived at its final destination.
        /// </summary>
        /// <param name="cargo">cargo</param>
        void cargoHasArrived(Cargo cargo);

        /// <summary>
        /// A handling event regitration attempt is received.
        /// </summary>
        /// <param name="attempt"> handling event registration attempt</param>
        void receivedHandlingEventRegistrationAttempt(HandlingEventRegistrationAttempt attempt);
    }
}