namespace NDDDSample.Domain.Model.Locations
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Reflection;

    #endregion

    /// <summary>
    /// Sample locations, for test purposes.
    /// </summary>
    public class SampleLocations
    {
        private static readonly IDictionary<UnLocode, Location> ALL = new Dictionary<UnLocode, Location>();

        #region Cities

        public static readonly Location CHICAGO = new Location(new UnLocode("USCHI"), "Chicago");
        public static readonly Location DALLAS = new Location(new UnLocode("USDAL"), "Dallas");
        public static readonly Location GOTHENBURG = new Location(new UnLocode("SEGOT"), "Göteborg");
        public static readonly Location HAMBURG = new Location(new UnLocode("DEHAM"), "Hamburg");
        public static readonly Location HANGZOU = new Location(new UnLocode("CNHGH"), "Hangzhou");
        public static readonly Location HELSINKI = new Location(new UnLocode("FIHEL"), "Helsinki");
        public static readonly Location HONGKONG = new Location(new UnLocode("CNHKG"), "Hongkong");
        public static readonly Location MELBOURNE = new Location(new UnLocode("AUMEL"), "Melbourne");
        public static readonly Location NEWYORK = new Location(new UnLocode("USNYC"), "New York");
        public static readonly Location ROTTERDAM = new Location(new UnLocode("NLRTM"), "Rotterdam");
        public static readonly Location SHANGHAI = new Location(new UnLocode("CNSHA"), "Shanghai");
        public static readonly Location STOCKHOLM = new Location(new UnLocode("SESTO"), "Stockholm");
        public static readonly Location TOKYO = new Location(new UnLocode("JNTKO"), "Tokyo");

        #endregion

        #region Static Constr

        static SampleLocations()
        {
            foreach (var fieldInfo in typeof (SampleLocations).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                try
                {
                    if (fieldInfo.FieldType == typeof (Location))
                    {
                        var location = (Location) fieldInfo.GetValue(null);
                        ALL.Add(location.UnLocode, location);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Can't initialize Sample Locations", e);
                }
            }
        }

        #endregion

        public static IList<Location> GetAll()
        {
            return new List<Location>(ALL.Values);
        }

        public static Location Lookup(UnLocode unLocode)
        {
            return ALL[unLocode];
        }
    }
}