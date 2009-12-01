namespace NDDDSample.Web.Controllers.CargoAdmin
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// The command is object that is filled by asp.net mvc 
    /// framework autmaticaly from view's hidden fields in order to
    /// assign an Itinerary for misrouted cargoes.
    /// </summary>
    public class LegCommand
    {
        public string VoyageNumber { get; set; }

        public string FromUnLocode { get; set; }

        public string ToUnLocode { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}