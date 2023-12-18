using AdventOfCode2023.Day01;
using NUnit.Framework;

namespace AdventOfCode2023.Tests
{
    [TestFixture()]
    public class ExperimentTests
    {
        [Test()]
        public void ExperimentTest()
        {
            Experiment experiment = new Experiment();

            var result = experiment.Run();

            Console.WriteLine(result);
        }
    }
}