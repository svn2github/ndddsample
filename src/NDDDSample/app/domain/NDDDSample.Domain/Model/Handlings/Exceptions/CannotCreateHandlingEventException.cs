namespace NDDDSample.Domain.Model.Handlings.Exceptions
{
    using System;

    /// <summary>
    /// If a HandlingEvent can't be created from a given set of parameters.
    ///
    /// It is a checked exception because it's not a programming error, but rather a
    /// special case that the application is built to handle. It can occur during normal
    /// program execution.
    /// </summary>
    public class CannotCreateHandlingEventException : Exception
    {
        public CannotCreateHandlingEventException(Exception e)
            : base("Cannot create handling event", e) {}

        protected CannotCreateHandlingEventException() {}
    }
}