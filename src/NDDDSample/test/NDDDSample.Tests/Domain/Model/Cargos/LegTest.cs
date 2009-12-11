namespace NDDDSample.Tests.Domain.Model.Cargos
{
    #region Usings

    using System;
    using NDDDSample.Domain.Model.Cargos;
    using NUnit.Framework;

    #endregion

    [TestFixture, Category(UnitTestCategories.DomainModel)]
    public class LegTest
    {
        [ExpectedException(typeof (ArgumentNullException), UserMessage = "Should't accept null constructor arguments"),
         Test]
        
        public void TestConstructor()
        {
            new Leg(null, null, null, DateTime.Now, DateTime.Now.AddDays(2));
        }
    }
}