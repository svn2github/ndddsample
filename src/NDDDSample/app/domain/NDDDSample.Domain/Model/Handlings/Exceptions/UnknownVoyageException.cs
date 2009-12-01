namespace NDDDSample.Domain.Model.Handlings.Exceptions
{
    using Voyages;

    /// <summary>
    /// Thrown when trying to register an event with an unknown carrier movement id.
    /// </summary>
    public class UnknownVoyageException : CannotCreateHandlingEventException
    {
        private readonly VoyageNumber voyageNumber;

        public UnknownVoyageException(VoyageNumber voyageNumber)
        {
            this.voyageNumber = voyageNumber;
        }

        public override string Message
        {
            get { return "No voyage with number " + voyageNumber.IdString + " exists in the system"; }
        }
    }
}