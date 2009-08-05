namespace NDDDSample.Web.Views.CargoAdmin
{
    #region Usings

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Interfaces.BookingRemoteService.Common.Dto;

    #endregion

    public partial class List : ViewPage<IList<CargoRoutingDTO>> {}
}