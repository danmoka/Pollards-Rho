using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollardsRho;

namespace Tests
{
    [TestClass]
    public class PollardMethodTests
    {
        private readonly PollardMethod _pollardMethod;

        public PollardMethodTests()
        {
            _pollardMethod = new PollardMethod();
        }

        [TestMethod]
        public void GetLogarithm_ReturnsCorrectValue_1()
        {
            var p = new Point(80, 87);
            var q = new Point(3, 6);
            var curve = new EllipticCurve(2, 3, 97, 5, p);

            var expected = 3;
            var actual = _pollardMethod.GetLogarithm(p, q, curve);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetLogarithm_ReturnsCorrectValue_2()
        {
            var p = new Point(24, 53);
            var q = new Point(31, 6);
            var curve = new EllipticCurve(3, 9, 73, 83, p);

            var expected = 53;
            var actual = _pollardMethod.GetLogarithm(p, q, curve);

            Assert.AreEqual(expected, actual);
        }
    }
}
