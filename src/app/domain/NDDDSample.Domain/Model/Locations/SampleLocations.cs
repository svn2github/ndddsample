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
        public static Location HONGKONG = new Location(new UnLocode("CNHKG"), "Hongkong");
        public static Location MELBOURNE = new Location(new UnLocode("AUMEL"), "Melbourne");
        public static Location STOCKHOLM = new Location(new UnLocode("SESTO"), "Stockholm");
        public static Location HELSINKI = new Location(new UnLocode("FIHEL"), "Helsinki");
        public static Location CHICAGO = new Location(new UnLocode("USCHI"), "Chicago");
        public static Location TOKYO = new Location(new UnLocode("JNTKO"), "Tokyo");
        public static Location HAMBURG = new Location(new UnLocode("DEHAM"), "Hamburg");
        public static Location SHANGHAI = new Location(new UnLocode("CNSHA"), "Shanghai");
        public static Location ROTTERDAM = new Location(new UnLocode("NLRTM"), "Rotterdam");
        public static Location GOTHENBURG = new Location(new UnLocode("SEGOT"), "Göteborg");
        public static Location HANGZOU = new Location(new UnLocode("CNHGH"), "Hangzhou");
        public static Location NEWYORK = new Location(new UnLocode("USNYC"), "New York");
        public static Location DALLAS = new Location(new UnLocode("USDAL"), "Dallas");

        public static IDictionary<UnLocode, Location> ALL = new Dictionary<UnLocode, Location>();

        static SampleLocations()
        {
            foreach (var fieldInfo in typeof (SampleLocations).GetFields(BindingFlags.Static))
            {
                try
                {
                    if (fieldInfo.FieldType == typeof (Location))
                    {
                        var location = (Location) fieldInfo.GetValue(null);
                        ALL.Add(location.UnLocode(), location);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Can't initialize Sample Locations", e);
                }
            }
        }

        public static List<Location> GetAll()
        {
            return new List<Location>(ALL.Values);
        }

        public static Location Lookup(UnLocode unLocode)
        {
            return ALL[unLocode];
        }
    }
}