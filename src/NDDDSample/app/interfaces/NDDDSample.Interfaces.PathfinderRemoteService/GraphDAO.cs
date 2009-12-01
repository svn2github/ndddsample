namespace NDDDSample.Interfaces.PathfinderRemoteService
{
    #region Usings

    using System;
    using System.Collections.Generic;

    #endregion

    public class GraphDAO
    {
        private static readonly Random random = new Random();

        public virtual IList<String> ListLocations()
        {
            return new List<string>
                       {
                           "CNHKG",
                           "AUMEL",
                           "SESTO",
                           "FIHEL",
                           "USCHI",
                           "JNTKO",
                           "DEHAM",
                           "CNSHA",
                           "NLRTM",
                           "SEGOT",
                           "CNHGH",
                           "USNYC",
                           "USDAL"
                       };
        }

        public virtual string GetVoyageNumber(string from, string to)
        {
            int i = random.Next(5);
            if (i == 0)
            {
                return "0100S";
            }
            if (i == 1)
            {
                return "0200T";
            }
            if (i == 2)
            {
                return "0300A";
            }
            if (i == 3)
            {
                return "0301S";
            }
            return "0400S";
        }
    }
}