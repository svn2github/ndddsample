namespace NDDDSample.Domain.Model.Voyages
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Locations;

    #endregion

    /// <summary>
    ///  Sample carrier movements, for test purposes.
    /// </summary>
    public class SampleVoyages
    {
        private static readonly IDictionary<VoyageNumber, Voyage> ALL = new Dictionary<VoyageNumber, Voyage>();

        #region Depricated

        // TODO CM00[1-6] and createVoyage are deprecated. Remove and refactor tests.
        public static readonly Voyage CM001 = CreateVoyage("CM001", SampleLocations.STOCKHOLM, SampleLocations.HAMBURG);
        public static readonly Voyage CM002 = CreateVoyage("CM002", SampleLocations.HAMBURG, SampleLocations.HONGKONG);
        public static readonly Voyage CM003 = CreateVoyage("CM003", SampleLocations.HONGKONG, SampleLocations.NEWYORK);
        public static readonly Voyage CM004 = CreateVoyage("CM004", SampleLocations.NEWYORK, SampleLocations.CHICAGO);
        public static readonly Voyage CM005 = CreateVoyage("CM005", SampleLocations.CHICAGO, SampleLocations.HAMBURG);
        public static readonly Voyage CM006 = CreateVoyage("CM006", SampleLocations.HAMBURG, SampleLocations.HANGZOU);

        #endregion

        #region Named Voyages

        /// <summary>
        /// Voyage number 0300A (by airplane)   
        /// Dallas - Hamburg - Stockholm - Helsinki   
        /// </summary>
        public static readonly Voyage DALLAS_TO_HELSINKI =
            new Voyage.Builder(new VoyageNumber("0300A"), SampleLocations.DALLAS).
                AddMovement(SampleLocations.HAMBURG, DateTime.Parse("2008-10-29 03:30"),
                            DateTime.Parse("2008-10-31 14:00")).
                AddMovement(SampleLocations.STOCKHOLM, DateTime.Parse("2008-11-01 15:20"),
                            DateTime.Parse("2008-11-01 18:40")).
                AddMovement(SampleLocations.HELSINKI, DateTime.Parse("2008-11-02 09:00"),
                            DateTime.Parse("2008-11-02 11:15")).
                Build();

        /// <summary>
        /// Voyage number 0301S (by ship)
        /// Dallas - Hamburg - Stockholm - Helsinki, alternate route
        /// </summary>
        public static readonly Voyage DALLAS_TO_HELSINKI_ALT =
            new Voyage.Builder(new VoyageNumber("0301S"), SampleLocations.DALLAS).
                AddMovement(SampleLocations.HELSINKI, DateTime.Parse("2008-10-29 03:30"),
                            DateTime.Parse("2008-11-05 15:45")).
                Build();

        /// <summary>
        ///  Voyage number 0400S (by ship)
        /// Helsinki - Rotterdam - Shanghai - Hongkong
        /// </summary>
        public static readonly Voyage HELSINKI_TO_HONGKONG =
            new Voyage.Builder(new VoyageNumber("0400S"), SampleLocations.HELSINKI).
                AddMovement(SampleLocations.ROTTERDAM, DateTime.Parse("2008-11-04 05:50"),
                            DateTime.Parse("2008-11-06 14:10")).
                AddMovement(SampleLocations.SHANGHAI, DateTime.Parse("2008-11-10 21:45"),
                            DateTime.Parse("2008-11-22 16:40")).
                AddMovement(SampleLocations.HONGKONG, DateTime.Parse("2008-11-24 07:00"),
                            DateTime.Parse("2008-11-28 13:37")).
                Build();

        /// <summary>
        /// Voyage number 0200T (by train)
        /// New York - Chicago - Dallas
        /// </summary>
        public static readonly Voyage NEW_YORK_TO_DALLAS =
            new Voyage.Builder(new VoyageNumber("0200T"), SampleLocations.NEWYORK).
                AddMovement(SampleLocations.CHICAGO, DateTime.Parse("2008-10-24 07:00"),
                            DateTime.Parse("2008-10-24 17:45")).
                AddMovement(SampleLocations.DALLAS, DateTime.Parse("2008-10-24 21:25"),
                            DateTime.Parse("2008-10-25 19:30")).
                Build();

        /// <summary>
        ///Voyage number 0100S (by ship)
        ///Hongkong - Hangzou - Tokyo - Melbourne - New York   
        ///</summary>      
        public static Voyage HONGKONG_TO_NEW_YORK =
            new Voyage.Builder(new VoyageNumber("0100S"), SampleLocations.HONGKONG).
                AddMovement(SampleLocations.HANGZOU, DateTime.Parse("2008-10-01 12:00"),
                            DateTime.Parse("2008-10-03 14:30")).
                AddMovement(SampleLocations.TOKYO, DateTime.Parse("2008-10-03 21:00"),
                            DateTime.Parse("2008-10-06 06:15")).
                AddMovement(SampleLocations.MELBOURNE, DateTime.Parse("2008-10-06 11:00"),
                            DateTime.Parse("2008-10-12 11:30")).
                AddMovement(SampleLocations.NEWYORK, DateTime.Parse("2008-10-14 12:00"),
                            DateTime.Parse("2008-10-23 23:10")).
                Build();

        #endregion

        #region v[1-4] Voyages

        public static readonly Voyage v100 = new Voyage.Builder(new VoyageNumber("V100"), SampleLocations.HONGKONG).
            AddMovement(SampleLocations.TOKYO, DateTime.Parse("2009-03-03"), DateTime.Parse("2009-03-05")).
            AddMovement(SampleLocations.NEWYORK, DateTime.Parse("2009-03-06"), DateTime.Parse("2009-03-09")).
            Build();

        public static readonly Voyage v200 = new Voyage.Builder(new VoyageNumber("V200"), SampleLocations.TOKYO).
            AddMovement(SampleLocations.NEWYORK, DateTime.Parse("2009-03-06"), DateTime.Parse("2009-03-08")).
            AddMovement(SampleLocations.CHICAGO, DateTime.Parse("2009-03-10"), DateTime.Parse("2009-03-14")).
            AddMovement(SampleLocations.STOCKHOLM, DateTime.Parse("2009-03-14"), DateTime.Parse("2009-03-16")).
            Build();

        public static readonly Voyage v300 = new Voyage.Builder(new VoyageNumber("V300"), SampleLocations.TOKYO).
            AddMovement(SampleLocations.ROTTERDAM, DateTime.Parse("2009-03-08"), DateTime.Parse("2009-03-11")).
            AddMovement(SampleLocations.HAMBURG, DateTime.Parse("2009-03-11"), DateTime.Parse("2009-03-12")).
            AddMovement(SampleLocations.MELBOURNE, DateTime.Parse("2009-03-14"), DateTime.Parse("2009-03-18")).
            AddMovement(SampleLocations.TOKYO, DateTime.Parse("2009-03-19"), DateTime.Parse("2009-03-21")).
            Build();

        public static readonly Voyage v400 = new Voyage.Builder(new VoyageNumber("V400"), SampleLocations.HAMBURG).
            AddMovement(SampleLocations.STOCKHOLM, DateTime.Parse("2009-03-14"), DateTime.Parse("2009-03-15")).
            AddMovement(SampleLocations.HELSINKI, DateTime.Parse("2009-03-15"), DateTime.Parse("2009-03-16")).
            AddMovement(SampleLocations.HAMBURG, DateTime.Parse("2009-03-20"), DateTime.Parse("2009-03-22")).
            Build();

        #endregion

        #region Static Methods

        private static Voyage CreateVoyage(string id, Location from, Location to)
        {
            var carrierMovements = new List<CarrierMovement>
                                       {new CarrierMovement(from, to, new DateTime(), new DateTime())};
            var schedule = new Schedule(carrierMovements);
            return new Voyage(new VoyageNumber(id), schedule);
        }


        public static IList<Voyage> GetAll()
        {
            return new List<Voyage>(ALL.Values);
        }

        public static Voyage Lookup(VoyageNumber voyageNumber)
        {
            if (!ALL.ContainsKey(voyageNumber))
            {
                return null;
            }

            return ALL[voyageNumber];
        }

        #endregion

        #region Static constr

        static SampleVoyages()
        {
            foreach (var fieldInfo in typeof (SampleVoyages).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                try
                {
                    if (fieldInfo.FieldType == typeof (Voyage))
                    {
                        var voyage = (Voyage) fieldInfo.GetValue(null);
                        ALL.Add(voyage.VoyageNumber, voyage);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Can't initialize Sample Locations", e);
                }
            }
        }

        #endregion
    }
}