namespace NDDDSample.Web.Controllers.CargoAdmin
{
    #region Usings

    using System.Collections.Generic;  
    using Interfaces.BookingRemoteService.Common.Dto;

    #endregion

    /// <summary>
    /// The class provide a strongly typed result for RegistrationFormViewModel action 
    /// of the Cargo Admin Controller. The class incapsulates two classes and
    /// is used to provide a strongly typed result for the Registration Form View.
    /// </summary>
    public class RegistrationFormViewModel
    {
        private readonly IList<LocationDTO> locationDtos;
        private readonly IList<string> unLoccodes;

        public RegistrationFormViewModel(IList<LocationDTO> locationDtos, IList<string> unLoccodes)
        {
            this.locationDtos = locationDtos;
            this.unLoccodes = unLoccodes;            
        }

        public IList<LocationDTO> LocationDtos
        {
            get { return locationDtos; }
        }

        public IList<string> UnLoccodes
        {
            get { return unLoccodes; }
        }
    }
}