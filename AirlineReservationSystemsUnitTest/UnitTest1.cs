using NUnit.Framework;
using Bunit;
using AirlineReservationSystems;
using AirlineReservationSystems.Pages;

namespace AirlineReservationSystemsUnitTest
{
    public class Tests
    {
        private Test _primeService;

        [SetUp]
        public void SetUp()
        {
            _primeService = new Test();
        }

        [TestCase(1)]
        public void Test1(int value)
        {
            var result = _primeService.Test1(value);

            Assert.IsFalse(result, $"{value} should not be prime");
        }
        [TestCase(23)]
        public void Test2(int value)
        {
            var result = _primeService.Test2(value);

            Assert.IsTrue(result, $"{value} should not be prime");
        }
    }
}