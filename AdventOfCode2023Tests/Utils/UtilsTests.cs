namespace AdventOfCode2023.Utils.Tests
{
    using AdventOfCode2023.Utils;
    using NUnit.Framework;
    using System.Globalization;

    [TestFixture()]
    public class UtilsTests
    {
        [Test()]
        [TestCase("qwerty\r\nasdfgh\r\nzxcvbn", "qwerty,asdfgh,zxcvbn", TestName = "Windows line endings")]
        [TestCase("qwerty\nasdfgh\nzxcvbn", "qwerty,asdfgh,zxcvbn", TestName = "Linux line endings")]
        [TestCase("\r\n\r\nqwerty\r\nasdfgh\r\nzxcvbn", "qwerty,asdfgh,zxcvbn", TestName = "Leading empty lines")]
        [TestCase("qwerty\r\nasdfgh\r\nzxcvbn\r\n\r\n", "qwerty,asdfgh,zxcvbn", TestName = "Trailing empty lines")]
        [TestCase("qwerty\r\n  asdfgh\r\nzxcvbn", "qwerty,asdfgh,zxcvbn", TestName = "Leading whitespace in individual line")]
        [TestCase("qwerty\r\nasdfgh  \r\nzxcvbn", "qwerty,asdfgh,zxcvbn", TestName = "Trailing whitespace in individual line")]
        [TestCase(" qwerty\r\n  asdfgh\r\nzxcvbn", "qwerty,asdfgh,zxcvbn", TestName = "Leading whitespace in first line")]
        [TestCase("qwerty\r\nasdfgh  \r\nzxcvbn ", "qwerty,asdfgh,zxcvbn", TestName = "Trailing whitespace in last line")]
        public void AsListTest(string input, string output)
        {
            Assert.That(string.Join(",", input.AsList().ToArray()), Is.EqualTo(output));
        }

        [Test()]
        [TestCase(0, 0, 12, 345, 123, "12.345s")]
        [TestCase(0, 0, 0, 0, 123, "123μs")]
        [TestCase(0, 0, 0, 1, 123, "1.123ms")]
        [TestCase(1, 23, 45, 67, 89, "1h 23m 45.067s")]
        public void FormatTimeSpanTest(double hours, double minutes, double seconds, double milliseconds, double microseconds, string expected)
        {
            TimeSpan h = TimeSpan.FromHours(hours);
            TimeSpan m = TimeSpan.FromMinutes(minutes);
            TimeSpan s = TimeSpan.FromSeconds(seconds);
            TimeSpan ms = TimeSpan.FromMilliseconds(milliseconds);
            TimeSpan us = TimeSpan.FromMicroseconds(microseconds);

            var ts = h + m + s + ms + us;

            Assert.That(ts.Format(), Is.EqualTo(expected));
        }

        [Test()]
        [TestCase(8, 12, 4)]
        [TestCase(24, 36, 12)]
        [TestCase(12, 18, 6)]
        [TestCase(9, 27, 9)]
        [TestCase(0, 5, 5)]
        [TestCase(-6, 9, 3)]
        [TestCase(12, 0, 12)]
        [TestCase(-15, -25, 5)]
        public void GcdTest_Int(int a, int b, int expected)
        {
            Assert.That(Utils.Gcd(a, b), Is.EqualTo(expected));
        }

        [Test()]
        [TestCase(8L, 12L, 4L)]
        [TestCase(24L, 36L, 12L)]
        [TestCase(12L, 18L, 6L)]
        [TestCase(9L, 27L, 9L)]
        [TestCase(0L, 5L, 5L)]
        [TestCase(-6L, 9L, 3L)]
        [TestCase(12L, 0L, 12L)]
        [TestCase(-15L, -25L, 5L)]
        public void GcdTest_Long(long a, long b, long expected)
        {
            Assert.That(Utils.Gcd(a, b), Is.EqualTo(expected));
        }

        [Test()]
        [TestCase(3, 5, 15)]
        [TestCase(-4, 6, 12)]
        [TestCase(0, 8, 0)]
        [TestCase(7, -14, 14)]
        public void LcmTest_Pairs_Ints(int a, int b, int expected)
        {
            int[] pair = [a, b];
            Assert.That(Utils.Lcm(pair), Is.EqualTo(expected));
        }

        [Test()]
        [TestCase(2L, 4L, 6L, 8L, 24L)]
        [TestCase(-3L, 5L, -7L, 10L, 210L)]
        [TestCase(0L, 8L, 12L, -24L, 0L)]
        [TestCase(-6L, -9L, 15L, 18L, 90L)]
        public void LcmTest_Quads_Longs(long a, long b, long c, long d, long expected)
        {
            long[] quad = [a, b, c, d];
            Assert.That(Utils.Lcm(quad), Is.EqualTo(expected));
        }

        [Test()]
        [TestCase("Kitten", "Sitten", 1)]
        [TestCase("Sittin", "Sitting", 1)]
        [TestCase("Uninformed", "Uniformed", 1)]
        [TestCase("String One", "String Uno", 2)]
        [TestCase("flaw", "lawn", 2)]
        [TestCase("saturday", "sunday", 3)]
        [TestCase("abc", "xyz", 3)]
        [TestCase("dog", "dogs", 1)]
        [TestCase("abcdef", "aecdbf", 2)]
        [TestCase("abcdef", "azcedb", 4)]
        public void LevenshteinTest(string s1, string s2, int expected)
        {
            Assert.That(Utils.Levenshtein(s1, s2), Is.EqualTo(expected));
        }

        [Test()]
        [TestCase("123\r\n456\r\n789", 0, "147")]
        [TestCase("00000\r\n11111\r\n22222\r\n12345", 2, "0123")]
        public void ColToStringTest(string input, int col, string expected)
        {
            var grid = input.AsGrid();
            Assert.That(grid.ColToString(col), Is.EqualTo(expected));
        }

        [Test()]
        [TestCase("123\r\n456\r\n789", 0, "123")]
        [TestCase("00000\r\n11111\r\n22222\r\n12345", 2, "22222")]
        public void RowToStringTest(string input, int col, string expected)
        {
            var grid = input.AsGrid();
            Assert.That(grid.RowToString(col), Is.EqualTo(expected));
        }

        [Test()]
        [TestCase("2,7,34|5,-4,-1", "3,4")]
        [TestCase("2,-1,1,8|-3,3,9,3|-2,1,4,2", "1,-4,2")]
        [TestCase("2,-3,1,4,-5,10|1,2,-1,3,2,5|-3,1,2,-1,4,-8|4,5,-3,2,1,3|-2,3,1,-5,6,1", "10.5884,-7.9694,1.7585,-0.5714,6.9116")]
        [TestCase("25,5,1,7|100,10,1,62|10000,100,1,9602", "1,-4,2")]
        public void SolveLinearEqSystemTest(string inputMatrix, string expectedArray)
        {
            // Arrange
            var rows = inputMatrix.Split('|');
            var coefficientsMatrix = new double[rows.Length, rows.Length+1];
            for (int row = 0; row < rows.Length; row++)
            {
                var cols = rows[row].Split(',');
                for (int col = 0; col < rows.Length + 1; col++)
                    coefficientsMatrix[row, col] = double.Parse(cols[col]);
            }

            // Act
            var indeterminates = Utils.SolveLinearEqSystem(coefficientsMatrix);

            // Assert
            Assert.That(string.Join(',', indeterminates.Select(x => x.ToString("0.####", new CultureInfo("en-US")))), Is.EqualTo(expectedArray));
        }

        [Test()]
        [TestCase("7,5|62,10|9602,100", "1,-4,2")]
        [TestCase("16,6|50,10|1594,50", "0.6841,-2.4455,6.0455")]
        public void DiscoverPolynomialTest(string solutionsString, string expectedArray)
        {
            // Arrange
            List<(long y, long x)> solutions = [];
            foreach (var solution in solutionsString.Split('|'))
            {
                long y = long.Parse(solution.Split(',')[0]);
                long x = long.Parse(solution.Split(',')[1]);
                solutions.Add((y, x));
            }

            // Act
            var indeterminates = Utils.DiscoverPolynomial(solutions);

            // Assert
            Assert.That(string.Join(',', indeterminates.Select(x => x.ToString("0.####", new CultureInfo("en-US")))), Is.EqualTo(expectedArray));
        }

        [Test()]
        [TestCase("4,-3,8", 6, 134)]
        public void SolvePolynomialTest(string indeterminates, long x, double y)
        {
            // Arrange
            var indt = indeterminates.Split(",").Select(double.Parse).ToArray();

            // Act
            var resp = Utils.SolvePolynomial(indt, x);

            // Assert
            Assert.That(resp, Is.EqualTo(y));
        }
    }
}