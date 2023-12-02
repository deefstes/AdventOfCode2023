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
    }
}