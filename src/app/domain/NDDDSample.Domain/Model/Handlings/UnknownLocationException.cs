namespace NDDDSample.Domain.Model.Handlings
{
    #region Usings

    using Locations;

    #endregion

    public class UnknownLocationException : CannotCreateHandlingEventException
    {
        private readonly UnLocode unlocode;

        public UnknownLocationException(UnLocode unlocode)
        {
            this.unlocode = unlocode;
        }


        public override string Message
        {
            get { return "No location with UN locode " + unlocode.IdString() + " exists in the system"; }
        }
    }
}