namespace MathProject
{
    public class CubicEquation
    {
        // x^3 + A * x^2 + B * x + C
        public Complex A { get; }
        public Complex B { get; }
        public Complex C { get; }

        public CubicEquation(Complex r0, Complex r1, Complex r2, Complex r3)
        {
            if (r0 == 0)
                throw new Exception();

            A = r1 / r0;
            B = r2 / r0;
            C = r3 / r0;
        }

        public Complex[] GetSolutions()
        {
            var p = (3 * B - A * A) / 3;
            var q = (2 * A * A * A - 9 * A * B + 27 * C) / 27;
            var shift = -A / 3;

            var s = new QuadraticEquation(27, 27 * q, -p * p * p).GetSolutions();
            if (s[0].Re == 0 && s[0].Im == 0 && s[1].Re == 0 && s[1].Im == 0)
                return new Complex[] { shift, shift, shift };

            var a = Complex.TakeRoot(s[0] == 0 ? s[1] : s[0], 3);

            var answers = new Complex[3];
            for (var i = 0; i < a.Length; i++)
                answers[i] = a[i] - p / (3 * a[i]) + shift;

            return answers;
        }

        public override string ToString()
        {
            // Элементарная реализация строкового представления объекта этого класса. Не заморачиваемся
            return String.Format("x^3 + {0}x^2 + {1}x + {2}", A, B, C);
        }
    }
}
