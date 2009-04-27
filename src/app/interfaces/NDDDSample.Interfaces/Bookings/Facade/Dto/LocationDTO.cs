namespace NDDDSample.Interfaces.Bookings.Facade.Dto
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Location DTO.
    /// </summary>
    [Serializable]
    public class LocationDTO
    {
        private readonly string name;
        private readonly string unLocode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="unLocode">UN Location Code</param>
        /// <param name="name">Location Name</param>
        public LocationDTO(string unLocode, string name)
        {
            this.unLocode = unLocode;
            this.name = name;
        }

        public string UnLocode
        {
            get { return unLocode; }
        }

        public string Name
        {
            get { return name; }
        }
    }
}