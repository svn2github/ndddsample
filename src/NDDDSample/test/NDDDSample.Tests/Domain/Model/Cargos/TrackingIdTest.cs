namespace NDDDSample.Tests.Domain.Model.Cargos
{
    #region Usings

    using System;
    using NDDDSample.Domain.Model.Cargos;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.DomainModel)]
    public class TrackingIdTest
    {
        [ExpectedException(typeof (ArgumentNullException), UserMessage = "Should't accept null constructor arguments")
        ]
        [Test]
        public void Constructor()
        {
            new TrackingId(null);
        }
    }
}