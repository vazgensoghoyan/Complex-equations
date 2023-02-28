using System;
using MathProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class TestComplex
    {
        [TestMethod]
        public void Argument()
        {
            var accuracy = 0.00001f;

            Assert.AreEqual(Math.PI / 2, new Complex(0, 11).Arg, accuracy);
            Assert.AreEqual(-Math.PI / 2, new Complex(0, -100).Arg, accuracy);

            Assert.AreEqual(Math.Atan(-1f / 117), new Complex(117, -1).Arg, accuracy);

            Assert.AreEqual(Math.PI + Math.Atan(-19f / 708), new Complex(-708, 19).Arg, accuracy);
            Assert.AreEqual(-Math.PI + Math.Atan(13f / 78), new Complex(-78, -13).Arg, accuracy);
        }

        [TestMethod]
        public void Addition()
        {
            var n1 = new Complex(1, 3);
            var n2 = new Complex(4, -5);

            Assert.AreEqual(new Complex(5, -2), n1 + n2);
        }

        [TestMethod]
        public void Subtraction()
        {
            var n1 = new Complex(-2, 1);
            var n2 = new Complex(Math.Sqrt(3), 5);

            Assert.AreEqual(new Complex(-2 - Math.Sqrt(3), -4), n1 - n2);
            Assert.AreEqual(new Complex(2 + Math.Sqrt(3), 4), n2 - n1);
        }

        [TestMethod]
        public void Multiplication()
        {
            var n1 = new Complex(1, -1);
            var n2 = new Complex(3, 6);

            Assert.AreEqual(new Complex(9, 3), n1 * n2);
        }

        [TestMethod]
        public void Division()
        {
            var n1 = new Complex(13, 1);
            var n2 = new Complex(7, -6);

            Assert.AreEqual(new Complex(1, 1), n1 / n2);

            n1 = 1;
            n2 = new Complex(Math.Sqrt(3), 1);

            Assert.AreEqual(new Complex(Math.Sqrt(3) / 4, -1f / 4), n1 / n2);
        }

        [TestMethod]
        public void Exponentiation()
        {
            var n = new Complex(2, -Math.Sqrt(3));

            Assert.AreEqual(new Complex(11041, -7316 * Math.Sqrt(3)), Complex.Pow(n, 10));
        }

        [TestMethod]
        public void Root()
        {
            var n = new Complex(-1f / 2, Math.Sqrt(3) / 2);
            var expecting = new Complex[] { 1, n, ~n };

            foreach (var i in Complex.TakeRoot(1, 3))
                Assert.AreEqual(true, Array.Exists(expecting, a => a == i));
        }
    }

    [TestClass]
    public class TestQuadraticEquationSolver
    {
        static void TestEquation(double r0, double r1, double r2, params Complex[] exp)
        {
            var result = new QuadraticEquation(r0, r1, r2).GetSolutions();

            Assert.AreEqual(exp.Length, result.Length);

            foreach (var i in result)
                Assert.AreEqual(true, Array.Exists(exp, a => a == i));
        }

        [TestMethod]
        public void PositiveDiscriminant()
        {
            TestEquation(6, 11, -35, 5f / 3, -7f / 2);
            TestEquation(2, -4, -2, 1 + Math.Sqrt(2), 1 - Math.Sqrt(2));
            TestEquation(-4, -7, 12, (-7 + Math.Sqrt(241)) / 8, (-7 - Math.Sqrt(241)) / 8);
        }

        [TestMethod]
        public void ZeroDiscriminant()
        {
            TestEquation(1, 2, 1, -1, -1);
            TestEquation(1, -2, 1, 1, 1);
            TestEquation(1, -6, 9, 3, 3);
            TestEquation(3, -6f / 17, 3f / 289, 1f / 17, 1f / 17);
        }

        [TestMethod]
        public void NegativeDiscriminant()
        {
            TestEquation(1, 1, 9, new Complex(-1, Math.Sqrt(35)) / 2, new Complex(-1, -Math.Sqrt(35)) / 2);
        }

        [TestMethod]
        public void RandomTestsWithRealCoefficients()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                Random random = new Random();

                var r0 = Math.Round(random.NextDouble() * 10000) - 5000;
                if (r0 == 0)
                    continue;
                var r1 = Math.Round(random.NextDouble() * 10000) - 5000;
                var r2 = Math.Round(random.NextDouble() * 10000) - 5000;

                foreach (var a in new QuadraticEquation(r0, r1, r2).GetSolutions())
                {
                    Assert.AreEqual(0, r0 * a * a + r1 * a + r2);
                }
            }
        }

        [TestMethod]
        public void RandomTestsWithComplexCoefficients()
        {
            Random random = new Random();

            for (int i = 0; i < 1_000_000; i++)
            {
                var n0 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);
                if (n0.Re == 0 && n0.Im == 0)
                    continue;
                var n1 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);
                var n2 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);

                foreach (var a in new QuadraticEquation(n0, n1, n2).GetSolutions())
                {
                    Assert.AreEqual(0, n0 * a * a + n1 * a + n2);
                }
            }
        }
    }

    [TestClass]
    public class TestCubicEquationSolver
    {
        static void TestEquation(double r0, double r1, double r2, double r3, params Complex[] exp)
        {
            var result = new CubicEquation(r0, r1, r2, r3).GetSolutions();

            Assert.AreEqual(exp.Length, result.Length);

            foreach (var i in result)
                Assert.AreEqual(true, Array.Exists(exp, a => a == i));
        }

        [TestMethod]
        public void ZeroDiscriminant()
        {
            TestEquation(1, 0, -12, 16, -4, 2, 2);
            TestEquation(1, -5, 7, -3, 1, 1, 3);
        }

        [TestMethod]
        public void NegativeDiscriminant()
        {
            TestEquation(1, -7, 14, -8, 1, 2, 4);
            TestEquation(2, -5, -2, 2, -0.7320508076, 2.7320508076, 0.5);
            TestEquation(5, -8, -8, 5, -1, 0.46933761, 2.13066238);
            TestEquation(1, -3, -13, 15, -3, 1, 5);
            TestEquation(1, 1, -4, -4, -2, -1, 2);
        }

        [TestMethod]
        public void PositiveDiscriminant()
        {
            Complex n;

            n = new Complex(-1.5, Math.Sqrt(3) / 2);
            TestEquation(1, 4, 6, 3, -1, n, ~n);

            n = new Complex(-1.62996052, 1.09112364);
            TestEquation(1, 3, 3, -1, 0.25992105, n, ~n);

            n = new Complex(-2.07721735, 1.86579517);
            TestEquation(1, 3, 3, -9, 1.15443469, n, ~n);

            n = new Complex(-2.72112479, 1.24902477);
            TestEquation(1, 6, 12, 5, -0.55775043, n, ~n);

            n = new Complex(-0.5, 1.93649167);
            TestEquation(1, 0, 3, -4, 1, n, ~n);

            n = new Complex(-0.5, 1.32287566);
            TestEquation(1, 0, 1, -2, 1, n, ~n);

            n = new Complex(-0.23278562, 0.79255199);
            TestEquation(1, -1, 0, -1, 1.46557123, n, ~n);
        }

        [TestMethod]
        public void RandomTestsWithRealCoefficients()
        {
            Random random = new Random();

            for (int i = 0; i < 1_000_000; i++)
            {
                var r0 = random.NextDouble() * 100 + 1;
                var r1 = random.NextDouble() * 100 + 1;
                var r2 = random.NextDouble() * 100 + 1;
                var r3 = random.NextDouble() * 100 + 1;

                var answers = new CubicEquation(r0, r1, r2, r3).GetSolutions();
                foreach (var a in answers)
                {
                    Assert.AreEqual(0, r0 * a * a * a + r1 * a * a + r2 * a + r3);
                }
            }
        }

        [TestMethod]
        public void RandomTestsWithComplexCoefficients()
        {
            Random random = new Random();

            for (int i = 0; i < 1_000_000; i++)
            {
                var n0 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);
                if (n0.Re == 0 && n0.Im == 0)
                    continue;
                var n1 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);
                var n2 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);
                var n3 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);

                foreach (var a in new CubicEquation(n0, n1, n2, n3).GetSolutions())
                {
                    Assert.AreEqual(0, n0 * a * a * a + n1 * a * a + n2 * a + n3);
                }
            }
        }
    }

    [TestClass]
    public class TestQuarticEquationSolver
    {
        [TestMethod]
        public void RandomTestsWithRealCoefficients()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                Random random = new Random();

                var r0 = Math.Round(random.NextDouble() * 1000) - 500;
                if (r0 == 0)
                    continue;
                var r1 = Math.Round(random.NextDouble() * 1000) - 500;
                var r2 = Math.Round(random.NextDouble() * 1000) - 500;
                var r3 = Math.Round(random.NextDouble() * 1000) - 500;
                var r4 = Math.Round(random.NextDouble() * 1000) - 500;

                var answers = new QuarticEquation(r0, r1, r2, r3, r4).GetSolutions();
                foreach (var a in answers)
                {
                    Assert.AreEqual(0, r0 * a * a * a * a + r1 * a * a * a + r2 * a * a + r3 * a + r4);
                }
            }
        }

        [TestMethod]
        public void RandomTestsWithComplexCoefficients()
        {
            Random random = new Random();

            for (int i = 0; i < 1_000_000; i++)
            {
                var n0 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);
                if (n0.Re == 0 && n0.Im == 0)
                    continue;
                var n1 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);
                var n2 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);
                var n3 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);
                var n4 = new Complex(random.NextDouble() * 10000 - 5000, random.NextDouble() * 10000 - 5000);

                foreach (var a in new QuarticEquation(n0, n1, n2, n3, n4).GetSolutions())
                {
                    Assert.AreEqual(0, n0 * a * a * a * a + n1 * a * a * a + n2 * a * a + n3 * a + n4);
                }
            }
        }
    }
}
