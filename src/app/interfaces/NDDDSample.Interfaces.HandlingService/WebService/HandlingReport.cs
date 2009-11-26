namespace NDDDSample.Interfaces.HandlingService.WebService
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    #endregion

    [DataContract(Name = "HandlingReport")]
    public class HandlingReport
    {
        private DateTime? completionTime;
        private IList<string> trackingIds;
        private string type;
        private string unLocode;
        private string voyageNumber;

        /// <summary>
        ///  Gets\Sets the value of the completionTime property.
        /// </summary>
        [DataMember(Order = 0, Name = "CompletionTime", IsRequired = true)]
        public DateTime? CompletionTime
        {
            get { return completionTime; }
            set { completionTime = value; }
        }

        /// <summary>
        /// Gets\Sets the value of the trackingIds property.
        /// </summary>
        [DataMember(Order = 1, Name = "TrackingIds", IsRequired = true)]
        public IList<string> TrackingIds
        {
            get { return trackingIds; }
            set { trackingIds = value; }
        }

        /// <summary>
        /// Gets\Sets the value of the Type property.
        /// </summary>
        [DataMember(Order = 2, Name = "Type", IsRequired = true)]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Sets\Sets the value of the UnLocode property.
        /// </summary>
        [DataMember(Order = 3, Name = "UnLocode", IsRequired = true)]
        public string UnLocode
        {
            get { return unLocode; }
            set { unLocode = value; }
        }

        /// <summary>
        /// Gets\Sets the value of the voyageNumber property.
        /// </summary>
        [DataMember(Order = 4, Name = "VoyageNumber")]
        public string VoyageNumber
        {
            get { return voyageNumber; }
            set { voyageNumber = value; }
        }
    }
}