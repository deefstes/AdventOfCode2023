using NUnit.Framework;

namespace AdventOfCode2023.Utils.Tests
{
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
    }
}