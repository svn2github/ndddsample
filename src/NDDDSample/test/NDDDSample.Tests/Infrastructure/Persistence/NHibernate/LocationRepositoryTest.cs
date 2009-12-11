namespace NDDDSample.Tests.Infrastructure.Persistence.NHibernate
{
    #region Usings

    using System.Collections.Generic;
    using NDDDSample.Domain.Model.Locations;
    using NDDDSample.Persistence.NHibernate;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.Infrastructure)]
    public class LocationRepositoryTest : BaseRepositoryTest
    {
        private ILocationRepository locationRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            locationRepository = new LocationRepositoryHibernate();
        }


        [Test]
        public void Find()
        {
            var melbourne = new UnLocode("AUMEL");
            Location location = locationRepository.Find(melbourne);
            Assert.IsNotNull(location);
            Assert.AreEqual(melbourne, location.UnLocode);

            Assert.IsNull(locationRepository.Find(new UnLocode("NOLOC")));
        }

        [Test]
        public void FindAll()
        {
            IList<Location> allLocations = locationRepository.FindAll();

            Assert.IsNotNull(allLocations);
            Assert.AreEqual(7, allLocations.Count);
        }
    }
}