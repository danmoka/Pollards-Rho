using Microsoft.VisualStudio.TestTools.UnitTesting;
using PollardsRho;

namespace Tests
{
    [TestClass]
    public class ECCTest
    {
        private readonly EllipticCurve _ecc = new EllipticCurve(3, 9, 73, 83, new Point(31, 6));

        [TestMethod]
        public void IOnCurveTest()
        {
            Assert.IsTrue(_ecc.IsOnCurve(new Point(24, 53)));
        }

        [TestMethod]
        public void INotOnCurveTest()
        {
            Assert.IsFalse(_ecc.IsOnCurve(new Point(0, 4)));
        }

        [TestMethod]
        public void AddDiffOnCurvePointsTest()
        {
            Assert.AreEqual(_ecc.Add(new Point(31, 6), new Point(24, 53)), new Point(2, 60));
        }

        [TestMethod]
        public void AddSameOnCurvePointsTest()
        {
            Assert.AreEqual(_ecc.Add(new Point(31, 6), new Point(31, 6)), new Point(17, 3));
        }

        [TestMethod]
        public void AddInfinitePointTest()
        {
            var point = new Point(31, 6);

            Assert.AreEqual(_ecc.Add(point, null), point);
            Assert.AreEqual(_ecc.Add(null, point), point);
        }

        [TestMethod]
        public void MultiplyOnCurvePointTest()
        {
            Assert.AreEqual(_ecc.Multiply(10, new Point(31, 6)), new Point(32, 60));
            Assert.AreEqual(_ecc.Multiply(0, new Point(31, 6)), null);
        }

        [TestMethod]
        public void NegPointTest()
        {
            Assert.AreEqual(_ecc.NegX_axisPoint(new Point(31, 6)), new Point(31, 67));
            Assert.AreEqual(_ecc.NegX_axisPoint(null), null);
        }
    }
}