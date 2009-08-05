namespace NDDDSample.Interfaces.BookingRemoteService.Common.Dto
{
    #region Usings

    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    /// Location DTO.
    /// </summary>   
    [DataContract]
    public class LocationDTO
    {
        [DataMember] private string name;
        [DataMember] private string unLocode;

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