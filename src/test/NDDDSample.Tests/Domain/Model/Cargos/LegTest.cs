namespace NDDDSample.Tests.Domain.Model.Cargos
{
    using System;
    using NDDDSample.Domain.Model.Cargos;
    using NUnit.Framework;

    [TestFixture]
    public class LegTest
    {

        [ExpectedException(typeof(ArgumentNullException), UserMessage = "Should't accept null constructor arguments")]
        [Test]
        public void testConstructor()
        {
            new Leg(null, null, null, DateTime.Now, DateTime.Now.AddDays(2));
        }
    }
}
