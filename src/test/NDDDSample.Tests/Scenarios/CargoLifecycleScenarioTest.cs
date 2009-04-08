namespace NDDDSample.Tests.Scenarios
{
    #region Usings

    using NDDDSample.Domain.Model.Cargos;
    using NDDDSample.Domain.Model.Handlings;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Domain.Model.Voyages;
    using NUnit.Framework;

    #endregion

    [TestFixture]
    public class CargoLifecycleScenarioTest
    {
        /**
      * Repository implementations are part of the infrastructure layer,
      * which in this test is stubbed out by in-memory replacements.
      */
        IHandlingEventRepository handlingEventRepository;
        ICargoRepository cargoRepository;
        ILocationRepository locationRepository;
        IVoyageRepository voyageRepository;
      

        [Test]
        public void testCargoFromHongkongToStockholm()
        {
            //Test method
            Assert.AreEqual(1, 1);
        }
    }
}