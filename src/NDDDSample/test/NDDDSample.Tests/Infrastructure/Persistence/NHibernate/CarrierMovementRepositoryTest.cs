namespace NDDDSample.Tests.Infrastructure.Persistence.NHibernate
{
    #region Usings

    using NDDDSample.Domain.Model.Voyages;
    using NDDDSample.Persistence.NHibernate;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.Infrastructure)]
    public class CarrierMovementRepositoryTest : BaseRepositoryTest
    {
        private IVoyageRepository voyageRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            voyageRepository = new VoyageRepositoryHibernate();
        }

        [Test]
        public void Find()
        {
            Voyage voyage = voyageRepository.Find(new VoyageNumber("0101"));
            Assert.IsNotNull(voyage);
            Assert.AreEqual("0101", voyage.VoyageNumber.IdString);

            //TODO adapt
            /*Assert.AreEqual(STOCKHOLM, carrierMovement.DepartureLocation);
             Assert.AreEqual(HELSINKI, carrierMovement.ArrivalLocation);
             Assert.AreEqual(DateTestUtil.ToDate("2007-09-23", "02:00"), carrierMovement.DepartureTime());
             Assert.AreEqual(DateTestUtil.ToDate("2007-09-23", "03:00"), carrierMovement.ArrivalTime());*/
        }
    }
}