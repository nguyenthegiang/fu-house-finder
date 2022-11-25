using NUnit.Framework;
using System.Transactions;


namespace HouseFinder.Test
{
    /**
     * [Test - CampusController]
     */
    [TestFixture]
    public class CampusControllerTest
    {
        private TransactionScope scope;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}