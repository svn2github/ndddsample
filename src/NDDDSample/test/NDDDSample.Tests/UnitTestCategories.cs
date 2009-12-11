namespace NDDDSample.Tests
{
    /// <summary>
    /// The class has clasifications for Unit Tests.
    /// The clasification allows to run only selected logical categories,
    /// (slower categories could be skipped in some cases, such as integration)
    /// </summary>
    public static class UnitTestCategories
    {
        public const string Infrastructure = "Infrastructure Layer";
        public const string Integration = "Integration";
        public const string Controllers = "Controller Layer";
        public const string Scenarios = "Scenarios";
        public const string DomainModel = "Domain Model Layer";
    }
    
}
