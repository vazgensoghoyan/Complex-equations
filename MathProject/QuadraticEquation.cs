namespace MathProject
{
    public class QuadraticEquation
    {
        // x^2 + A * x + B
        public Complex A { get; }
        public Complex B { get; }

        public QuadraticEquation(Complex r0, Complex r1, Complex r2)
        {
            if (r0 == 0)
                throw new Exception();

            A = r1 / r0;
            B = r2 / r0;
        }

        public Complex[] GetSolutions()
        {
            var ans = Complex.TakeRoot(A * A / 4 - B, 2);

            for (var i = 0; i < ans.Length; i++)
                ans[i] -= A / 2;

            return ans;
        }

        public override string ToString()
        {
            return String.Format("x^2 + {0}x + {1}", A, B);
        }
    }
}
