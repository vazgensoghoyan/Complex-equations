namespace MathProject
{
    public class QuarticEquation
    {
        // x^4 + A * x^3 + B * x^2 + C * x + D
        public Complex A { get; }
        public Complex B { get; }
        public Complex C { get; }
        public Complex D { get; }

        public QuarticEquation(Complex r0, Complex r1, Complex r2, Complex r3, Complex r4)
        {
            if (r0 == 0)
                throw new Exception();

            A = r1 / r0;
            B = r2 / r0;
            C = r3 / r0;
            D = r4 / r0;
        }

        public Complex[] GetSolutions()
        {
            // y^4 + m * y^2 + n * y + p
            var m = (8 * B - 3 * A * A) / 8;
            var n = (8 * C - 4 * A * B + A * A * A) / 8;
            var p = (16 * B * A * A + 256 * D - 64 * A * C - 3 * A * A * A * A) / 256;
            var shift = -A / 4;

            if (m == 0 && n == 0 && p == 0)
                return new Complex[] { shift, shift, shift, shift };

            var cubicAnswers = new CubicEquation
                (1, m, (m * m - 4 * p) / 4, -n * n / 8).GetSolutions();

            Complex t = 0;
            foreach (var i in cubicAnswers)
                if (i != 0)
                {
                    t = i;
                    break;
                }

            var s = Complex.TakeRoot(2 * t, 2)[0];

            var one = new QuadraticEquation(1, -s, m / 2 + t + n / (2 * s)).GetSolutions();
            var two = new QuadraticEquation(1, s, m / 2 + t - n / (2 * s)).GetSolutions();

            var ans = new Complex[4];
            Array.Copy(one, ans, 2);
            Array.Copy(two, 0, ans, 2, 2);

            for (var i = 0; i < ans.Length; i++)
                ans[i] += shift;

            return ans;
        }

        public override string ToString()
        {
            return String.Format("x^4 + ({0})x^3 + ({1})x^2 + ({2})x + ({3})", A, B, C, D);
        }
    }
}
